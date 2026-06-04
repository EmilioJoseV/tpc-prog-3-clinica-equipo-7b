using System.Collections.Generic;

namespace TurnosClinica.Dominio.Entidades
{
    public class EstadoTurno
    {
        public EstadoTurno()
        {
        }

        public int IdEstadoTurno { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EsFinal { get; set; }
        public bool Activo { get; set; } = true;
    }
}
