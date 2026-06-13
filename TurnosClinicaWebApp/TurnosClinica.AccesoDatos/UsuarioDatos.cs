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
                accesoDatos.setearConsulta(
                    "SELECT IdUsuario, NombreUsuario, Email, PasswordHash, Activo, IdRol, IdMedico"
                    + " FROM Usuarios"
                    + " WHERE IdUsuario = @idUsuario");
                accesoDatos.setearParametro("@idUsuario", idUsuario);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    usuario = MapearFilaAEntidad(accesoDatos.Lector);
                }

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

        public void Agregar(Usuario usuario)
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

        public void Modificar(Usuario usuario)
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

        public Usuario MapearFilaAEntidad(SqlDataReader fila)
        {
            Usuario usuario = new Usuario();
            usuario.IdUsuario = Convert.ToInt32(fila["IdUsuario"]);
            usuario.NombreUsuario = fila["NombreUsuario"].ToString();
            usuario.Email = fila["Email"].ToString();
            usuario.PasswordHash = fila["PasswordHash"] is DBNull ? string.Empty : fila["PasswordHash"].ToString();
            usuario.Activo = Convert.ToBoolean(fila["Activo"]);

            if (fila["IdRol"] != DBNull.Value)
            {
                usuario.Rol = new Rol
                {
                    IdRol = Convert.ToInt32(fila["IdRol"])
                };
            }

            if (fila["IdMedico"] != DBNull.Value)
            {
                usuario.Medico = new Medico
                {
                    IdMedico = Convert.ToInt32(fila["IdMedico"])
                };
            }

            return usuario;
        }

        public List<Usuario> ListarConFiltros(string campo, string criterio, string filtro, bool? activo)
        {
            throw new NotImplementedException();
        }
    }
}
