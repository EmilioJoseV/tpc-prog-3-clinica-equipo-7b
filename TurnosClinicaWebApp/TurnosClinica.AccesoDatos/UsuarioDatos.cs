using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class UsuarioDatos : IFiltrable<Usuario>, IMapeable<Usuario>
    {
        private readonly AccesoDatos accesoDatos;

        public UsuarioDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<Usuario> Listar(int? idRol, int? idMedico, bool? activo)
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

        public Usuario ValidarCredenciales(string nombreUsuario, string password)
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

        public Usuario MapearFilaAEntidad(SqlDataReader fila)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> ListarConFiltros(string campo, string criterio, string filtro, bool? activo)
        {
            throw new NotImplementedException();
        }
    }
}
