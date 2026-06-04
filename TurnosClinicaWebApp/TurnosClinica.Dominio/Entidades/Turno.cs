using System;

namespace TurnosClinica.Dominio.Entidades
{
    public class Turno
    {
        public int IdTurno { get; set; }
        public string NumeroTurno { get; set; }
        public DateTime FechaTurno { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string Observaciones { get; set; }
        public string DiagnosticoMedico { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaModificacion { get; set; }
        public Paciente Paciente { get; set; }
        public Medico Medico { get; set; }
        public Especialidad Especialidad { get; set; }
        public EstadoTurno EstadoTurno { get; set; }
        public Usuario UsuarioAlta { get; set; }
        public Usuario UsuarioModificacion { get; set; }
    }
}
