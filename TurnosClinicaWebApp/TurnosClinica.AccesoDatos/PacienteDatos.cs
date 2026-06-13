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

        public List<Paciente> Listar(bool activo)
        {
            List<Paciente> pacientes = new List<Paciente>();
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT IdPaciente, DNI, Nombre, Apellido, FechaNacimiento, Telefono, Email, Direccion, Activo"
                    + " FROM Pacientes"
                    + (activo ? " WHERE Activo = 1" : string.Empty)
                    + " ORDER BY Apellido, Nombre");
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
            throw new NotImplementedException();
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
