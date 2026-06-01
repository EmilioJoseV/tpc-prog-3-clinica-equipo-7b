using System.Collections.Generic;

namespace TurnosClinica.Dominio.Entidades
{
    public class EstadoTurno
    {
        public EstadoTurno()
        {
            Turnos = new List<Turno>();
        }

        public int IdEstadoTurno { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EsFinal { get; set; }
        public bool Activo { get; set; } = true;

        public List<Turno> Turnos { get; set; }
    }
}
