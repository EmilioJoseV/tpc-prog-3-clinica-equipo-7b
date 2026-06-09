using System;
using System.Collections.Generic;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class MedicoDatos
    {
        private readonly AccesoDatos accesoDatos;

        public MedicoDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<Medico> Listar()
        {
            List<Medico> medicos = new List<Medico>();
            try
            {
                return medicos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Medico ObtenerPorId(int idMedico)
        {
            Medico medico = new Medico();
            try
            {
                return medico;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Medico> ObtenerTodosActivos()
        {
            List<Medico> medicos = new List<Medico>();
            try
            {
                return medicos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Medico ObtenerPorDni(string dni)
        {
            Medico medico = new Medico();
            try
            {
                return medico;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Medico ObtenerPorMatricula(string matricula)
        {
            Medico medico = new Medico();
            try
            {
                return medico;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Medico ObtenerPorEmail(string email)
        {
            Medico medico = new Medico();
            try
            {
                return medico;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Medico> ListarPorEspecialidad(int idEspecialidad)
        {
            List<Medico> medicos = new List<Medico>();
            try
            {
                return medicos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<HorarioDisponibilidadMedico> ListarHorariosDisponibilidad(int idMedico)
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

        public bool ExisteDni(string dni)
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

        public bool ExisteMatricula(string matricula)
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

        public bool ExisteEmail(string email)
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

        public int Agregar(Medico medico)
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

        public bool Modificar(Medico medico)
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
