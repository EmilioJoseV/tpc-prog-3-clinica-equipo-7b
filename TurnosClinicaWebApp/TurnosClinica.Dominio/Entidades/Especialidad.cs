using System.Collections.Generic;

namespace TurnosClinica.Dominio.Entidades
{
    public class Especialidad
    {
        public Especialidad()
        {
            MedicoEspecialidades = new List<MedicoEspecialidad>();
            Turnos = new List<Turno>();
        }

        public int IdEspecialidad { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; } = true;

        public List<MedicoEspecialidad> MedicoEspecialidades { get; set; }
        public List<Turno> Turnos { get; set; }
    }
}
