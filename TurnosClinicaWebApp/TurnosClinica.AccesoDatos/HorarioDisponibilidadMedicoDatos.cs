using System;
using System.Collections.Generic;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class HorarioDisponibilidadMedicoDatos
    {
        private readonly AccesoDatos accesoDatos;

        public HorarioDisponibilidadMedicoDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<HorarioDisponibilidadMedico> Listar()
        {
            List<HorarioDisponibilidadMedico> horarios = new List<HorarioDisponibilidadMedico>();
            try
            {
                return horarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public HorarioDisponibilidadMedico ObtenerPorId(int idHorarioDisponibilidadMedico)
        {
            HorarioDisponibilidadMedico horario = new HorarioDisponibilidadMedico();
            try
            {
                return horario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<HorarioDisponibilidadMedico> ObtenerTodosActivos()
        {
            List<HorarioDisponibilidadMedico> horarios = new List<HorarioDisponibilidadMedico>();
            try
            {
                return horarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<HorarioDisponibilidadMedico> ListarPorMedico(int idMedico)
        {
            List<HorarioDisponibilidadMedico> horarios = new List<HorarioDisponibilidadMedico>();
            try
            {
                return horarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<HorarioDisponibilidadMedico> ListarPorDiaSemana(int diaSemana)
        {
            List<HorarioDisponibilidadMedico> horarios = new List<HorarioDisponibilidadMedico>();
            try
            {
                return horarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public HorarioDisponibilidadMedico ObtenerPorMedicoYDia(int idMedico, int diaSemana)
        {
            HorarioDisponibilidadMedico horario = new HorarioDisponibilidadMedico();
            try
            {
                return horario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Agregar(HorarioDisponibilidadMedico horarioDisponibilidadMedico)
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

        public bool Modificar(HorarioDisponibilidadMedico horarioDisponibilidadMedico)
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
