using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnosClinica.Dominio
{
    public class TurnoTrabajo
    {
        public int IdTurnoTrabajo { get; set; }
        public string Nombre { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public bool Activo { get; set; } = true;
    }
}
