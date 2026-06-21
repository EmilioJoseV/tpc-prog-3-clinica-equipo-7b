using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class PacienteDatos : IFiltrable<Paciente>, IMapeable<Paciente>
    {
        private readonly AccesoDatos accesoDatos;
        private readonly PersonaDatos personaDatos;

        public PacienteDatos()
        {
            accesoDatos = new AccesoDatos();
            personaDatos = new PersonaDatos(accesoDatos);
        }

        public PacienteDatos(AccesoDatos accesoDatosCompartido)
        {
            accesoDatos = accesoDatosCompartido;
            personaDatos = new PersonaDatos(accesoDatosCompartido);
        }

        public List<Paciente> Listar(bool? activo)
        {
            return ListarConFiltros(null, null, null, activo);
        }

        public List<Paciente> ListarFiltroRapido(string filtro)
        {
            return ListarConFiltros("Nombre", "Contiene", filtro, null);
        }

        public Paciente ObtenerPorId(int idPaciente)
        {
            Paciente paciente = new Paciente();
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT Pcte.IdPaciente, Pcte.IdPersona, Pcte.FechaNacimiento, Pcte.Direccion, Pcte.Activo,"
                    + " Per.DNI, Per.Nombre, Per.Apellido, Per.Telefono, Per.Email"
                    + " FROM Pacientes Pcte"
                    + " INNER JOIN Personas Per ON Per.IdPersona = Pcte.IdPersona"
                    + " WHERE Pcte.IdPaciente = @idPaciente");
                accesoDatos.setearParametro("@idPaciente", idPaciente);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    paciente = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return paciente;
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

        public Paciente ObtenerPorIdPersona(int idPersona)
        {
            Paciente paciente = new Paciente();
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT Pcte.IdPaciente, Pcte.IdPersona, Pcte.FechaNacimiento, Pcte.Direccion, Pcte.Activo,"
                    + " Per.DNI, Per.Nombre, Per.Apellido, Per.Telefono, Per.Email"
                    + " FROM Pacientes Pcte"
                    + " INNER JOIN Personas Per ON Per.IdPersona = Pcte.IdPersona"
                    + " WHERE Pcte.IdPersona = @idPersona");
                accesoDatos.setearParametro("@idPersona", idPersona);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    paciente = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return paciente;
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

        public void Agregar(Paciente paciente)
        {
            try
            {
                if (paciente.IdPersona <= 0)
                {
                    paciente.IdPersona = personaDatos.Agregar(paciente);
                }
                else
                {
                    personaDatos.Modificar(paciente);
                }

                accesoDatos.setearConsulta(
                    "INSERT INTO Pacientes (IdPersona, FechaNacimiento, Direccion, Activo)"
                    + " VALUES (@idPersona, @fechaNacimiento, @direccion, @activo)");
                accesoDatos.setearParametro("@idPersona", paciente.IdPersona);
                accesoDatos.setearParametro("@fechaNacimiento", paciente.FechaNacimiento);
                accesoDatos.setearParametro("@direccion", string.IsNullOrWhiteSpace(paciente.Direccion) ? (object)DBNull.Value : paciente.Direccion);
                accesoDatos.setearParametro("@activo", paciente.Activo);
                accesoDatos.ejecutarAccion();
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

        public void Modificar(Paciente paciente)
        {
            try
            {
                personaDatos.Modificar(paciente);

                accesoDatos.setearConsulta(
                    "UPDATE Pacientes"
                    + " SET IdPersona = @idPersona, FechaNacimiento = @fechaNacimiento, Direccion = @direccion, Activo = @activo"
                    + " WHERE IdPaciente = @idPaciente");
                accesoDatos.setearParametro("@idPersona", paciente.IdPersona);
                accesoDatos.setearParametro("@fechaNacimiento", paciente.FechaNacimiento);
                accesoDatos.setearParametro("@direccion", string.IsNullOrWhiteSpace(paciente.Direccion) ? (object)DBNull.Value : paciente.Direccion);
                accesoDatos.setearParametro("@activo", paciente.Activo);
                accesoDatos.setearParametro("@idPaciente", paciente.IdPaciente);
                accesoDatos.ejecutarAccion();
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

        public List<Paciente> ListarConFiltros(string campo, string criterio, string filtro, bool? activo)
        {
            List<Paciente> pacientes = new List<Paciente>();
            try
            {
                string consulta = "SELECT Pcte.IdPaciente, Pcte.IdPersona, Pcte.FechaNacimiento, Pcte.Direccion, Pcte.Activo, "
                    + "       Per.DNI, Per.Nombre, Per.Apellido, Per.Telefono, Per.Email "
                    + "FROM Pacientes Pcte "
                    + "INNER JOIN Personas Per ON Per.IdPersona = Pcte.IdPersona WHERE 1=1";

                if (activo.HasValue)
                {
                    consulta += " AND Pcte.Activo = @activo";
                }

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    switch (campo)
                    {
                        case "DNI":
                            consulta += criterio == "Igual a"
                                ? " AND Per.DNI = @filtro"
                                : criterio == "Mayor a"
                                    ? " AND Per.DNI > @filtro"
                                    : " AND Per.DNI < @filtro";
                            break;
                        case "Nombre":
                            consulta += criterio == "Comienza con"
                                ? " AND Per.Nombre LIKE @filtro + '%'"
                                : criterio == "Termina con"
                                    ? " AND Per.Nombre LIKE '%' + @filtro"
                                    : " AND Per.Nombre LIKE '%' + @filtro + '%'";
                            break;
                        case "Apellido":
                            consulta += criterio == "Comienza con"
                                ? " AND Per.Apellido LIKE @filtro + '%'"
                                : criterio == "Termina con"
                                    ? " AND Per.Apellido LIKE '%' + @filtro"
                                    : " AND Per.Apellido LIKE '%' + @filtro + '%'";
                            break;
                        default:
                            consulta += " AND (Per.Nombre LIKE '%' + @filtro + '%' OR Per.Apellido LIKE '%' + @filtro + '%' OR Per.DNI LIKE '%' + @filtro + '%')";
                            break;
                    }
                }

                consulta += " ORDER BY Per.Apellido, Per.Nombre";

                accesoDatos.setearConsulta(consulta);
                if (activo.HasValue)
                {
                    accesoDatos.setearParametro("@activo", activo.Value);
                }
                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    accesoDatos.setearParametro("@filtro", filtro);
                }

                accesoDatos.ejecutarLectura();
                while (accesoDatos.Lector.Read())
                {
                    pacientes.Add(MapearFilaAEntidad(accesoDatos.Lector));
                }

                return pacientes;
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

        public bool Desactivar(int idPaciente)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "UPDATE Pacientes"
                    + " SET Activo = 0"
                    + " WHERE IdPaciente = @idPaciente");
                accesoDatos.setearParametro("@idPaciente", idPaciente);
                accesoDatos.ejecutarAccion();
                return true;
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

        public Paciente MapearFilaAEntidad(SqlDataReader fila)
        {
            Paciente paciente = new Paciente
            {
                IdPaciente = Convert.ToInt32(fila["IdPaciente"]),
                IdPersona = Convert.ToInt32(fila["IdPersona"]),
                DNI = fila["DNI"].ToString(),
                Nombre = fila["Nombre"].ToString(),
                Apellido = fila["Apellido"].ToString(),
                Telefono = fila["Telefono"] is DBNull ? string.Empty : fila["Telefono"].ToString(),
                Email = fila["Email"].ToString(),
                FechaNacimiento = Convert.ToDateTime(fila["FechaNacimiento"]),
                Direccion = fila["Direccion"] is DBNull ? string.Empty : fila["Direccion"].ToString(),
                Activo = Convert.ToBoolean(fila["Activo"])
            };
            return paciente;
        }
    }
}
