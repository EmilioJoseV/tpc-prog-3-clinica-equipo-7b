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
        private readonly PersonaNegocio personaNegocio;
        private readonly bool usaAccesoCompartido;

        public UsuarioNegocio()
        {
            usuarioDatos = new UsuarioDatos();
            personaNegocio = new PersonaNegocio();
            usaAccesoCompartido = false;
        }

        public UsuarioNegocio(TurnosClinica.AccesoDatos.AccesoDatos accesoDatosCompartido)
        {
            usuarioDatos = new UsuarioDatos(accesoDatosCompartido);
            personaNegocio = new PersonaNegocio(accesoDatosCompartido);
            usaAccesoCompartido = true;
        }

        public List<Usuario> Listar()
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

        public Usuario ObtenerPorId(int idUsuario)
        {
            try
            {
                if (idUsuario <= 0)
                {
                    throw new ArgumentException("El ID de usuario no es válido.");
                }

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
                {
                    return null;
                }

                return usuarioDatos.ObtenerPorIdPersona(idPersona);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Agregar(Usuario usuario)
        {
            usuario.EstadoUsuario = new EstadoUsuario
            {
                Nombre = EstadoUsuarioEnum.Pendiente.ToString()
            };
            ValidarUsuario(usuario, true);
            EjecutarOperacion(datos => datos.Agregar(usuario));
        }

        public void Modificar(Usuario usuario)
        {
            usuario.EstadoUsuario = new EstadoUsuario
            {
                Nombre = EstadoUsuarioEnum.Pendiente.ToString()
            };
            ValidarUsuario(usuario, false);
            EjecutarOperacion(datos => datos.Modificar(usuario));
        }

        public void Desactivar(int idUsuario)
        {
            try
            {
                if (idUsuario <= 0)
                {
                    throw new ArgumentException("ID de usuario no valido para desactivacion.");
                }

                EjecutarOperacion(datos => datos.Desactivar(idUsuario));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValidarUsuario(Usuario usuario, bool esAlta)
        {
            if (usuario == null)
            {
                throw new Exception("El usuario es obligatorio.");
            }

            personaNegocio.ValidarPersona(usuario, esAlta);

            if (usuario.Rol == null || string.IsNullOrWhiteSpace(usuario.Rol.Nombre) || !Enum.TryParse(usuario.Rol.Nombre, true, out RolEnum _))
            {
                throw new Exception("Debe asignar un rol válido al usuario.");
            }

            usuario.NombreUsuario = string.IsNullOrWhiteSpace(usuario.NombreUsuario) ? null : usuario.NombreUsuario.Trim();
            usuario.PasswordHash = string.IsNullOrWhiteSpace(usuario.PasswordHash) ? null : usuario.PasswordHash.Trim();

            if (!string.IsNullOrWhiteSpace(usuario.NombreUsuario))
            {
                Usuario usuarioActual = usuario.IdUsuario > 0 ? usuarioDatos.ObtenerPorId(usuario.IdUsuario) : null;
                bool nombreUsuarioCambio = usuarioActual == null || !string.Equals(usuarioActual.NombreUsuario, usuario.NombreUsuario, StringComparison.OrdinalIgnoreCase);

                if ((esAlta || nombreUsuarioCambio) && usuarioDatos.ExisteNombreUsuario(usuario.NombreUsuario))
                {
                    throw new Exception("Ya existe un nombre de usuario registrado con ese valor.");
                }
            }
        }

        private void EjecutarOperacion(Action<UsuarioDatos> accion)
        {
            if (usaAccesoCompartido)
            {
                accion(usuarioDatos);
                return;
            }

            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();
                    UsuarioDatos datos = new UsuarioDatos(transaccionDatos.AccesoDatos);
                    accion(datos);
                    transaccionDatos.Confirmar();
                }
                catch (Exception ex)
                {
                    transaccionDatos.Cancelar();
                    throw ex;
                }
            }
        }
    }
}
