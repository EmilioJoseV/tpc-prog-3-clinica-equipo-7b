using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Negocio
{
    public class UsuarioNegocio : IEntidadGestionableNegocio<Usuario>
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

        public List<Usuario> Listar(bool? activo = null)
        {
            return usuarioDatos.Listar(activo);
        }

        public List<Usuario> ListarFiltroRapido(string palabra, bool? activo = null)
        {
            return usuarioDatos.ListarFiltroRapido(palabra, activo);
        }

        public List<Usuario> ListarFiltroAvanzado(string campo, string criterio, string filtro, bool? activo = null)
        {
            return usuarioDatos.ListarFiltroAvanzado(campo, criterio, filtro, activo);
        }

        public Usuario ObtenerPorId(int id)
        {
            if (id <= 0)
            {
                throw new Exception("El id de usuario no es valido.");
            }

            return usuarioDatos.ObtenerPorId(id);
        }

        public Usuario ObtenerPorIdPersona(int idPersona)
        {
            if (idPersona <= 0)
            {
                return null;
            }

            return usuarioDatos.ObtenerPorIdPersona(idPersona);
        }

        public Usuario ObtenerPorEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            return usuarioDatos.ObtenerPorEmail(email.Trim());
        }

        public Usuario ValidarCredenciales(string nombreUsuario, string password)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            string passwordHash = new SeguridadService().CalcularHash(password);
            Usuario usuario = usuarioDatos.ValidarCredenciales(nombreUsuario.Trim(), passwordHash);
            ValidarEstadoParaIngreso(usuario);
            return usuario;
        }

        public Usuario RegistrarCredencialesPendientes(
            string email,
            string nombreUsuario,
            string nuevaContrasena,
            string confirmacionContrasena)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Debe ingresar un email.");
            }

            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                throw new Exception("Debe ingresar un nombre de usuario.");
            }

            if (nombreUsuario.Trim().Length < 4)
            {
                throw new Exception("El nombre de usuario debe tener al menos 4 caracteres.");
            }

            SeguridadService seguridadService = new SeguridadService();
            seguridadService.ValidarNuevaContrasena(nuevaContrasena, confirmacionContrasena);

            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio(manejador.CrearAccesoDatos());
                    Usuario usuario = usuarioNegocio.ObtenerPorEmail(email.Trim());
                    if (usuario == null)
                    {
                        throw new Exception("No existe un usuario registrado con ese correo.");
                    }

                    if (!EsEstado(usuario, EstadoUsuarioEnum.Pendiente))
                    {
                        throw new Exception("La cuenta no esta pendiente de registro.");
                    }

                    usuario.NombreUsuario = nombreUsuario.Trim();
                    usuario.PasswordHash = seguridadService.CalcularHash(nuevaContrasena.Trim());
                    usuario.EstadoUsuario = new EstadoUsuario
                    {
                        Nombre = EstadoUsuarioEnum.Activo.ToString()
                    };

                    usuarioNegocio.Modificar(usuario);
                    manejador.Confirmar();
                    return usuarioNegocio.ObtenerPorId(usuario.IdUsuario);
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }
        }

        public void Agregar(Usuario usuario)
        {
            PrepararEstadoInicial(usuario);
            ValidarAlta(usuario);
            usuarioDatos.Agregar(usuario);
        }

        public void AgregarConPersona(Usuario usuario)
        {
            ValidarRolAdministrativo(usuario);
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
            ValidarRolAdministrativo(usuario);
            Usuario usuarioActual = ObtenerPorId(usuario.IdUsuario);
            ValidarUsuarioAdministrativoExistente(usuarioActual);

            if (usuario.Persona == null
                || usuarioActual.Persona.IdPersona != usuario.Persona.IdPersona)
            {
                throw new Exception("La persona asociada al usuario no se puede cambiar.");
            }

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

        public void ModificarPerfilConPersona(Usuario usuario)
        {
            if (usuario == null || usuario.IdUsuario <= 0)
            {
                throw new Exception("El id de usuario no es valido.");
            }

            Usuario usuarioActual = ObtenerPorId(usuario.IdUsuario);
            if (usuarioActual == null)
            {
                throw new Exception("El usuario no existe.");
            }

            if (usuario.Persona == null
                || usuarioActual.Persona.IdPersona != usuario.Persona.IdPersona)
            {
                throw new Exception("La persona asociada al usuario no se puede cambiar.");
            }

            usuario.Rol = usuarioActual.Rol;
            usuario.EstadoUsuario = usuarioActual.EstadoUsuario;
            ValidarModificacion(usuario);

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

        public void Desactivar(int id)
        {
            Usuario usuario = ObtenerPorId(id);
            ValidarDesactivacion(usuario);
            usuarioDatos.Desactivar(id);
        }

        public void Activar(int id)
        {
            Usuario usuario = ObtenerPorId(id);
            ValidarActivacion(usuario);
            usuarioDatos.Activar(id);
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

            Usuario usuarioActual = ObtenerPorId(usuario.IdUsuario);
            if (usuarioActual == null)
            {
                throw new Exception("El usuario no existe.");
            }

            if (!EsEstado(usuarioActual, EstadoUsuarioEnum.Inactivo)
                && EsEstado(usuario, EstadoUsuarioEnum.Inactivo))
            {
                ValidarDesactivacion(usuarioActual);
            }

            if (EsEstado(usuarioActual, EstadoUsuarioEnum.Inactivo)
                && !EsEstado(usuario, EstadoUsuarioEnum.Inactivo))
            {
                ValidarActivacion(usuarioActual);
            }
        }

        private void ValidarDesactivacion(Usuario usuario)
        {
            ValidarUsuarioAdministrativoExistente(usuario);

            if (EsEstado(usuario, EstadoUsuarioEnum.Inactivo))
            {
                throw new Exception("El usuario ya esta inactivo.");
            }
        }

        private void ValidarActivacion(Usuario usuario)
        {
            ValidarUsuarioAdministrativoExistente(usuario);

            if (!EsEstado(usuario, EstadoUsuarioEnum.Inactivo))
            {
                throw new Exception("El usuario ya esta activo.");
            }

            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
            {
                throw new Exception("El usuario debe tener un nombre de usuario para poder activarse.");
            }

            if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
            {
                throw new Exception("El usuario debe tener una contrasena para poder activarse.");
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

            if (usuario.Rol == null
                || string.IsNullOrWhiteSpace(usuario.Rol.Nombre)
                || !Enum.TryParse(usuario.Rol.Nombre, true, out RolEnum _))
            {
                throw new Exception("Debe asignar un rol valido al usuario.");
            }

            if (usuario.EstadoUsuario == null
                || string.IsNullOrWhiteSpace(usuario.EstadoUsuario.Nombre)
                || !Enum.TryParse(usuario.EstadoUsuario.Nombre, true, out EstadoUsuarioEnum _))
            {
                throw new Exception("Debe asignar un estado valido al usuario.");
            }

            usuario.NombreUsuario = string.IsNullOrWhiteSpace(usuario.NombreUsuario)
                ? null
                : usuario.NombreUsuario.Trim();
            usuario.PasswordHash = string.IsNullOrWhiteSpace(usuario.PasswordHash)
                ? null
                : usuario.PasswordHash.Trim();

            if (!string.IsNullOrWhiteSpace(usuario.NombreUsuario)
                && usuarioDatos.ExisteNombreUsuario(usuario.NombreUsuario, usuario.IdUsuario))
            {
                throw new Exception("Ya existe un nombre de usuario registrado con ese valor.");
            }
        }

        private void ValidarRolAdministrativo(Usuario usuario)
        {
            if (usuario == null || usuario.Rol == null)
            {
                throw new Exception("Debe asignar un rol valido al usuario.");
            }

            bool esAdministrador = string.Equals(
                usuario.Rol.Nombre,
                RolEnum.Administrador.ToString(),
                StringComparison.OrdinalIgnoreCase);
            bool esRecepcionista = string.Equals(
                usuario.Rol.Nombre,
                RolEnum.Recepcionista.ToString(),
                StringComparison.OrdinalIgnoreCase);

            if (!esAdministrador && !esRecepcionista)
            {
                throw new Exception("Solo se pueden administrar usuarios Administrador o Recepcionista.");
            }
        }

        private void ValidarUsuarioAdministrativoExistente(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new Exception("El usuario no existe.");
            }

            ValidarRolAdministrativo(usuario);
        }

        private void ValidarEstadoParaIngreso(Usuario usuario)
        {
            if (usuario == null)
            {
                return;
            }

            if (EsEstado(usuario, EstadoUsuarioEnum.Inactivo))
            {
                throw new Exception("La cuenta se encuentra inactiva.");
            }

            if (EsEstado(usuario, EstadoUsuarioEnum.Bloqueado))
            {
                throw new Exception("La cuenta se encuentra bloqueada.");
            }
        }

        private void PrepararEstadoInicial(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new Exception("El usuario es obligatorio.");
            }

            usuario.NombreUsuario = null;
            usuario.PasswordHash = null;
            usuario.EstadoUsuario = new EstadoUsuario
            {
                Nombre = EstadoUsuarioEnum.Pendiente.ToString()
            };
        }

        private bool EsEstado(Usuario usuario, EstadoUsuarioEnum estado)
        {
            return usuario.EstadoUsuario != null
                && string.Equals(
                    usuario.EstadoUsuario.Nombre,
                    estado.ToString(),
                    StringComparison.OrdinalIgnoreCase);
        }
    }
}
