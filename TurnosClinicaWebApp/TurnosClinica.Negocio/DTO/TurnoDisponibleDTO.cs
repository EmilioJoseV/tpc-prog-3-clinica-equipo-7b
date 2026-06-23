using System;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio.DTO
{
    public class TurnoDisponibleDTO
    {
        public DateTime FechaTurno { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public DateTime FechaAlta { get; set; }
        public Medico Medico { get; set; }
    }
}
