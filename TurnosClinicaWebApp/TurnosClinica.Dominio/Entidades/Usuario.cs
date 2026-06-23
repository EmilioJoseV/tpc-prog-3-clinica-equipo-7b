namespace TurnosClinica.Dominio.Entidades
{
    public class Usuario
    {
        public Usuario()
        {
            Persona = new Persona();
        }

        public int IdUsuario { get; set; }
        public Persona Persona { get; set; }
        public string NombreUsuario { get; set; }
        public string PasswordHash { get; set; }
        public byte[] Imagen { get; set; }
        public EstadoUsuario EstadoUsuario { get; set; }
        public Rol Rol { get; set; }
    }
}
