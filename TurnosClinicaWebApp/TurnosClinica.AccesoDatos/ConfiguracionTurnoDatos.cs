using System;
using System.Collections.Generic;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class ConfiguracionTurnoDatos
    {
        private readonly AccesoDatos accesoDatos;

        public ConfiguracionTurnoDatos()
        {
            accesoDatos = new AccesoDatos();
        }


        public ConfiguracionTurno ObtenerConfiguracion()
        {
            ConfiguracionTurno configuracionTurno = new ConfiguracionTurno();
            
            try
            {
                return configuracionTurno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Modificar(ConfiguracionTurno configuracionTurno)
        {
            try
            {
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
