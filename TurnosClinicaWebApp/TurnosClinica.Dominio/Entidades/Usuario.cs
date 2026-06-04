using System.Collections.Generic;

namespace TurnosClinica.Dominio.Entidades
{
    public class Usuario
    {
        public Usuario()
        {
            TurnosAlta = new List<Turno>();
            TurnosModificacion = new List<Turno>();
        }

        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool Activo { get; set; } = true;
        public Rol Rol { get; set; }
        public Medico Medico { get; set; }
        public List<Turno> TurnosAlta { get; set; }
        public List<Turno> TurnosModificacion { get; set; }
    }
}
