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
        private readonly PersonaDatos personaDatos;

        public UsuarioNegocio()
        {
            usuarioDatos = new UsuarioDatos();
            personaDatos = new PersonaDatos();
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
            ValidarUsuario(usuario, true);

            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
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
            ValidarUsuario(usuario, false);

            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
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
                {
                    throw new ArgumentException("ID de usuario no válido para eliminación.");
                }

                using (TransaccionDatos transaccionDatos = new TransaccionDatos())
                {
                    try
                    {
                        transaccionDatos.IniciarTransaccion();
                        UsuarioDatos datos = new UsuarioDatos(transaccionDatos.AccesoDatos);
                        datos.EliminarLogico(idUsuario);
                        transaccionDatos.Confirmar();
                    }
                    catch (Exception ex)
                    {
                        transaccionDatos.Cancelar();
                        throw ex;
                    }
                }
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
                if (idUsuario <= 0)
                {
                    throw new ArgumentException("ID de usuario no válido para alta lógica.");
                }

                using (TransaccionDatos transaccionDatos = new TransaccionDatos())
                {
                    try
                    {
                        transaccionDatos.IniciarTransaccion();
                        UsuarioDatos datos = new UsuarioDatos(transaccionDatos.AccesoDatos);
                        datos.AltaLogica(idUsuario);
                        transaccionDatos.Confirmar();
                    }
                    catch (Exception ex)
                    {
                        transaccionDatos.Cancelar();
                        throw ex;
                    }
                }
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
                {
                    throw new ArgumentException("ID de usuario no válido para eliminación.");
                }

                using (TransaccionDatos transaccionDatos = new TransaccionDatos())
                {
                    try
                    {
                        transaccionDatos.IniciarTransaccion();
                        UsuarioDatos datos = new UsuarioDatos(transaccionDatos.AccesoDatos);
                        datos.EliminarFisico(idUsuario);
                        transaccionDatos.Confirmar();
                    }
                    catch (Exception ex)
                    {
                        transaccionDatos.Cancelar();
                        throw ex;
                    }
                }
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

            if (string.IsNullOrWhiteSpace(usuario.DNI))
            {
                throw new Exception("El DNI es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(usuario.Nombre))
            {
                throw new Exception("El nombre es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(usuario.Apellido))
            {
                throw new Exception("El apellido es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(usuario.Email))
            {
                throw new Exception("El correo electrónico es obligatorio.");
            }

            if (usuario.Rol == null || string.IsNullOrWhiteSpace(usuario.Rol.Nombre) || !Enum.TryParse(usuario.Rol.Nombre, true, out RolEnum _))
            {
                throw new Exception("Debe asignar un rol válido al usuario.");
            }

            Persona personaPorDni = personaDatos.ObtenerPorDni(usuario.DNI.Trim());
            if (personaPorDni != null && personaPorDni.IdPersona != usuario.IdPersona)
            {
                throw new Exception("Ya existe una persona registrada con ese DNI.");
            }

            Persona personaPorEmail = personaDatos.ObtenerPorEmail(usuario.Email.Trim());
            if (personaPorEmail != null && personaPorEmail.IdPersona != usuario.IdPersona)
            {
                throw new Exception("Ya existe una persona registrada con ese correo electrónico.");
            }

            Usuario usuarioActual = null;
            if (usuario.IdUsuario > 0)
            {
                usuarioActual = usuarioDatos.ObtenerPorId(usuario.IdUsuario);
            }

            usuario.NombreUsuario = string.IsNullOrWhiteSpace(usuario.NombreUsuario) ? null : usuario.NombreUsuario.Trim();
            usuario.PasswordHash = string.IsNullOrWhiteSpace(usuario.PasswordHash) ? null : usuario.PasswordHash.Trim();

            if (usuario.NombreUsuario == null && usuario.PasswordHash == null)
            {
                usuario.EstadoUsuario = EstadoUsuarioEnum.Pendiente;
            }
            else if (!Enum.IsDefined(typeof(EstadoUsuarioEnum), usuario.EstadoUsuario))
            {
                usuario.EstadoUsuario = EstadoUsuarioEnum.Pendiente;
            }

            if (!string.IsNullOrWhiteSpace(usuario.NombreUsuario))
            {
                bool nombreUsuarioCambio = usuarioActual == null || !string.Equals(usuarioActual.NombreUsuario, usuario.NombreUsuario, StringComparison.OrdinalIgnoreCase);
                if ((esAlta || nombreUsuarioCambio) && usuarioDatos.ExisteNombreUsuario(usuario.NombreUsuario))
                {
                    throw new Exception("Ya existe un nombre de usuario registrado con ese valor.");
                }
            }
        }
    }
}
