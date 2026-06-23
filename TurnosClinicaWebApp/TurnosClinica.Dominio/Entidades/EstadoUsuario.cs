namespace TurnosClinica.Dominio.Entidades
{
    public class EstadoUsuario
    {
        public EstadoUsuario()
        {
            Activo = true;
        }

        public int IdEstadoUsuario { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
