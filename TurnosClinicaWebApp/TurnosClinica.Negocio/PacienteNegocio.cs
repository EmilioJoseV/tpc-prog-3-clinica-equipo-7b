using System;
using System.Collections.Generic;
using AccesoDatosBase = TurnosClinica.AccesoDatos.AccesoDatos;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Negocio
{
    public class PacienteNegocio
    {
        private readonly PacienteDatos pacienteDatos;
        private readonly PersonaNegocio personaNegocio;

        public PacienteNegocio()
        {
            pacienteDatos = new PacienteDatos();
            personaNegocio = new PersonaNegocio();
        }

        public List<Paciente> Listar(bool? activo = null)
        {
            try
            {
                return pacienteDatos.Listar(activo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Paciente> ListarFiltroRapido(string filtro)
        {
            try
            {
                return pacienteDatos.ListarFiltroRapido(filtro);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Paciente> ListarConFiltros(string campo, string criterio, string filtro, bool? activo = null)
        {
            try
            {
                return pacienteDatos.ListarConFiltros(campo, criterio, filtro, activo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Paciente ObtenerPorId(int idPaciente)
        {
            try
            {
                return pacienteDatos.ObtenerPorId(idPaciente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Agregar(Paciente paciente)
        {
            ValidarPaciente(paciente, true);

            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();

                    PacienteDatos pacienteDatosTransaccional = new PacienteDatos(transaccionDatos.AccesoDatos);
                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio(transaccionDatos.AccesoDatos);

                    pacienteDatosTransaccional.Agregar(paciente);
                    SincronizarUsuarioAsociado(paciente, usuarioNegocio);

                    transaccionDatos.Confirmar();
                }
                catch (Exception ex)
                {
                    transaccionDatos.Cancelar();
                    throw ex;
                }
            }
        }

        public void Modificar(Paciente paciente)
        {
            ValidarPaciente(paciente, false);

            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();

                    PacienteDatos pacienteDatosTransaccional = new PacienteDatos(transaccionDatos.AccesoDatos);
                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio(transaccionDatos.AccesoDatos);

                    pacienteDatosTransaccional.Modificar(paciente);
                    SincronizarUsuarioAsociado(paciente, usuarioNegocio);

                    transaccionDatos.Confirmar();
                }
                catch (Exception ex)
                {
                    transaccionDatos.Cancelar();
                    throw ex;
                }
            }
        }

        public bool Desactivar(int idPaciente)
        {
            try
            {
                Paciente paciente = pacienteDatos.ObtenerPorId(idPaciente);
                if (paciente == null)
                {
                    return false;
                }

                using (TransaccionDatos transaccionDatos = new TransaccionDatos())
                {
                    try
                    {
                        transaccionDatos.IniciarTransaccion();

                        PacienteDatos pacienteDatosTransaccional = new PacienteDatos(transaccionDatos.AccesoDatos);
                        UsuarioDatos usuarioDatos = new UsuarioDatos(transaccionDatos.AccesoDatos);

                        bool resultado = pacienteDatosTransaccional.Desactivar(idPaciente);
                        if (resultado)
                        {
                            Usuario usuario = usuarioDatos.ObtenerPorIdPersona(paciente.IdPersona);
                            if (usuario != null)
                            {
                                usuario.EstadoUsuario = new EstadoUsuario
                                {
                                    Nombre = EstadoUsuarioEnum.Inactivo.ToString()
                                };
                                usuarioDatos.Modificar(usuario);
                            }
                        }

                        transaccionDatos.Confirmar();
                        return resultado;
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

        private void ValidarPaciente(Paciente paciente, bool esAlta)
        {
            if (paciente == null)
            {
                throw new Exception("El paciente es obligatorio.");
            }

            if (paciente.FechaNacimiento == default(DateTime))
            {
                throw new Exception("La fecha de nacimiento es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(paciente.Direccion))
            {
                throw new Exception("La dirección es obligatoria.");
            }

            personaNegocio.ValidarPersona(paciente, esAlta);
        }

        private void SincronizarUsuarioAsociado(Paciente paciente, UsuarioNegocio usuarioNegocio)
        {
            Usuario usuarioExistente = usuarioNegocio.ObtenerPorIdPersona(paciente.IdPersona);
            Usuario usuario = new Usuario
            {
                IdPersona = paciente.IdPersona,
                DNI = paciente.DNI,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                Telefono = paciente.Telefono,
                Email = paciente.Email,
                NombreUsuario = usuarioExistente != null ? usuarioExistente.NombreUsuario : null,
                PasswordHash = usuarioExistente != null ? usuarioExistente.PasswordHash : null,
                Imagen = usuarioExistente != null ? usuarioExistente.Imagen : null,
                Rol = new Rol
                {
                    Nombre = RolEnum.Paciente.ToString()
                }
            };

            if (usuarioExistente != null)
            {
                usuario.IdUsuario = usuarioExistente.IdUsuario;
                usuarioNegocio.Modificar(usuario);
            }
            else
            {
                usuarioNegocio.Agregar(usuario);
            }
        }
    }
}
