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
            usuarioDatos = new UsuarioDatos(new AccesoDatosBase());
        }

        public UsuarioNegocio(AccesoDatosBase accesoDatos)
        {
            usuarioDatos = new UsuarioDatos(accesoDatos);
        }

        public List<Usuario> Listar()
        {
            return usuarioDatos.Listar(null, null);
        }

        public Usuario ObtenerPorId(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                throw new ArgumentException("El id de usuario no es valido.");
            }

            return usuarioDatos.ObtenerPorId(idUsuario);
        }

        public Usuario ObtenerPorIdPersona(int idPersona)
        {
            if (idPersona <= 0)
            {
                return null;
            }

            return usuarioDatos.ObtenerPorIdPersona(idPersona);
        }

        public void Agregar(Usuario usuario)
        {
            PrepararEstadoInicial(usuario);
            ValidarAlta(usuario);
            usuarioDatos.Agregar(usuario);
        }

        public void AgregarConPersona(Usuario usuario)
        {
            PrepararEstadoInicial(usuario);

            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    PersonaNegocio personaNegocio = new PersonaNegocio(manejador.CrearAccesoDatos());
                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio(manejador.CrearAccesoDatos());

                    usuario.Persona.IdPersona = personaNegocio.Agregar(usuario.Persona);
                    usuarioNegocio.Agregar(usuario);

                    manejador.Confirmar();
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }
        }

        public void Modificar(Usuario usuario)
        {
            ValidarModificacion(usuario);
            usuarioDatos.Modificar(usuario);
        }

        public void ModificarConPersona(Usuario usuario)
        {
            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    PersonaNegocio personaNegocio = new PersonaNegocio(manejador.CrearAccesoDatos());
                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio(manejador.CrearAccesoDatos());

                    personaNegocio.Modificar(usuario.Persona);
                    usuarioNegocio.Modificar(usuario);

                    manejador.Confirmar();
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }
        }

        public void Desactivar(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                throw new ArgumentException("El id de usuario no es valido para desactivacion.");
            }

            usuarioDatos.Desactivar(idUsuario);
        }

        public void Activar(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                throw new ArgumentException("El id de usuario no es valido para activacion.");
            }

            usuarioDatos.AltaLogica(idUsuario);
        }

        private void PrepararEstadoInicial(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new Exception("El usuario es obligatorio.");
            }

            usuario.EstadoUsuario = new EstadoUsuario
            {
                Nombre = EstadoUsuarioEnum.Pendiente.ToString()
            };
        }

        private void ValidarAlta(Usuario usuario)
        {
            ValidarUsuario(usuario);

            if (usuario.Persona.IdPersona <= 0)
            {
                throw new Exception("La persona del usuario debe existir.");
            }
        }

        private void ValidarModificacion(Usuario usuario)
        {
            ValidarUsuario(usuario);

            if (usuario.IdUsuario <= 0)
            {
                throw new Exception("El id de usuario no es valido.");
            }

            if (usuario.Persona.IdPersona <= 0)
            {
                throw new Exception("La persona del usuario debe existir.");
            }
        }

        private void ValidarUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new Exception("El usuario es obligatorio.");
            }

            if (usuario.Persona == null)
            {
                throw new Exception("La persona del usuario es obligatoria.");
            }

            if (usuario.Rol == null || string.IsNullOrWhiteSpace(usuario.Rol.Nombre) || !Enum.TryParse(usuario.Rol.Nombre, true, out RolEnum _))
            {
                throw new Exception("Debe asignar un rol valido al usuario.");
            }

            if (usuario.EstadoUsuario == null || string.IsNullOrWhiteSpace(usuario.EstadoUsuario.Nombre))
            {
                throw new Exception("Debe asignar un estado valido al usuario.");
            }

            usuario.NombreUsuario = string.IsNullOrWhiteSpace(usuario.NombreUsuario) ? null : usuario.NombreUsuario.Trim();
            usuario.PasswordHash = string.IsNullOrWhiteSpace(usuario.PasswordHash) ? null : usuario.PasswordHash.Trim();

            if (!string.IsNullOrWhiteSpace(usuario.NombreUsuario))
            {
                Usuario usuarioActual = usuario.IdUsuario > 0 ? usuarioDatos.ObtenerPorId(usuario.IdUsuario) : null;
                bool nombreUsuarioCambio = usuarioActual == null
                    || !string.Equals(usuarioActual.NombreUsuario, usuario.NombreUsuario, StringComparison.OrdinalIgnoreCase);

                if (nombreUsuarioCambio && usuarioDatos.ExisteNombreUsuario(usuario.NombreUsuario))
                {
                    throw new Exception("Ya existe un nombre de usuario registrado con ese valor.");
                }
            }
        }
    }
}
