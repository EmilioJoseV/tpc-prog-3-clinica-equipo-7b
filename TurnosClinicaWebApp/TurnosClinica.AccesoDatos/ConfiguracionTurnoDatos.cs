using System;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class ConfiguracionTurnoDatos : IMapeable<ConfiguracionTurno>
    {
        private readonly AccesoDatosBase accesoDatos;

        public ConfiguracionTurnoDatos()
        {
            accesoDatos = new AccesoDatosBase();
        }

        public ConfiguracionTurnoDatos(AccesoDatosBase accesoDatosCompartido)
        {
            accesoDatos = accesoDatosCompartido;
        }

        public ConfiguracionTurno ObtenerConfiguracion()
        {
            try
            {
                string consulta = "SELECT TOP 1 IdConfiguracionTurno, DuracionMinutos, Activo"
                    + " FROM ConfiguracionesTurno"
                    + " WHERE Activo = 1"
                    + " ORDER BY IdConfiguracionTurno DESC";
                accesoDatos.setearConsulta(consulta);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    return MapearFilaAEntidad(accesoDatos.Lector);
                }

                return new ConfiguracionTurno();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public void Modificar(ConfiguracionTurno configuracionTurno)
        {
            try
            {
                if (configuracionTurno.IdConfiguracionTurno > 0)
                {
                    accesoDatos.setearConsulta(
                        "UPDATE ConfiguracionesTurno"
                        + " SET DuracionMinutos = @duracionMinutos"
                        + " WHERE IdConfiguracionTurno = @idConfiguracionTurno");
                    accesoDatos.setearParametro("@duracionMinutos", configuracionTurno.DuracionMinutos);
                    accesoDatos.setearParametro("@idConfiguracionTurno", configuracionTurno.IdConfiguracionTurno);
                    accesoDatos.ejecutarAccion();
                    return;
                }

                accesoDatos.setearConsulta(
                    "INSERT INTO ConfiguracionesTurno (DuracionMinutos, Activo)"
                    + " VALUES (@duracionMinutos, @activo)");
                accesoDatos.setearParametro("@duracionMinutos", configuracionTurno.DuracionMinutos);
                accesoDatos.setearParametro("@activo", configuracionTurno.Activo);
                accesoDatos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public ConfiguracionTurno MapearFilaAEntidad(SqlDataReader fila)
        {
            ConfiguracionTurno configuracion = new ConfiguracionTurno();
            configuracion.IdConfiguracionTurno = Convert.ToInt32(fila["IdConfiguracionTurno"]);
            configuracion.DuracionMinutos = Convert.ToInt32(fila["DuracionMinutos"]);
            configuracion.Activo = Convert.ToBoolean(fila["Activo"]);
            return configuracion;
        }
    }
}
