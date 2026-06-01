using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnosClinica.Dominio
{
    public class ConfiguracionTurno
    {
        public int IdConfiguracionTurno { get; set; }
        public int DuracionMinutos { get; set; }
        public bool Activo { get; set; } = true;
    }
}
