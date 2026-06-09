using System;
using System.Collections.Generic;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class RolDatos
    {
        private readonly AccesoDatos accesoDatos;

        public RolDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<Rol> Listar()
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

        public List<Rol> ObtenerTodosActivos()
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

        public Rol ObtenerPorNombre(string nombre)
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
