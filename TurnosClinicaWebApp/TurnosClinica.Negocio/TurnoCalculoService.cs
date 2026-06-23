using System;
using System.Collections.Generic;
using System.Linq;
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

                List<HorarioDisponibilidadMedico> horariosDelDia = horarioDisponibilidadMedicoNegocio.ListarPorMedico(medico.IdMedico)
                    .Where(horario => horario.DiaSemana == diaSemana)
                    .OrderBy(horario => horario.HoraDesde)
                    .ToList();

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

            return turnosDisponibles
                .GroupBy(turno => new
                {
                    IdMedico = turno.Medico.IdMedico,
                    turno.FechaTurno,
                    turno.HoraInicio,
                    turno.HoraFin
                })
                .Select(grupo => grupo.First())
                .ToList();
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
            return turnos
                .Where(turno => turno != null
                    && turno.EstadoTurno != null
                    && !turno.EstadoTurno.EsFinal)
                .ToList();
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

                if (!turnosOcupados.Any(turno => SeSuperpone(turno.HoraInicio, turno.HoraFin, horaActual, horaFin)))
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

        private bool SeSuperpone(TimeSpan inicioExistente, TimeSpan finExistente, TimeSpan inicioNuevo, TimeSpan finNuevo)
        {
            return inicioNuevo < finExistente && inicioExistente < finNuevo;
        }
    }
}
