using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Dominio.Entidades
{
    public class Usuario : Persona
    {
        public Usuario()
        {
        }

        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string PasswordHash { get; set; }
        public byte[] Imagen { get; set; }
        public EstadoUsuarioEnum EstadoUsuario { get; set; }
        public Rol Rol { get; set; }
    }
}
