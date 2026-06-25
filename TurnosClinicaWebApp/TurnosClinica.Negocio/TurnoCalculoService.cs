using System;
using System.Collections.Generic;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;
using TurnosClinica.Negocio.DTO;

namespace TurnosClinica.Negocio
{
    public class TurnoCalculoService
    {
        private const int DiasMaximosAnticipacion = 90;

        private readonly ConfiguracionTurnoNegocio configuracionTurnoNegocio;
        private readonly EspecialidadNegocio especialidadNegocio;
        private readonly HorarioDisponibilidadMedicoNegocio horarioDisponibilidadMedicoNegocio;
        private readonly MedicoNegocio medicoNegocio;
        private readonly TurnoNegocio turnoNegocio;

        public TurnoCalculoService()
        {
            configuracionTurnoNegocio = new ConfiguracionTurnoNegocio();
            especialidadNegocio = new EspecialidadNegocio();
            horarioDisponibilidadMedicoNegocio = new HorarioDisponibilidadMedicoNegocio();
            medicoNegocio = new MedicoNegocio();
            turnoNegocio = new TurnoNegocio();
        }

        public List<TurnoDisponibleDTO> ListarTurnosDisponibles(int idEspecialidad, DateTime fechaConsulta)
        {
            ValidarConsulta(idEspecialidad, fechaConsulta);

            Especialidad especialidad = especialidadNegocio.ObtenerPorId(idEspecialidad);
            if (especialidad == null || !especialidad.Activo)
            {
                throw new Exception("La especialidad no esta disponible.");
            }

            List<Medico> medicos = medicoNegocio.ListarPorEspecialidad(idEspecialidad);
            if (medicos.Count == 0)
            {
                return new List<TurnoDisponibleDTO>();
            }

            int duracionBloque = ObtenerDuracionBloque();
            DiaSemanaEnum diaSemana = ObtenerDiaSemana(fechaConsulta.Date);
            List<TurnoDisponibleDTO> turnosDisponibles = new List<TurnoDisponibleDTO>();

            foreach (Medico medico in medicos)
            {
                if (medico == null || !medico.Activo)
                {
                    continue;
                }

                List<HorarioDisponibilidadMedico> horariosDelDia = ObtenerHorariosDelDia(medico.IdMedico, diaSemana);

                if (horariosDelDia.Count == 0)
                {
                    continue;
                }

                List<Turno> turnosOcupados = ObtenerTurnosOcupados(medico.IdMedico, fechaConsulta.Date);
                foreach (HorarioDisponibilidadMedico horario in horariosDelDia)
                {
                    turnosDisponibles.AddRange(ConstruirBloquesDisponibles(
                        medico,
                        fechaConsulta.Date,
                        horario,
                        duracionBloque,
                        turnosOcupados));
                }
            }

            return QuitarDuplicados(turnosDisponibles);
        }

        public List<TurnoDisponibleDTO> ListarTurnosDisponiblesMasProximos(
            int idEspecialidad,
            DateTime fechaDesde)
        {
            ValidarConsulta(idEspecialidad, fechaDesde);

            DateTime fechaLimite = DateTime.Today.AddDays(DiasMaximosAnticipacion);
            DateTime fechaConsulta = fechaDesde.Date;

            while (fechaConsulta <= fechaLimite)
            {
                List<TurnoDisponibleDTO> disponibles =
                    ListarTurnosDisponibles(idEspecialidad, fechaConsulta);

                if (disponibles.Count > 0)
                {
                    return disponibles;
                }

                fechaConsulta = fechaConsulta.AddDays(1);
            }

            return new List<TurnoDisponibleDTO>();
        }

        private void ValidarConsulta(int idEspecialidad, DateTime fechaConsulta)
        {
            if (idEspecialidad <= 0)
            {
                throw new ArgumentException("Debe especificar una especialidad.");
            }

            if (fechaConsulta.Date < DateTime.Today || fechaConsulta.Date > DateTime.Today.AddDays(DiasMaximosAnticipacion))
            {
                throw new ArgumentException("La fecha de consulta debe estar entre hoy y " + DiasMaximosAnticipacion + " dias en el futuro.");
            }
        }

        private List<Turno> ObtenerTurnosOcupados(int idMedico, DateTime fecha)
        {
            List<Turno> turnos = turnoNegocio.ListarPorMedicoYFecha(idMedico, fecha);
            List<Turno> turnosOcupados = new List<Turno>();

            foreach (Turno turno in turnos)
            {
                if (turno != null
                    && turno.EstadoTurno != null
                    && !turno.EstadoTurno.EsFinal)
                {
                    turnosOcupados.Add(turno);
                }
            }

            return turnosOcupados;
        }

