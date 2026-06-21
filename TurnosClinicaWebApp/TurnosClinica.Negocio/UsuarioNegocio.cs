using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

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
                return usuarioDatos.Listar(null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Agregar(Usuario usuario)
        {
            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                        throw new Exception("El nombre de usuario es obligatorio.");

                    if (string.IsNullOrWhiteSpace(usuario.DNI))
                        throw new Exception("El DNI es obligatorio.");

                    if (string.IsNullOrWhiteSpace(usuario.Email))
                        throw new Exception("El correo electrónico es obligatorio.");

                    if (usuario.Rol == null || string.IsNullOrWhiteSpace(usuario.Rol.Nombre) || !Enum.TryParse(usuario.Rol.Nombre, true, out RolEnum _))
                        throw new Exception("Debe asignar un Rol válido al usuario.");

                    if (!Enum.IsDefined(typeof(EstadoUsuarioEnum), usuario.EstadoUsuario))
                    {
                        usuario.EstadoUsuario = EstadoUsuarioEnum.Activo;
                    }

                    transaccionDatos.IniciarTransaccion();
                    UsuarioDatos datos = new UsuarioDatos(transaccionDatos.AccesoDatos);
                    datos.Agregar(usuario);
                    transaccionDatos.Confirmar();
                }
                catch (Exception ex)
                {
                    transaccionDatos.Cancelar();
                    throw ex;
                }
            }
        }

        public void Modificar(Usuario usuario)
        {
            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    if (usuario.IdUsuario <= 0)
                        throw new Exception("No se puede modificar un usuario sin un ID válido.");

                    if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                        throw new Exception("El nombre de usuario no puede quedar vacío.");

                    if (string.IsNullOrWhiteSpace(usuario.DNI))
                        throw new Exception("El DNI no puede quedar vacío.");

                    transaccionDatos.IniciarTransaccion();
                    UsuarioDatos datos = new UsuarioDatos(transaccionDatos.AccesoDatos);
                    datos.Modificar(usuario);
                    transaccionDatos.Confirmar();
                }
                catch (Exception ex)
                {
                    transaccionDatos.Cancelar();
                    throw ex;
                }
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

        public List<Usuario> ListarConFiltros(string rolLiteral, string estadoUsuarioLiteral)
        {
            try
            {
                return usuarioDatos.Listar(rolLiteral, estadoUsuarioLiteral);
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

        public Usuario ObtenerPorIdPersona(int idPersona)
        {
            try
            {
                if (idPersona <= 0)
                    return null;

                return usuarioDatos.ObtenerPorIdPersona(idPersona);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
