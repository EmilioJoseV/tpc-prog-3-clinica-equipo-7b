using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class PacienteDatos : IFiltrable<Paciente>, IMapeable<Paciente>
    {
        private readonly AccesoDatos accesoDatos;

        public PacienteDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<Paciente> Listar(bool activo)
        {
            List<Paciente> pacientes = new List<Paciente>();
            try
            {
                return pacientes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Paciente ObtenerPorId(int idPaciente)
        {
            Paciente paciente = new Paciente();
            try
            {
                return paciente;
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

        public int Agregar(Paciente paciente)
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

        public bool Modificar(Paciente paciente)
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

        public List<Paciente> ListarConFiltros(string campo, string criterio, string filtro, bool? activo)
        {
            throw new NotImplementedException();
        }

        public Paciente MapearFilaAEntidad(SqlDataReader fila)
        {
            throw new NotImplementedException();
        }
    }
}
