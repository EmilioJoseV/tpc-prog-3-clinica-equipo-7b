using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Negocio
{
    public class PacienteNegocio
    {
        private readonly PacienteDatos pacienteDatos;

        public PacienteNegocio()
        {
            pacienteDatos = new PacienteDatos();
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

        public Paciente ObtenerPorIdPersona(int idPersona)
        {
            try
            {
                if (idPersona <= 0)
                    return null;

                return pacienteDatos.ObtenerPorIdPersona(idPersona);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Agregar(Paciente paciente)
        {
            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();

                    PacienteDatos pacienteDatosTransaccional = new PacienteDatos(transaccionDatos.AccesoDatos);
                    UsuarioDatos usuarioDatos = new UsuarioDatos(transaccionDatos.AccesoDatos);
                    pacienteDatosTransaccional.Agregar(paciente);
                    SincronizarUsuarioAsociado(paciente, usuarioDatos);

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
            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();

                    PacienteDatos pacienteDatosTransaccional = new PacienteDatos(transaccionDatos.AccesoDatos);
                    UsuarioDatos usuarioDatos = new UsuarioDatos(transaccionDatos.AccesoDatos);
                    pacienteDatosTransaccional.Modificar(paciente);
                    SincronizarUsuarioAsociado(paciente, usuarioDatos);

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
                bool resultado = pacienteDatos.Desactivar(idPaciente);
                if (resultado && paciente != null)
                {
                    UsuarioDatos usuarioDatos = new UsuarioDatos();
                    Usuario usuario = usuarioDatos.ObtenerPorIdPersona(paciente.IdPersona);
                    if (usuario != null)
                    {
                        usuario.EstadoUsuario = EstadoUsuarioEnum.Inactivo;
                        usuarioDatos.Modificar(usuario);
                    }
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SincronizarUsuarioAsociado(Paciente paciente, UsuarioDatos usuarioDatos)
        {
            Usuario usuarioExistente = usuarioDatos.ObtenerPorIdPersona(paciente.IdPersona);
            Usuario usuario = new Usuario
            {
                IdPersona = paciente.IdPersona,
                DNI = paciente.DNI,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                Telefono = paciente.Telefono,
                Email = paciente.Email,
                NombreUsuario = usuarioExistente != null && !string.IsNullOrWhiteSpace(usuarioExistente.NombreUsuario)
                    ? usuarioExistente.NombreUsuario
                    : paciente.DNI,
                PasswordHash = usuarioExistente != null && !string.IsNullOrWhiteSpace(usuarioExistente.PasswordHash)
                    ? usuarioExistente.PasswordHash
                    : Guid.NewGuid().ToString("N"),
                Imagen = usuarioExistente != null ? usuarioExistente.Imagen : null,
                EstadoUsuario = !paciente.Activo
                    ? EstadoUsuarioEnum.Inactivo
                    : (usuarioExistente != null
                        ? usuarioExistente.EstadoUsuario
                        : EstadoUsuarioEnum.Pendiente),
                Rol = new Rol
                {
                    Nombre = RolEnum.Paciente.ToString()
                }
            };

            if (usuarioExistente != null)
            {
                usuario.IdUsuario = usuarioExistente.IdUsuario;
                usuarioDatos.Modificar(usuario);
            }
            else
            {
                usuarioDatos.Agregar(usuario);
            }
        }
    }
}
