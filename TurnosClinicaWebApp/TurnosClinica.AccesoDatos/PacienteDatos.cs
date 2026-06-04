using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class PacienteDatos
    {
        private readonly AccesoDatos AccesoDatos;


        public PacienteDatos()
        {
            AccesoDatos = new AccesoDatos();
        }

        public List<Paciente> Listar()
        {
            try
            {
                return new List<Paciente>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
