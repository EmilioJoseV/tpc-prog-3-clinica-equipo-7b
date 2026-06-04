using System;

namespace TurnosClinica.Dominio.Entidades
{
    public class HorarioDisponibilidadMedico
    {
        public int IdHorarioDisponibilidadMedico { get; set; }
        public int DiaSemana { get; set; }
        public TimeSpan HoraDesde { get; set; }
        public TimeSpan HoraHasta { get; set; }
        public bool Activo { get; set; } = true;
    }
}
