using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnosClinica.Dominio
{
    internal class DiaAtencionMedico
    {
        public int IdDiaAtencionMedico { get; set; }
        public int IdMedico { get; set; }
        public byte DiaSemana { get; set; }
        public TimeSpan HoraDesde { get; set; }
        public TimeSpan HoraHasta { get; set; }
        public bool Activo { get; set; }
    }
}
