using System;
using System.Collections.Generic;
using System.Linq;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;
using TurnosClinica.Negocio.DTO;

namespace TurnosClinica.Negocio
{
    public class TurnoNegocio
    {
        private readonly TurnoDatos turnoDatos;

        public TurnoNegocio()
        {
            turnoDatos = new TurnoDatos(new AccesoDatosBase());
        }

        public Turno ObtenerPorId(int idTurno)
        {
            if (idTurno <= 0)
            {
                throw new Exception("El id del turno no es valido.");
            }

            return turnoDatos.ObtenerPorId(idTurno);
        }

        public List<Turno> ListarPorMedicoYFecha(int idMedico, DateTime fecha)
        {
            if (idMedico <= 0)
            {
                throw new Exception("El id del medico no es valido.");
            }

            return turnoDatos.ListarPorMedicoYFecha(idMedico, fecha);
        }

        public List<Turno> ListarPorPacienteYFecha(int idPaciente, DateTime fecha)
        {
            if (idPaciente <= 0)
            {
                throw new Exception("El id del paciente no es valido.");
            }

            return turnoDatos.ListarPorPacienteYFecha(idPaciente, fecha);
        }

        public void Agregar(Turno turno)
        {
            ValidarAlta(turno);
            PrepararTurno(turno);

            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    EstadoTurnoNegocio estadoTurnoNegocio = new EstadoTurnoNegocio(manejador.CrearAccesoDatos());
                    TurnoDatos datos = new TurnoDatos(manejador.CrearAccesoDatos());
                    turno.EstadoTurno = estadoTurnoNegocio.ObtenerPorNombre(EstadoTurnoEnum.Nuevo.ToString());
                    turno.IdTurno = datos.Agregar(turno);
                    manejador.Confirmar();
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }

            EnviarConfirmacion(turno);
        }

        private void ValidarAlta(Turno turno)
        {
            if (turno == null)
            {
                throw new Exception("El turno es obligatorio.");
            }

            if (turno.Paciente == null || turno.Paciente.IdPaciente <= 0)
            {
                throw new Exception("Debe seleccionar un paciente.");
            }

            if (turno.Medico == null || turno.Medico.IdMedico <= 0)
            {
                throw new Exception("Debe seleccionar un medico.");
            }

            if (turno.Especialidad == null || turno.Especialidad.IdEspecialidad <= 0)
            {
                throw new Exception("Debe seleccionar una especialidad.");
            }

            if (turno.UsuarioAlta == null || turno.UsuarioAlta.IdUsuario <= 0)
            {
                throw new Exception("No se pudo identificar al usuario que registra el turno.");
            }

            if (string.IsNullOrWhiteSpace(turno.Observaciones))
            {
                throw new Exception("Las observaciones son obligatorias.");
            }

            if (turno.Observaciones.Trim().Length > 500)
            {
                throw new Exception("Las observaciones no pueden superar los 500 caracteres.");
            }

            Paciente paciente = new PacienteNegocio().ObtenerPorId(turno.Paciente.IdPaciente);
            if (paciente == null || !paciente.Activo)
            {
                throw new Exception("El paciente seleccionado no esta disponible.");
            }

            if (paciente.Persona == null || string.IsNullOrWhiteSpace(paciente.Persona.Email))
            {
                throw new Exception("El paciente debe tener un email para recibir la confirmacion.");
            }

            Especialidad especialidad = new EspecialidadNegocio().ObtenerPorId(turno.Especialidad.IdEspecialidad);
            if (especialidad == null || !especialidad.Activo)
            {
                throw new Exception("La especialidad seleccionada no esta disponible.");
            }

            Medico medico = new MedicoNegocio().ObtenerPorId(turno.Medico.IdMedico);
            if (medico == null || !medico.Activo)
            {
                throw new Exception("El medico seleccionado no esta disponible.");
            }

            bool atiendeEspecialidad = medico.Especialidades != null
                && medico.Especialidades.Any(item => item.IdEspecialidad == especialidad.IdEspecialidad);
            if (!atiendeEspecialidad)
            {
                throw new Exception("El medico no atiende la especialidad seleccionada.");
            }

            Usuario usuario = new UsuarioNegocio().ObtenerPorId(turno.UsuarioAlta.IdUsuario);
            if (usuario == null)
            {
                throw new Exception("El usuario que registra el turno no existe.");
            }

            ValidarHorarioDisponible(turno);

            if (turnoDatos.ExisteSuperposicionPaciente(
                paciente.IdPaciente,
                turno.FechaTurno,
                turno.HoraInicio,
                turno.HoraFin))
            {
                throw new Exception("El paciente ya tiene un turno en ese horario.");
            }

            turno.Paciente = paciente;
            turno.Medico = medico;
            turno.Especialidad = especialidad;
            turno.UsuarioAlta = usuario;
        }

        private void ValidarHorarioDisponible(Turno turno)
        {
            List<TurnoDisponibleDTO> disponibles = new TurnoCalculoService().ListarTurnosDisponibles(
                turno.Especialidad.IdEspecialidad,
                turno.FechaTurno);

            bool horarioDisponible = disponibles.Any(disponible =>
                disponible.Medico != null
                && disponible.Medico.IdMedico == turno.Medico.IdMedico
                && disponible.FechaTurno.Date == turno.FechaTurno.Date
                && disponible.HoraInicio == turno.HoraInicio
                && disponible.HoraFin == turno.HoraFin);

            if (!horarioDisponible)
            {
                throw new Exception("El horario seleccionado ya no esta disponible.");
            }
        }

        private void PrepararTurno(Turno turno)
        {
            turno.FechaTurno = turno.FechaTurno.Date;
            turno.Observaciones = turno.Observaciones.Trim();
            turno.DiagnosticoMedico = null;
        }

        private void EnviarConfirmacion(Turno turno)
        {
            try
            {
                new MailNegocio().EnviarConfirmacionTurnoNuevo(turno);
            }
            catch
            {
                //Por ahora no hacemos nada....
            }
        }
    }
}
