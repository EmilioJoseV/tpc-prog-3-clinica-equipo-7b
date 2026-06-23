using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class EspecialidadDatos : IEntidadGestionable<Especialidad>, IMapeable<Especialidad>
    {
        private readonly AccesoDatosBase accesoDatos;

        public EspecialidadDatos(AccesoDatosBase accesoDatos)
        {
            this.accesoDatos = accesoDatos;
        }

        public List<Especialidad> Listar(bool? activo = null)
        {
            return ListarFiltroAvanzado(null, null, null, activo);
        }

        public List<Especialidad> ListarFiltroRapido(string palabra, bool? activo = null)
        {
            return ListarFiltroAvanzado(null, null, palabra, activo);
        }

        public List<Especialidad> ListarFiltroAvanzado(string campo, string criterio, string filtro, bool? activo = null)
        {
            List<Especialidad> especialidades = new List<Especialidad>();

            try
            {
                string consulta =
                    "SELECT IdEspecialidad, Nombre, Descripcion, Activo "
                    + "FROM Especialidades "
                    + "WHERE 1=1";

                if (activo.HasValue)
                {
                    consulta += " AND Activo = @activo";
                }

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    consulta += ConstruirFiltro(campo, criterio);
                }

                consulta += " ORDER BY Nombre";
                accesoDatos.setearConsulta(consulta);

                if (activo.HasValue)
                {
                    accesoDatos.setearParametro("@activo", activo.Value);
                }

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    accesoDatos.setearParametro("@filtro", filtro.Trim());
                }

                accesoDatos.ejecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    especialidades.Add(MapearFilaAEntidad(accesoDatos.Lector));
                }

                return especialidades;
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public Especialidad ObtenerPorId(int id)
        {
            Especialidad especialidad = null;

            try
            {
                accesoDatos.setearConsulta(
                    "SELECT IdEspecialidad, Nombre, Descripcion, Activo "
                    + "FROM Especialidades "
                    + "WHERE IdEspecialidad = @idEspecialidad");
                accesoDatos.setearParametro("@idEspecialidad", id);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    especialidad = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return especialidad;
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public int Agregar(Especialidad especialidad)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "INSERT INTO Especialidades (Nombre, Descripcion, Activo) "
                    + "VALUES (@nombre, @descripcion, @activo)");
                accesoDatos.setearParametro("@nombre", especialidad.Nombre);
                accesoDatos.setearParametro("@descripcion", string.IsNullOrWhiteSpace(especialidad.Descripcion)
                    ? (object)DBNull.Value
                    : especialidad.Descripcion);
                accesoDatos.setearParametro("@activo", especialidad.Activo);
                accesoDatos.ejecutarAccion();
                return 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public void Modificar(Especialidad especialidad)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "UPDATE Especialidades "
                    + "SET Nombre = @nombre, Descripcion = @descripcion, Activo = @activo "
                    + "WHERE IdEspecialidad = @idEspecialidad");
                accesoDatos.setearParametro("@nombre", especialidad.Nombre);
                accesoDatos.setearParametro("@descripcion", string.IsNullOrWhiteSpace(especialidad.Descripcion)
                    ? (object)DBNull.Value
                    : especialidad.Descripcion);
                accesoDatos.setearParametro("@activo", especialidad.Activo);
                accesoDatos.setearParametro("@idEspecialidad", especialidad.IdEspecialidad);
                accesoDatos.ejecutarAccion();
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public void Desactivar(int id)
        {
            CambiarEstado(id, false);
        }

        public void Activar(int id)
        {
            CambiarEstado(id, true);
        }

        public bool ExisteNombre(string nombre, int excluirId = 0)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT COUNT(*) FROM Especialidades "
                    + "WHERE UPPER(Nombre) = UPPER(@nombre) "
                    + "AND IdEspecialidad <> @excluirId");
                accesoDatos.setearParametro("@nombre", nombre);
                accesoDatos.setearParametro("@excluirId", excluirId);
                return accesoDatos.ejecutarAccionScalar() > 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public bool EstaAsociadaAMedico(int idEspecialidad)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT COUNT(*) FROM MedicosEspecialidades "
                    + "WHERE IdEspecialidad = @idEspecialidad");
                accesoDatos.setearParametro("@idEspecialidad", idEspecialidad);
                return accesoDatos.ejecutarAccionScalar() > 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public Especialidad MapearFilaAEntidad(SqlDataReader fila)
        {
            return new Especialidad
            {
                IdEspecialidad = Convert.ToInt32(fila["IdEspecialidad"]),
                Nombre = fila["Nombre"].ToString(),
                Descripcion = fila["Descripcion"] is DBNull ? string.Empty : fila["Descripcion"].ToString(),
                Activo = Convert.ToBoolean(fila["Activo"])
            };
        }

        private string ConstruirFiltro(string campo, string criterio)
        {
            string campoSql = ObtenerCampoSql(campo);

            if (string.IsNullOrWhiteSpace(campoSql))
            {
                return " AND (Nombre LIKE '%' + @filtro + '%' OR Descripcion LIKE '%' + @filtro + '%')";
            }

            if (string.Equals(criterio, "Igual a", StringComparison.OrdinalIgnoreCase))
            {
                return " AND " + campoSql + " = @filtro";
            }

            if (string.Equals(criterio, "Comienza con", StringComparison.OrdinalIgnoreCase))
            {
                return " AND " + campoSql + " LIKE @filtro + '%'";
            }

            if (string.Equals(criterio, "Termina con", StringComparison.OrdinalIgnoreCase))
            {
                return " AND " + campoSql + " LIKE '%' + @filtro";
            }

            return " AND " + campoSql + " LIKE '%' + @filtro + '%'";
        }

        private string ObtenerCampoSql(string campo)
        {
            if (string.Equals(campo, "Nombre", StringComparison.OrdinalIgnoreCase))
            {
                return "Nombre";
            }

            if (string.Equals(campo, "Descripcion", StringComparison.OrdinalIgnoreCase))
            {
                return "Descripcion";
            }

            return null;
        }

        private void CambiarEstado(int id, bool activo)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "UPDATE Especialidades SET Activo = @activo "
                    + "WHERE IdEspecialidad = @idEspecialidad");
                accesoDatos.setearParametro("@activo", activo);
                accesoDatos.setearParametro("@idEspecialidad", id);
                accesoDatos.ejecutarAccion();
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }
    }
}
