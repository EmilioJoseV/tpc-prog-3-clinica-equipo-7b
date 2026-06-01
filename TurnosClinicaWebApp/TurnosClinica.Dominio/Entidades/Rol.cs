using System.Collections.Generic;

namespace TurnosClinica.Dominio.Entidades
{
    public class Rol
    {
        public Rol()
        {
            Usuarios = new List<Usuario>();
        }

        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; } = true;

        public List<Usuario> Usuarios { get; set; }
    }
}
