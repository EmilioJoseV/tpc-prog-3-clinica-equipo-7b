using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio.DTO
{
    public class TurnoDisponibleDTO
    {
        public DateTime FechaTurno { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public DateTime FechaAlta { get; set; }
        private Medico Medico { get; set; }
    }
}
