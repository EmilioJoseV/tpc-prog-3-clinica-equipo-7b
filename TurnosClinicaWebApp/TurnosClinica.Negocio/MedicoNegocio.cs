using System;
using System.Collections.Generic;
using AccesoDatosBase = TurnosClinica.AccesoDatos.AccesoDatos;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Negocio
{
    public class MedicoNegocio
    {
        private readonly MedicoDatos medicoDatos;
        private readonly PersonaNegocio personaNegocio;

        public MedicoNegocio()
        {
            medicoDatos = new MedicoDatos();
            personaNegocio = new PersonaNegocio();
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

        public void Agregar(Medico medico)
        {
            ValidarMedico(medico, true);

            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();

                    MedicoDatos medicoDatosTransaccional = new MedicoDatos(transaccionDatos.AccesoDatos);
                    MedicoEspecialidadesDatos medicoEspecialidadesDatos = new MedicoEspecialidadesDatos(transaccionDatos.AccesoDatos);
                    HorarioDisponibilidadMedicoDatos horarioDisponibilidadMedicoDatos = new HorarioDisponibilidadMedicoDatos(transaccionDatos.AccesoDatos);
                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio(transaccionDatos.AccesoDatos);

                    medicoDatosTransaccional.Agregar(medico);
                    SincronizarUsuarioAsociado(medico, usuarioNegocio);
                    medicoEspecialidadesDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.Especialidades);
                    horarioDisponibilidadMedicoDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.HorariosDisponibilidad);

                    transaccionDatos.Confirmar();
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
            ValidarMedico(medico, false);

            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();

                    MedicoDatos medicoDatosTransaccional = new MedicoDatos(transaccionDatos.AccesoDatos);
                    MedicoEspecialidadesDatos medicoEspecialidadesDatos = new MedicoEspecialidadesDatos(transaccionDatos.AccesoDatos);
                    HorarioDisponibilidadMedicoDatos horarioDisponibilidadMedicoDatos = new HorarioDisponibilidadMedicoDatos(transaccionDatos.AccesoDatos);
                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio(transaccionDatos.AccesoDatos);

                    medicoDatosTransaccional.Modificar(medico);
                    SincronizarUsuarioAsociado(medico, usuarioNegocio);
                    medicoEspecialidadesDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.Especialidades);
                    horarioDisponibilidadMedicoDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.HorariosDisponibilidad);

                    transaccionDatos.Confirmar();
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
                if (medico == null)
                {
                    return false;
                }

                using (TransaccionDatos transaccionDatos = new TransaccionDatos())
                {
                    try
                    {
                        transaccionDatos.IniciarTransaccion();

                        MedicoDatos medicoDatosTransaccional = new MedicoDatos(transaccionDatos.AccesoDatos);
                        UsuarioDatos usuarioDatos = new UsuarioDatos(transaccionDatos.AccesoDatos);

                        bool resultado = medicoDatosTransaccional.Desactivar(idMedico);
                        if (resultado)
                        {
                            Usuario usuario = usuarioDatos.ObtenerPorIdPersona(medico.IdPersona);
                            if (usuario != null)
                            {
                                usuario.EstadoUsuario = EstadoUsuarioEnum.Inactivo;
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

        private void ValidarMedico(Medico medico, bool esAlta)
        {
            if (medico == null)
            {
                throw new Exception("El medico es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(medico.Matricula))
            {
                throw new Exception("La matricula es obligatoria");
            }

            personaNegocio.ValidarPersona(medico, esAlta);

            if (esAlta && medicoDatos.ExisteMatricula(medico.Matricula.Trim()))
            {
                throw new Exception("Ya existe un medico registrado con esa matricula");
            }

            if (!esAlta && medico.IdMedico > 0)
            {
                Medico medicoActual = medicoDatos.ObtenerPorId(medico.IdMedico);
                if (medicoActual != null && !string.Equals(medicoActual.Matricula, medico.Matricula, StringComparison.OrdinalIgnoreCase) && medicoDatos.ExisteMatricula(medico.Matricula.Trim()))
                {
                    throw new Exception("Ya existe un medico registrado con esa matricula");
                }
            }
        }

        private void SincronizarUsuarioAsociado(Medico medico, UsuarioNegocio usuarioNegocio)
        {
            Usuario usuarioExistente = usuarioNegocio.ObtenerPorIdPersona(medico.IdPersona);
            Usuario usuario = new Usuario
            {
                IdPersona = medico.IdPersona,
                DNI = medico.DNI,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Telefono = medico.Telefono,
                Email = medico.Email,
                NombreUsuario = usuarioExistente != null ? usuarioExistente.NombreUsuario : null,
                PasswordHash = usuarioExistente != null ? usuarioExistente.PasswordHash : null,
                Imagen = usuarioExistente != null ? usuarioExistente.Imagen : null,
                Rol = new Rol
                {
                    Nombre = RolEnum.Medico.ToString()
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
