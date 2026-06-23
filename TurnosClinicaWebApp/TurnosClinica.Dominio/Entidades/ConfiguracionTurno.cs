namespace TurnosClinica.Dominio.Entidades
{
    public class ConfiguracionTurno
    {
        public ConfiguracionTurno()
        {
            Activo = true;
        }

        public int IdConfiguracionTurno { get; set; }
        public int DuracionMinutos { get; set; }
        public bool Activo { get; set; }
    }
}
