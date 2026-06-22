using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class PacienteDatos : IEntidadGestionable<Paciente>, IMapeable<Paciente>
    {
        private readonly AccesoDatosBase accesoDatos;

        public PacienteDatos(AccesoDatosBase accesoDatos)
        {
            this.accesoDatos = accesoDatos;
        }

        public List<Paciente> Listar(bool? activo = null)
        {
            return ListarFiltroAvanzado(null, null, null, activo);
        }

        public List<Paciente> ListarFiltroRapido(string palabra, bool? activo = null)
        {
            return ListarFiltroAvanzado(null, null, palabra, activo);
        }

        public List<Paciente> ListarFiltroAvanzado(string campo, string criterio, string filtro, bool? activo = null)
        {
            List<Paciente> pacientes = new List<Paciente>();

            try
            {
                string consulta =
                    "SELECT Pcte.IdPaciente, Pcte.IdPersona, Pcte.FechaNacimiento, Pcte.Direccion, Pcte.Activo, "
                    + "Per.DNI, Per.Nombre, Per.Apellido, Per.Telefono, Per.Email "
                    + "FROM Pacientes Pcte "
                    + "INNER JOIN Personas Per ON Per.IdPersona = Pcte.IdPersona "
                    + "WHERE 1=1";

                if (activo.HasValue)
                {
                    consulta += " AND Pcte.Activo = @activo";
                }

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    consulta += ConstruirFiltro(campo, criterio);
                }

                consulta += " ORDER BY Per.Apellido, Per.Nombre";

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
                    pacientes.Add(MapearFilaAEntidad(accesoDatos.Lector));
                }

                return pacientes;
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

        public Paciente ObtenerPorId(int id)
        {
            Paciente paciente = null;

            try
            {
                accesoDatos.setearConsulta(
                    "SELECT Pcte.IdPaciente, Pcte.IdPersona, Pcte.FechaNacimiento, Pcte.Direccion, Pcte.Activo, "
                    + "Per.DNI, Per.Nombre, Per.Apellido, Per.Telefono, Per.Email "
                    + "FROM Pacientes Pcte "
                    + "INNER JOIN Personas Per ON Per.IdPersona = Pcte.IdPersona "
                    + "WHERE Pcte.IdPaciente = @idPaciente");
                accesoDatos.setearParametro("@idPaciente", id);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    paciente = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return paciente;
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

        public int Agregar(Paciente entidad)
        {
            try
            {
                if (entidad == null)
                {
                    throw new Exception("El paciente es obligatorio.");
                }

                accesoDatos.setearConsulta(
                    "INSERT INTO Pacientes (IdPersona, FechaNacimiento, Direccion, Activo) "
                    + "VALUES (@idPersona, @fechaNacimiento, @direccion, @activo)");
                accesoDatos.setearParametro("@idPersona", entidad.Persona.IdPersona);
                accesoDatos.setearParametro("@fechaNacimiento", entidad.FechaNacimiento);
                accesoDatos.setearParametro("@direccion", string.IsNullOrWhiteSpace(entidad.Direccion) ? (object)DBNull.Value : entidad.Direccion.Trim());
                accesoDatos.setearParametro("@activo", entidad.Activo);
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

        public void Modificar(Paciente entidad)
        {
            try
            {
                if (entidad == null)
                {
                    throw new Exception("El paciente es obligatorio.");
                }

                accesoDatos.setearConsulta(
                    "UPDATE Pacientes "
                    + "SET IdPersona = @idPersona, FechaNacimiento = @fechaNacimiento, Direccion = @direccion, Activo = @activo "
                    + "WHERE IdPaciente = @idPaciente");
                accesoDatos.setearParametro("@idPersona", entidad.Persona.IdPersona);
                accesoDatos.setearParametro("@fechaNacimiento", entidad.FechaNacimiento);
                accesoDatos.setearParametro("@direccion", string.IsNullOrWhiteSpace(entidad.Direccion) ? (object)DBNull.Value : entidad.Direccion.Trim());
                accesoDatos.setearParametro("@activo", entidad.Activo);
                accesoDatos.setearParametro("@idPaciente", entidad.IdPaciente);
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

        public Paciente MapearFilaAEntidad(SqlDataReader fila)
        {
            Paciente paciente = new Paciente
            {
                IdPaciente = Convert.ToInt32(fila["IdPaciente"]),
                FechaNacimiento = Convert.ToDateTime(fila["FechaNacimiento"]),
                Direccion = fila["Direccion"] is DBNull ? string.Empty : fila["Direccion"].ToString(),
                Activo = Convert.ToBoolean(fila["Activo"]),
                Persona = new Persona
                {
                    IdPersona = Convert.ToInt32(fila["IdPersona"]),
                    DNI = fila["DNI"].ToString(),
                    Nombre = fila["Nombre"].ToString(),
                    Apellido = fila["Apellido"].ToString(),
                    Telefono = fila["Telefono"] is DBNull ? string.Empty : fila["Telefono"].ToString(),
                    Email = fila["Email"].ToString()
                }
            };

            return paciente;
        }

        private string ConstruirFiltro(string campo, string criterio)
        {
            string campoSql = ObtenerCampoSql(campo);
            if (string.IsNullOrWhiteSpace(campoSql))
            {
                return " AND (Per.DNI LIKE '%' + @filtro + '%' OR Per.Nombre LIKE '%' + @filtro + '%' OR Per.Apellido LIKE '%' + @filtro + '%' OR Per.Email LIKE '%' + @filtro + '%' OR Pcte.Direccion LIKE '%' + @filtro + '%')";
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
            if (string.IsNullOrWhiteSpace(campo))
            {
                return null;
            }

            switch (campo.Trim())
            {
                case "DNI":
                    return "Per.DNI";
                case "Nombre":
                    return "Per.Nombre";
                case "Apellido":
                    return "Per.Apellido";
                case "Email":
                    return "Per.Email";
                case "Direccion":
                    return "Pcte.Direccion";
                default:
                    return null;
            }
        }

        private void CambiarEstado(int id, bool activo)
        {
            try
            {
                if (id <= 0)
                {
                    throw new Exception("El id del paciente no es valido.");
                }

                accesoDatos.setearConsulta(
                    "UPDATE Pacientes "
                    + "SET Activo = @activo "
                    + "WHERE IdPaciente = @idPaciente");
                accesoDatos.setearParametro("@idPaciente", id);
                accesoDatos.setearParametro("@activo", activo);
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
