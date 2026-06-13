using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class EstadoTurnoDatos : IMapeable<EstadoTurno>
    {
        private readonly AccesoDatos accesoDatos;

        public EstadoTurnoDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public EstadoTurno MapearFilaAEntidad(SqlDataReader fila)
        {
            EstadoTurno estadoTurno = new EstadoTurno();
            estadoTurno.IdEstadoTurno = Convert.ToInt32(fila["IdEstadoTurno"]);
            estadoTurno.Nombre = fila["Nombre"].ToString();
            estadoTurno.Descripcion = fila["Descripcion"] is DBNull ? string.Empty : fila["Descripcion"].ToString();
            estadoTurno.EsFinal = Convert.ToBoolean(fila["EsFinal"]);
            estadoTurno.Activo = Convert.ToBoolean(fila["Activo"]);
            return estadoTurno;
        }

        public EstadoTurno ObtenerPorId(int idEstadoTurno)
        {
            EstadoTurno estadoTurno = new EstadoTurno();
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT IdEstadoTurno, Nombre, Descripcion, EsFinal, Activo"
                    + " FROM EstadosTurno"
                    + " WHERE IdEstadoTurno = @idEstadoTurno");
                accesoDatos.setearParametro("@idEstadoTurno", idEstadoTurno);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    estadoTurno = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return estadoTurno;
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
    }
}
