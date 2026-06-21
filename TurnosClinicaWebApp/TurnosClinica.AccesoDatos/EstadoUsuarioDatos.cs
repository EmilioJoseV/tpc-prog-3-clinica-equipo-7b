using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class EstadoUsuarioDatos
    {
        private readonly AccesoDatos accesoDatos;

        public EstadoUsuarioDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public EstadoUsuarioDatos(AccesoDatos accesoDatosCompartido)
        {
            accesoDatos = accesoDatosCompartido;
        }

        public List<EstadoUsuario> Listar(bool? activo)
        {
            List<EstadoUsuario> estados = new List<EstadoUsuario>();
            try
            {
                string consulta = "SELECT IdEstadoUsuario, Nombre, Descripcion, Activo FROM EstadosUsuario WHERE 1=1";

                if (activo.HasValue)
                {
                    consulta += " AND Activo = @activo";
                }

                accesoDatos.setearConsulta(consulta);

                if (activo.HasValue)
                {
                    accesoDatos.setearParametro("@activo", activo.Value);
                }

                accesoDatos.ejecutarLectura();
                while (accesoDatos.Lector.Read())
                {
                    estados.Add(MapearFilaAEntidad(accesoDatos.Lector));
                }

                return estados;
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

        public EstadoUsuario ObtenerPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return null;
            }

            EstadoUsuario estadoUsuario = null;
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT IdEstadoUsuario, Nombre, Descripcion, Activo"
                    + " FROM EstadosUsuario"
                    + " WHERE UPPER(Nombre) = UPPER(@nombre)");
                accesoDatos.setearParametro("@nombre", nombre.Trim());
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    estadoUsuario = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return estadoUsuario;
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

        private EstadoUsuario MapearFilaAEntidad(SqlDataReader fila)
        {
            return new EstadoUsuario
            {
                IdEstadoUsuario = Convert.ToInt32(fila["IdEstadoUsuario"]),
                Nombre = fila["Nombre"].ToString(),
                Descripcion = fila["Descripcion"] is DBNull ? string.Empty : fila["Descripcion"].ToString(),
                Activo = Convert.ToBoolean(fila["Activo"])
            };
        }
    }
}