        private int ObtenerDuracionBloque()
        {
            ConfiguracionTurno configuracionTurno = configuracionTurnoNegocio.Obtener();
            if (configuracionTurno == null)
            {
                throw new Exception("No existe configuracion de turno activa.");
            }

            return configuracionTurno.DuracionMinutos;
        }

        private DiaSemanaEnum ObtenerDiaSemana(DateTime fecha)
        {
            switch (fecha.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return DiaSemanaEnum.Lunes;
                case DayOfWeek.Tuesday:
                    return DiaSemanaEnum.Martes;
                case DayOfWeek.Wednesday:
                    return DiaSemanaEnum.Miercoles;
                case DayOfWeek.Thursday:
                    return DiaSemanaEnum.Jueves;
                case DayOfWeek.Friday:
                    return DiaSemanaEnum.Viernes;
                case DayOfWeek.Saturday:
                    return DiaSemanaEnum.Sabado;
                default:
                    return DiaSemanaEnum.Domingo;
            }
        }

        private List<TurnoDisponibleDTO> ConstruirBloquesDisponibles(
            Medico medico,
            DateTime fecha,
            HorarioDisponibilidadMedico horario,
            int duracionBloque,
            List<Turno> turnosOcupados)
        {
            List<TurnoDisponibleDTO> turnos = new List<TurnoDisponibleDTO>();
            TimeSpan horaActual = horario.HoraDesde;
            TimeSpan duracion = TimeSpan.FromMinutes(duracionBloque);

            while (horaActual.Add(duracion) <= horario.HoraHasta)
            {
                TimeSpan horaFin = horaActual.Add(duracion);
                DateTime inicioTurno = fecha.Date.Add(horaActual);

                if (inicioTurno > DateTime.Now
                    && !ExisteSuperposicion(turnosOcupados, horaActual, horaFin))
                {
                    turnos.Add(new TurnoDisponibleDTO
                    {
                        FechaTurno = fecha,
                        HoraInicio = horaActual,
                        HoraFin = horaFin,
                        Medico = medico
                    });
                }

                horaActual = horaFin;
            }

            return turnos;
        }

        private List<HorarioDisponibilidadMedico> ObtenerHorariosDelDia(int idMedico, DiaSemanaEnum diaSemana)
        {
            List<HorarioDisponibilidadMedico> horarios = horarioDisponibilidadMedicoNegocio.ListarPorMedico(idMedico);
            List<HorarioDisponibilidadMedico> horariosDelDia = new List<HorarioDisponibilidadMedico>();

            foreach (HorarioDisponibilidadMedico horario in horarios)
            {
                if (horario != null && horario.DiaSemana == diaSemana)
                {
                    horariosDelDia.Add(horario);
                }
            }

            return horariosDelDia;
        }

        private List<TurnoDisponibleDTO> QuitarDuplicados(List<TurnoDisponibleDTO> turnosDisponibles)
        {
            List<TurnoDisponibleDTO> turnosSinDuplicados = new List<TurnoDisponibleDTO>();

            foreach (TurnoDisponibleDTO turno in turnosDisponibles)
            {
                if (!ExisteTurnoEnLista(turnosSinDuplicados, turno))
                {
                    turnosSinDuplicados.Add(turno);
                }
            }

            return turnosSinDuplicados;
        }

        private bool ExisteTurnoEnLista(List<TurnoDisponibleDTO> turnos, TurnoDisponibleDTO turnoBuscado)
        {
            foreach (TurnoDisponibleDTO turno in turnos)
            {
                if (turno.Medico != null
                    && turnoBuscado.Medico != null
                    && turno.Medico.IdMedico == turnoBuscado.Medico.IdMedico
                    && turno.FechaTurno == turnoBuscado.FechaTurno
                    && turno.HoraInicio == turnoBuscado.HoraInicio
                    && turno.HoraFin == turnoBuscado.HoraFin)
                {
                    return true;
                }
            }

            return false;
        }

        private bool ExisteSuperposicion(List<Turno> turnosOcupados, TimeSpan horaInicio, TimeSpan horaFin)
        {
            foreach (Turno turno in turnosOcupados)
            {
                if (SeSuperpone(turno.HoraInicio, turno.HoraFin, horaInicio, horaFin))
                {
                    return true;
                }
            }

            return false;
        }

        private bool SeSuperpone(TimeSpan inicioExistente, TimeSpan finExistente, TimeSpan inicioNuevo, TimeSpan finNuevo)
        {
            return inicioNuevo < finExistente && inicioExistente < finNuevo;
        }
    }
}
