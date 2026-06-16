using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class UsuarioNegocio
    {
        private readonly UsuarioDatos usuarioDatos;

        public UsuarioNegocio()
        {
            usuarioDatos = new UsuarioDatos();
        }

        public List<Usuario> ListarTodos()
        {
            try
            {
                return usuarioDatos.Listar(null, null, null);
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
                if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                    throw new Exception("El nombre de usuario es obligatorio.");

                if (string.IsNullOrWhiteSpace(usuario.Email))
                    throw new Exception("El correo electrónico es obligatorio.");

                if (usuario.Rol == null || usuario.Rol.IdRol <= 0)
                    throw new Exception("Debe asignar un Rol válido al usuario.");

                usuario.Activo = true;
                usuarioDatos.Agregar(usuario);
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
                if (usuario.IdUsuario <= 0)
                    throw new Exception("No se puede modificar un usuario sin un ID válido.");

                if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                    throw new Exception("El nombre de usuario no puede quedar vacío.");

                usuarioDatos.Modificar(usuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EliminarLogico(int idUsuario)
        {
            try
            {
                if (idUsuario <= 0)
                    throw new ArgumentException("ID de usuario no válido para eliminación.");

                usuarioDatos.EliminarLogico(idUsuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AltaLogica(int idUsuario)
        {
            try
            {
                UsuarioDatos datos = new UsuarioDatos();

                datos.AltaLogica(idUsuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EliminarFisico(int idUsuario)
        {
            try
            {
                if (idUsuario <= 0)
                    throw new ArgumentException("ID de usuario no válido para eliminación.");

                usuarioDatos.EliminarFisico(idUsuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Usuario> ListarConFiltros(int? idRol, int? idMedico, bool? activo)
        {
            try
            {
                return usuarioDatos.Listar(idRol, idMedico, activo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Usuario ObtenerPorId(int idUsuario)
        {
            try
            {
                if (idUsuario <= 0)
                    throw new ArgumentException("El ID de usuario no es válido.");

                return usuarioDatos.ObtenerPorId(idUsuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
