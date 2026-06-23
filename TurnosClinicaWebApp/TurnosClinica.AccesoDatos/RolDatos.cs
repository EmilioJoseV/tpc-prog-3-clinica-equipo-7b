using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class RolDatos
    {
        private readonly AccesoDatosBase accesoDatos;

        public RolDatos(AccesoDatosBase accesoDatosCompartido)
        {
            accesoDatos = accesoDatosCompartido;
        }

        public List<Rol> Listar(bool? activo)
        {
            List<Rol> roles = new List<Rol>();
            try
            {
                string consulta = "SELECT IdRol, Nombre, Descripcion, Activo FROM Roles WHERE 1=1";

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
                    roles.Add(MapearFilaAEntidad(accesoDatos.Lector));
                }

                return roles;
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

        public Rol ObtenerPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return null;
            }

            Rol rol = null;
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT IdRol, Nombre, Descripcion, Activo"
                    + " FROM Roles"
                    + " WHERE UPPER(Nombre) = UPPER(@nombre)");
                accesoDatos.setearParametro("@nombre", nombre.Trim());
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    rol = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return rol;
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

        private Rol MapearFilaAEntidad(SqlDataReader fila)
        {
            return new Rol
            {
                IdRol = Convert.ToInt32(fila["IdRol"]),
                Nombre = fila["Nombre"].ToString(),
                Descripcion = fila["Descripcion"] is DBNull ? string.Empty : fila["Descripcion"].ToString(),
                Activo = Convert.ToBoolean(fila["Activo"])
            };
        }
    }
}
