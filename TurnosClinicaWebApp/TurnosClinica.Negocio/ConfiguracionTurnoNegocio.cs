using System;
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
            try
            {
                return configuracionTurnoDatos.ObtenerConfiguracion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ModificarConfiguracionTurno(ConfiguracionTurno configuracionTurno)
        {
            ValidarConfiguracionTurno(configuracionTurno);

            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();
                    ConfiguracionTurnoDatos datos = new ConfiguracionTurnoDatos(transaccionDatos.AccesoDatos);
                    datos.Modificar(configuracionTurno);
                    transaccionDatos.Confirmar();
                }
                catch (Exception ex)
                {
                    transaccionDatos.Cancelar();
                    throw ex;
                }
            }
        }

        private void ValidarConfiguracionTurno(ConfiguracionTurno configuracionTurno)
        {
            if (configuracionTurno == null)
            {
                throw new Exception("La configuracion de turno es obligatoria.");
            }

            if (configuracionTurno.DuracionMinutos <= 0)
            {
                throw new Exception("La duracion del turno debe ser mayor a cero.");
            }
        }
    }
}
