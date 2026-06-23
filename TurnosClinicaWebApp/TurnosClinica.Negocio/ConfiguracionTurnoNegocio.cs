using System;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class ConfiguracionTurnoNegocio
    {
        private readonly ConfiguracionTurnoDatos configuracionTurnoDatos;
        private static readonly int duracionMinimaTurno = 5;

        public ConfiguracionTurnoNegocio()
        {
            configuracionTurnoDatos = new ConfiguracionTurnoDatos();
        }

        public ConfiguracionTurno Obtener()
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

        public void Modificar(ConfiguracionTurno configuracionTurno)
        {
            ValidarConfiguracionTurno(configuracionTurno);

            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();
                    ConfiguracionTurnoDatos datos = new ConfiguracionTurnoDatos(transaccionDatos.CrearAccesoDatos());
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

            if (configuracionTurno.DuracionMinutos < duracionMinimaTurno)
            {
                throw new Exception($"La duracion del turno debe ser al menos de {duracionMinimaTurno} minutos.");
            }
        }
    }
}
