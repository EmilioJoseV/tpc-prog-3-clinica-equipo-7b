using System;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Dominio.Entidades
{
    public class HorarioDisponibilidadMedico
    {
        public int IdHorarioDisponibilidadMedico { get; set; }
        public int IdMedico { get; set; }
        public DiaSemanaEnum DiaSemana { get; set; }
        public TimeSpan HoraDesde { get; set; }
        public TimeSpan HoraHasta { get; set; }
        public bool Activo { get; set; } = true;
    }
}
