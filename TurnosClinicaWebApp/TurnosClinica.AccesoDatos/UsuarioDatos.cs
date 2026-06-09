using System;
using System.Collections.Generic;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class UsuarioDatos
    {
        private readonly AccesoDatos accesoDatos;

        public UsuarioDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<Usuario> Listar()
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                return usuarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Usuario ObtenerPorId(int idUsuario)
        {
            Usuario usuario = new Usuario();
            try
            {
                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Usuario> ObtenerTodosActivos()
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                return usuarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Usuario ObtenerPorNombreUsuario(string nombreUsuario)
        {
            Usuario usuario = new Usuario();
            try
            {
                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Usuario ObtenerPorEmail(string email)
        {
            Usuario usuario = new Usuario();
            try
            {
                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Usuario ValidarCredenciales(string nombreUsuario, string passwordHash)
        {
            Usuario usuario = new Usuario();
            try
            {
                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Usuario> ListarPorRol(int idRol)
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                return usuarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Usuario> ListarPorMedico(int idMedico)
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                return usuarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExisteNombreUsuario(string nombreUsuario)
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

        public int Agregar(Usuario usuario)
        {
            try
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Modificar(Usuario usuario)
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
    }
}
