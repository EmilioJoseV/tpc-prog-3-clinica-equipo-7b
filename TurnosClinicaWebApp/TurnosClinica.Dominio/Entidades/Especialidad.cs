using System.Collections.Generic;

namespace TurnosClinica.Dominio.Entidades
{
    public class Especialidad
    {
        public Especialidad()
        {
        }

        public int IdEspecialidad { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; } = true;
    }
}
