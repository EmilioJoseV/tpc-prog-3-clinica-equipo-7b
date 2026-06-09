using System;
using System.Collections.Generic;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class TurnoDatos
    {
        private readonly AccesoDatos accesoDatos;

        public TurnoDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<Turno> Listar()
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

        public Turno ObtenerPorNumeroTurno(string numeroTurno)
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

        public List<Turno> ListarPorPaciente(int idPaciente)
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

        public List<Turno> ListarPorMedico(int idMedico)
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

        public List<Turno> ListarPorFecha(DateTime fechaTurno)
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

        public List<Turno> ListarPorEstado(int idEstadoTurno)
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

        public List<Turno> ListarMisTurnos(int idMedico)
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

        public string ObtenerSiguienteNumeroTurno()
        {
            try
            {
                return string.Empty;
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
    }
}
