using System;
using System.Security.Cryptography;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Negocio
{
    public class AccesoService
    {
        private const int LongitudClaveTemporal = 12;
        private readonly SeguridadService seguridadService = new SeguridadService();

        public bool IniciarRecuperacionContrasenaPorEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Debe ingresar un email.");
            }

            Usuario usuario = new UsuarioNegocio().ObtenerPorEmail(email.Trim());
            if (usuario == null)
            {
                return false;
            }

            if (EsEstado(usuario, EstadoUsuarioEnum.Inactivo))
            {
                throw new Exception("La cuenta se encuentra inactiva y no admite recuperacion automatica.");
            }

            if (EsEstado(usuario, EstadoUsuarioEnum.Bloqueado))
            {
                throw new Exception("La cuenta se encuentra bloqueada.");
            }

            string claveTemporal = GenerarClaveTemporal();

            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio(manejador.CrearAccesoDatos());
                    Usuario usuarioActual = usuarioNegocio.ObtenerPorId(usuario.IdUsuario);
                    if (usuarioActual == null)
                    {
                        throw new Exception("El usuario no existe.");
                    }

                    if (EsEstado(usuarioActual, EstadoUsuarioEnum.Inactivo))
                    {
                        throw new Exception("La cuenta se encuentra inactiva y no admite recuperacion automatica.");
                    }

                    if (EsEstado(usuarioActual, EstadoUsuarioEnum.Bloqueado))
                    {
                        throw new Exception("La cuenta se encuentra bloqueada.");
                    }

                    usuarioActual.PasswordHash = seguridadService.CalcularHash(claveTemporal);
                    usuarioActual.EstadoUsuario = new EstadoUsuario
                    {
                        Nombre = EstadoUsuarioEnum.CambioClavePendiente.ToString()
                    };

                    usuarioNegocio.Modificar(usuarioActual);
                    new MailService().EnviarRecuperacionContrasenaConClaveTemporal(usuarioActual, claveTemporal);
                    manejador.Confirmar();
                    return true;
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }
        }

        public Usuario CambiarContrasenaPendiente(int idUsuario, string nuevaContrasena, string confirmacionContrasena)
        {
            if (idUsuario <= 0)
            {
                throw new Exception("El usuario autenticado no es valido.");
            }

            if (string.IsNullOrWhiteSpace(nuevaContrasena))
            {
                throw new Exception("Debe ingresar la nueva contrasena.");
            }

            if (nuevaContrasena.Trim().Length < 8)
            {
                throw new Exception("La nueva contrasena debe tener al menos 8 caracteres.");
            }

            if (!string.Equals(nuevaContrasena, confirmacionContrasena, StringComparison.Ordinal))
            {
                throw new Exception("La confirmacion de contrasena no coincide.");
            }

            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio(manejador.CrearAccesoDatos());
                    Usuario usuario = usuarioNegocio.ObtenerPorId(idUsuario);
                    if (usuario == null)
                    {
                        throw new Exception("El usuario no existe.");
                    }

                    if (!EsEstado(usuario, EstadoUsuarioEnum.CambioClavePendiente))
                    {
                        throw new Exception("La cuenta no requiere cambio de contrasena.");
                    }

                    usuario.PasswordHash = seguridadService.CalcularHash(nuevaContrasena.Trim());
                    usuario.EstadoUsuario = new EstadoUsuario
                    {
                        Nombre = EstadoUsuarioEnum.Activo.ToString()
                    };

                    usuarioNegocio.Modificar(usuario);
                    manejador.Confirmar();
                    return usuarioNegocio.ObtenerPorId(idUsuario);
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }
        }

        private string GenerarClaveTemporal()
        {
            //Caracteres permitidos para la clave temporal.
            const string caracteres = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@#$%^&*()_+-={}[]";
            string clave = "";
            Random random = new Random();

            for (int i = 0; i < LongitudClaveTemporal; i++)
            {
                int posicion = random.Next(caracteres.Length);
                clave += caracteres[posicion];
            }

            return clave;
        }

        private bool EsEstado(Usuario usuario, EstadoUsuarioEnum estado)
        {
            return usuario != null
                && usuario.EstadoUsuario != null
                && string.Equals(
                    usuario.EstadoUsuario.Nombre,
                    estado.ToString(),
                    StringComparison.OrdinalIgnoreCase);
        }
    }
}
