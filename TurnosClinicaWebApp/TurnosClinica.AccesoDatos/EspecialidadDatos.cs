using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class EspecialidadDatos : IFiltrable<Especialidad>, IMapeable<Especialidad>
    {
        private readonly AccesoDatos accesoDatos;
        private readonly MedicoEspecialidadesDatos medicoEspecialidadesDatos;

        public EspecialidadDatos()
        {
            accesoDatos = new AccesoDatos();
            medicoEspecialidadesDatos = new MedicoEspecialidadesDatos();
        }

        public List<Especialidad> Listar(bool? activo)
        {
            List<Especialidad> especialidades = new List<Especialidad>();

            try
            {
                string consulta = "SELECT IdEspecialidad, Nombre, Descripcion, Activo"
                    + " FROM Especialidades";

                if (activo.HasValue)
                {
                    consulta += " WHERE Activo = 1";
                }

                consulta += " ORDER BY Nombre ASC";

                accesoDatos.setearConsulta(consulta);
                accesoDatos.ejecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    especialidades.Add(MapearFilaAEntidad(accesoDatos.Lector));
                }

                return especialidades;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public List<Especialidad> ListarPorMedico(int idMedico, bool? activo)
        {
            return medicoEspecialidadesDatos.ListarPorMedico(idMedico);
        }

        public Especialidad ObtenerPorId(int idEspecialidad)
        {
            Especialidad especialidad = new Especialidad();
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT IdEspecialidad, Nombre, Descripcion, Activo"
                    + " FROM Especialidades"
                    + " WHERE IdEspecialidad = @idEspecialidad");
                accesoDatos.setearParametro("@idEspecialidad", idEspecialidad);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    especialidad = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return especialidad;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public bool ExisteNombre(string nombre)
        {
            try { return false; } catch (Exception ex) { throw ex; }
        }

        public void Agregar(Especialidad especialidad)
        {
            try
            {
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificar(Especialidad especialidad)
        {
            try
            {
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Especialidad> ListarConFiltros(string campo, string criterio, string filtro, bool? activo)
        {
            throw new NotImplementedException();
        }

        public Especialidad MapearFilaAEntidad(SqlDataReader fila)
        {
            Especialidad especialidad = new Especialidad();
            especialidad.IdEspecialidad = Convert.ToInt32(fila["IdEspecialidad"]);
            especialidad.Nombre = fila["Nombre"].ToString();
            especialidad.Descripcion = fila["Descripcion"] is DBNull ? string.Empty : fila["Descripcion"].ToString();
            especialidad.Activo = Convert.ToBoolean(fila["Activo"]);
            return especialidad;
        }
    }
}
