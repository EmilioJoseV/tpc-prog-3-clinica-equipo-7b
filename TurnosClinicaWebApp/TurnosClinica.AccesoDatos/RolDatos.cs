using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class RolDatos : IMapeable<Rol>
    {
        private readonly AccesoDatos accesoDatos;

        public RolDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<Rol> Listar(bool activo)
        {
            List<Rol> roles = new List<Rol>();
            try
            {
                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Rol MapearFilaAEntidad(SqlDataReader fila)
        {
            throw new NotImplementedException();
        }

        public Rol ObtenerPorId(int idRol)
        {
            Rol rol = new Rol();
            try
            {
                return rol;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
