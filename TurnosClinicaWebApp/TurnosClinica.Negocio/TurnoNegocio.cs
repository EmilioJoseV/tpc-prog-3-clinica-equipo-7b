using System;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class TurnoNegocio
    {
        private readonly ConfiguracionTurnoDatos configuracionTurnoDatos;

        public TurnoNegocio()
        {
            configuracionTurnoDatos = new ConfiguracionTurnoDatos();
        }

        public ConfiguracionTurno obtenerConfiguracionTurno()
        {
            return configuracionTurnoDatos.ObtenerConfiguracion();
        }

        public void modificarConfiguracionTurno(ConfiguracionTurno configuracionTurno)
        {
            configuracionTurnoDatos.Modificar(configuracionTurno);
        }
    }
}
