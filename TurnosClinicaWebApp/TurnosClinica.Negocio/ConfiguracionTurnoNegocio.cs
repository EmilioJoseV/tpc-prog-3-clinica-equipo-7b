using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class ConfiguracionTurnoNegocio
    {
        private readonly ConfiguracionTurnoDatos configuracionTurnoDatos;

        public ConfiguracionTurnoNegocio()
        {
            configuracionTurnoDatos = new ConfiguracionTurnoDatos();
        }

        public ConfiguracionTurno ObtenerConfiguracionTurno()
        {
            return configuracionTurnoDatos.ObtenerConfiguracion();
        }

        public void ModificarConfiguracionTurno(ConfiguracionTurno configuracionTurno)
        {
            configuracionTurnoDatos.Modificar(configuracionTurno);
        }
    }
}
