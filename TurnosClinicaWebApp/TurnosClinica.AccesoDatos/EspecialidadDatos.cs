using System;
using System.Collections.Generic;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class EspecialidadDatos
    {
        private readonly AccesoDatos accesoDatos;

        public EspecialidadDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<Especialidad> Listar()
        {
            List<Especialidad> especialidades = new List<Especialidad>();
            try
            {
                return especialidades;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Especialidad ObtenerPorId(int idEspecialidad)
        {
            Especialidad especialidad = new Especialidad();
            try
            {
                return especialidad;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Especialidad> ObtenerTodosActivos()
        {
            List<Especialidad> especialidades = new List<Especialidad>();
            try
            {
                return especialidades;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Especialidad ObtenerPorNombre(string nombre)
        {
            Especialidad especialidad = new Especialidad();
            try
            {
                return especialidad;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExisteNombre(string nombre)
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

        public int Agregar(Especialidad especialidad)
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

        public bool Modificar(Especialidad especialidad)
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
