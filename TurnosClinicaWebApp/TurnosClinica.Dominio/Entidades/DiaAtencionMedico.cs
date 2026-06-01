using System;

namespace TurnosClinica.Dominio.Entidades
{
    public class DiaAtencionMedico
    {
        public int IdDiaAtencionMedico { get; set; }
        public int IdMedico { get; set; }
        public byte DiaSemana { get; set; }
        public TimeSpan HoraDesde { get; set; }
        public TimeSpan HoraHasta { get; set; }
        public bool Activo { get; set; } = true;

        public Medico Medico { get; set; }
    }
}
