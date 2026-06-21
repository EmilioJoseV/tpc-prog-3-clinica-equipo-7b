using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Negocio
{
    public class MedicoNegocio
    {
        private readonly MedicoDatos medicoDatos;

        public MedicoNegocio()
        {
            medicoDatos = new MedicoDatos();
        }

        public List<Medico> Listar(bool? activo = null)
        {
            try
            {
                return medicoDatos.Listar(activo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Medico> ListarFiltroRapido(string filtro)
        {
            try
            {
                return medicoDatos.ListarFiltroRapido(filtro);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Medico> ListarConFiltros(string campo, string criterio, string filtro, bool? activo = null)
        {
            try
            {
                return medicoDatos.ListarConFiltros(campo, criterio, filtro, activo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Medico ObtenerPorId(int idMedico)
        {
            try
            {
                return medicoDatos.ObtenerPorId(idMedico);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Medico ObtenerPorIdPersona(int idPersona)
        {
            try
            {
                if (idPersona <= 0)
                    return null;

                return medicoDatos.ObtenerPorIdPersona(idPersona);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Agregar(Medico medico)
        {
            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();

                    MedicoDatos medicoDatos = new MedicoDatos(transaccionDatos.AccesoDatos);
                    MedicoEspecialidadesDatos medicoEspecialidadesDatos = new MedicoEspecialidadesDatos(transaccionDatos.AccesoDatos);
                    HorarioDisponibilidadMedicoDatos horarioDisponibilidadMedicoDatos = new HorarioDisponibilidadMedicoDatos(transaccionDatos.AccesoDatos);
                    UsuarioDatos usuarioDatos = new UsuarioDatos(transaccionDatos.AccesoDatos);

                    medicoDatos.Agregar(medico);
                    SincronizarUsuarioAsociado(medico, usuarioDatos);
                    medicoEspecialidadesDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.Especialidades);
                    horarioDisponibilidadMedicoDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.HorariosDisponibilidad);

                    transaccionDatos.Confirmar();
                    return;
                }
                catch (Exception ex)
                {
                    transaccionDatos.Cancelar();
                    throw ex;
                }
            }
        }

        public void Modificar(Medico medico)
        {
            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();

                    MedicoDatos medicoDatosTransaccional = new MedicoDatos(transaccionDatos.AccesoDatos);
                    MedicoEspecialidadesDatos medicoEspecialidadesDatos = new MedicoEspecialidadesDatos(transaccionDatos.AccesoDatos);
                    HorarioDisponibilidadMedicoDatos horarioDisponibilidadMedicoDatos = new HorarioDisponibilidadMedicoDatos(transaccionDatos.AccesoDatos);
                    UsuarioDatos usuarioDatos = new UsuarioDatos(transaccionDatos.AccesoDatos);

                    medicoDatosTransaccional.Modificar(medico);
                    SincronizarUsuarioAsociado(medico, usuarioDatos);
                    medicoEspecialidadesDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.Especialidades);
                    horarioDisponibilidadMedicoDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.HorariosDisponibilidad);

                    transaccionDatos.Confirmar();
                    return;
                }
                catch (Exception ex)
                {
                    transaccionDatos.Cancelar();
                    throw ex;
                }
            }
        }

        public bool Desactivar(int idMedico)
        {
            try
            {
                Medico medico = medicoDatos.ObtenerPorId(idMedico);
                bool resultado = medicoDatos.Desactivar(idMedico);
                if (resultado && medico != null)
                {
                    UsuarioDatos usuarioDatos = new UsuarioDatos();
                    Usuario usuario = usuarioDatos.ObtenerPorIdPersona(medico.IdPersona);
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

        private void SincronizarUsuarioAsociado(Medico medico, UsuarioDatos usuarioDatos)
        {
            Usuario usuarioExistente = usuarioDatos.ObtenerPorIdPersona(medico.IdPersona);
            Usuario usuario = new Usuario
            {
                IdPersona = medico.IdPersona,
                DNI = medico.DNI,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Telefono = medico.Telefono,
                Email = medico.Email,
                NombreUsuario = usuarioExistente != null && !string.IsNullOrWhiteSpace(usuarioExistente.NombreUsuario)
                    ? usuarioExistente.NombreUsuario
                    : medico.DNI,
                PasswordHash = usuarioExistente != null && !string.IsNullOrWhiteSpace(usuarioExistente.PasswordHash)
                    ? usuarioExistente.PasswordHash
                    : "123456",
                Imagen = usuarioExistente != null ? usuarioExistente.Imagen : null,
                EstadoUsuario = !medico.Activo
                    ? EstadoUsuarioEnum.Inactivo
                    : (usuarioExistente != null
                        ? usuarioExistente.EstadoUsuario
                        : EstadoUsuarioEnum.Pendiente),
                Rol = new Rol
                {
                    Nombre = RolEnum.Medico.ToString()
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
