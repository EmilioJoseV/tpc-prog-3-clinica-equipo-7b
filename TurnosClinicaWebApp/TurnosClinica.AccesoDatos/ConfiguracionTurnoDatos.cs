using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class ConfiguracionTurnoDatos : IMapeable<ConfiguracionTurno>
    {
        private readonly AccesoDatos accesoDatos;

        public ConfiguracionTurnoDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public ConfiguracionTurno ObtenerConfiguracion(bool activo)
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

        public ConfiguracionTurno MapearFilaAEntidad(SqlDataReader fila)
        {
            throw new NotImplementedException();
        }
    }
}
