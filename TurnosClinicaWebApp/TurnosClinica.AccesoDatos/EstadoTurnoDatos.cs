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
            throw new NotImplementedException();
        }

        public EstadoTurno ObtenerPorId(int idEstadoTurno)
        {
            EstadoTurno estadoTurno = new EstadoTurno();
            try
            {
                return estadoTurno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
