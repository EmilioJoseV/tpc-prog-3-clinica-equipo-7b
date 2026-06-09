using System;
using System.Collections.Generic;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class EstadoTurnoDatos
    {
        private readonly AccesoDatos accesoDatos;

        public EstadoTurnoDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<EstadoTurno> Listar()
        {
            List<EstadoTurno> estados = new List<EstadoTurno>();
            try
            {
                return estados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public List<EstadoTurno> ObtenerTodosActivos()
        {
            List<EstadoTurno> estados = new List<EstadoTurno>();
            try
            {
                return estados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EstadoTurno ObtenerPorNombre(string nombre)
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
