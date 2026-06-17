using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class PacienteDatos : IFiltrable<Paciente>, IMapeable<Paciente>
    {
        private readonly AccesoDatos accesoDatos;

        public PacienteDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<Paciente> Listar(bool? activo)
        {
            List<Paciente> pacientes = new List<Paciente>();
            try
            {
                string consulta = "SELECT IdPaciente, DNI, Nombre, Apellido, FechaNacimiento, Telefono, Email, Direccion, Activo"
                    + " FROM Pacientes";

                if (activo.HasValue)
                {
                    consulta += " WHERE Activo = @activo";
                }

                consulta += " ORDER BY Apellido, Nombre";

                accesoDatos.setearConsulta(consulta);
                if (activo.HasValue)
                {
                    accesoDatos.setearParametro("@activo", activo.Value);
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

        public List<Paciente> ListarFiltroRapido(string filtro)
        {
            List<Paciente> pacientes = new List<Paciente>();
            try
            {
                string consulta = "SELECT IdPaciente, DNI, Nombre, Apellido, FechaNacimiento, Telefono, Email, Direccion, Activo"
                    + " FROM Pacientes";

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    consulta += " WHERE UPPER(Nombre) LIKE '%' + UPPER(@filtro) + '%'"
                        + " OR UPPER(Apellido) LIKE '%' + UPPER(@filtro) + '%'";
                }

                consulta += " ORDER BY Apellido, Nombre";

                accesoDatos.setearConsulta(consulta);
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

        public Paciente ObtenerPorId(int idPaciente)
        {
            Paciente paciente = new Paciente();
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT IdPaciente, DNI, Nombre, Apellido, FechaNacimiento, Telefono, Email, Direccion, Activo"
                    + " FROM Pacientes"
                    + " WHERE IdPaciente = @idPaciente");
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

        public bool ExisteDni(string dni)
        {
            try
            {
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExisteEmail(string email)
        {
            try
            {
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Agregar(Paciente paciente)
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

        public void Modificar(Paciente paciente)
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

        public List<Paciente> ListarConFiltros(string campo, string criterio, string filtro, bool? activo)
        {
            List<Paciente> pacientes = new List<Paciente>();
            try
            {
                string consulta = "SELECT IdPaciente, DNI, Nombre, Apellido, FechaNacimiento, Telefono, Email, Direccion, Activo"
                    + " FROM Pacientes";
                bool tieneCondicion = false;

                if (activo.HasValue)
                {
                    consulta += tieneCondicion ? " AND Activo = @activo" : " WHERE Activo = @activo";
                    tieneCondicion = true;
                }

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    switch (campo)
                    {
                        case "DNI":
                            consulta += criterio == "Igual a"
                                ? (tieneCondicion ? " AND DNI = @filtro" : " WHERE DNI = @filtro")
                                : criterio == "Mayor a"
                                    ? (tieneCondicion ? " AND DNI > @filtro" : " WHERE DNI > @filtro")
                                    : (tieneCondicion ? " AND DNI < @filtro" : " WHERE DNI < @filtro");
                            tieneCondicion = true;
                            break;
                        case "Nombre":
                            consulta += criterio == "Comienza con"
                                ? (tieneCondicion ? " AND Nombre LIKE @filtro + '%'" : " WHERE Nombre LIKE @filtro + '%'")
                                : criterio == "Termina con"
                                    ? (tieneCondicion ? " AND Nombre LIKE '%' + @filtro" : " WHERE Nombre LIKE '%' + @filtro")
                                    : (tieneCondicion ? " AND Nombre LIKE '%' + @filtro + '%'" : " WHERE Nombre LIKE '%' + @filtro + '%'");
                            tieneCondicion = true;
                            break;
                        case "Apellido":
                            consulta += criterio == "Comienza con"
                                ? (tieneCondicion ? " AND Apellido LIKE @filtro + '%'" : " WHERE Apellido LIKE @filtro + '%'")
                                : criterio == "Termina con"
                                    ? (tieneCondicion ? " AND Apellido LIKE '%' + @filtro" : " WHERE Apellido LIKE '%' + @filtro")
                                    : (tieneCondicion ? " AND Apellido LIKE '%' + @filtro + '%'" : " WHERE Apellido LIKE '%' + @filtro + '%'");
                            tieneCondicion = true;
                            break;
                        default:
                            consulta += tieneCondicion
                                ? " AND (Nombre LIKE '%' + @filtro + '%' OR Apellido LIKE '%' + @filtro + '%')"
                                : " WHERE (Nombre LIKE '%' + @filtro + '%' OR Apellido LIKE '%' + @filtro + '%')";
                            tieneCondicion = true;
                            break;
                    }
                }

                consulta += " ORDER BY Apellido, Nombre";

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
            Paciente paciente = new Paciente();
            paciente.IdPaciente = Convert.ToInt32(fila["IdPaciente"]);
            paciente.DNI = fila["DNI"].ToString();
            paciente.Nombre = fila["Nombre"].ToString();
            paciente.Apellido = fila["Apellido"].ToString();
            paciente.FechaNacimiento = Convert.ToDateTime(fila["FechaNacimiento"]);
            paciente.Telefono = fila["Telefono"] is DBNull ? string.Empty : fila["Telefono"].ToString();
            paciente.Email = fila["Email"].ToString();
            paciente.Direccion = fila["Direccion"] is DBNull ? string.Empty : fila["Direccion"].ToString();
            paciente.Activo = Convert.ToBoolean(fila["Activo"]);
            return paciente;
        }
    }
}
