using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class TurnoDatos : IMapeable<Turno>
    {
        private readonly AccesoDatos accesoDatos;

        public TurnoDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<Turno> Listar(int? idPaciente, int? idMedico, int? idUsuario, int? idUsuarioAlta, int? idUsuarioModificacion, bool? activo)
        {
            List<Turno> turnos = new List<Turno>();
            try
            {
                return turnos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Turno ObtenerPorId(int idTurno)
        {
            Turno turno = new Turno();
            try
            {
                return turno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Agregar(Turno turno)
        {
            try
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Modificar(Turno turno)
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

        public Turno MapearFilaAEntidad(SqlDataReader fila)
        {
            throw new NotImplementedException();
        }
    }
}
