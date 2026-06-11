using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class MedicoDatos : IFiltrable<Medico>, IMapeable<Medico>
    {
        private readonly AccesoDatos accesoDatos;

        public MedicoDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<Medico> Listar(bool activo)
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

        public List<Medico> ListarConFiltros(string campo, string criterio, string filtro, bool? activo)
        {
            throw new NotImplementedException();
        }

        public Medico MapearFilaAEntidad(SqlDataReader fila)
        {
            throw new NotImplementedException();
        }
    }
}
