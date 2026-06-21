using System;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class PersonaDatos
    {
        private readonly AccesoDatos accesoDatos;
        private readonly bool esCompartido;

        public PersonaDatos()
        {
            accesoDatos = new AccesoDatos();
            esCompartido = false;
        }

        public PersonaDatos(AccesoDatos accesoDatosCompartido)
        {
            accesoDatos = accesoDatosCompartido;
            esCompartido = true;
        }

        public int Agregar(Persona persona)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "INSERT INTO Personas (DNI, Nombre, Apellido, Telefono, Email)"
                    + " OUTPUT INSERTED.IdPersona"
                    + " VALUES (@dni, @nombre, @apellido, @telefono, @email)");
                accesoDatos.setearParametro("@dni", string.IsNullOrWhiteSpace(persona.DNI) ? (object)DBNull.Value : persona.DNI);
                accesoDatos.setearParametro("@nombre", persona.Nombre);
                accesoDatos.setearParametro("@apellido", persona.Apellido);
                accesoDatos.setearParametro("@telefono", string.IsNullOrWhiteSpace(persona.Telefono) ? (object)DBNull.Value : persona.Telefono);
                accesoDatos.setearParametro("@email", persona.Email);
                return accesoDatos.ejecutarAccionScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!esCompartido)
                {
                    accesoDatos.cerrarConexion();
                }
            }
        }

        public void Modificar(Persona persona)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "UPDATE Personas"
                    + " SET DNI = @dni, Nombre = @nombre, Apellido = @apellido, Telefono = @telefono, Email = @email"
                    + " WHERE IdPersona = @idPersona");
                accesoDatos.setearParametro("@idPersona", persona.IdPersona);
                accesoDatos.setearParametro("@dni", string.IsNullOrWhiteSpace(persona.DNI) ? (object)DBNull.Value : persona.DNI);
                accesoDatos.setearParametro("@nombre", persona.Nombre);
                accesoDatos.setearParametro("@apellido", persona.Apellido);
                accesoDatos.setearParametro("@telefono", string.IsNullOrWhiteSpace(persona.Telefono) ? (object)DBNull.Value : persona.Telefono);
                accesoDatos.setearParametro("@email", persona.Email);
                accesoDatos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!esCompartido)
                {
                    accesoDatos.cerrarConexion();
                }
            }
        }

        public Persona ObtenerPorId(int idPersona)
        {
            Persona persona = new Persona();
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT IdPersona, DNI, Nombre, Apellido, Telefono, Email"
                    + " FROM Personas"
                    + " WHERE IdPersona = @idPersona");
                accesoDatos.setearParametro("@idPersona", idPersona);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    persona = new Persona
                    {
                        IdPersona = Convert.ToInt32(accesoDatos.Lector["IdPersona"]),
                        DNI = accesoDatos.Lector["DNI"] is DBNull ? string.Empty : accesoDatos.Lector["DNI"].ToString(),
                        Nombre = accesoDatos.Lector["Nombre"].ToString(),
                        Apellido = accesoDatos.Lector["Apellido"].ToString(),
                        Telefono = accesoDatos.Lector["Telefono"] is DBNull ? string.Empty : accesoDatos.Lector["Telefono"].ToString(),
                        Email = accesoDatos.Lector["Email"].ToString()
                    };
                }

                return persona;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!esCompartido)
                {
                    accesoDatos.cerrarConexion();
                }
            }
        }

        public Persona ObtenerPorDni(string dni)
        {
            Persona persona = null;
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT IdPersona, DNI, Nombre, Apellido, Telefono, Email"
                    + " FROM Personas"
                    + " WHERE DNI = @dni");
                accesoDatos.setearParametro("@dni", dni);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    persona = new Persona
                    {
                        IdPersona = Convert.ToInt32(accesoDatos.Lector["IdPersona"]),
                        DNI = accesoDatos.Lector["DNI"].ToString(),
                        Nombre = accesoDatos.Lector["Nombre"].ToString(),
                        Apellido = accesoDatos.Lector["Apellido"].ToString(),
                        Telefono = accesoDatos.Lector["Telefono"] is DBNull ? string.Empty : accesoDatos.Lector["Telefono"].ToString(),
                        Email = accesoDatos.Lector["Email"].ToString()
                    };
                }

                return persona;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!esCompartido)
                {
                    accesoDatos.cerrarConexion();
                }
            }
        }
    }
}
