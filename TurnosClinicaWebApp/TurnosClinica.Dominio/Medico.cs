using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnosClinica.Dominio
{
    public class Medico
    {
        public int IdMedico { get; set; }
        public string Matricula { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Telefono { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public int IdTurnoMedico { get; set; }
        public TurnoTrabajo Turno { get; set; }
    }
}
