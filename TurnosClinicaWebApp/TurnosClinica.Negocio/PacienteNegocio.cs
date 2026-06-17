using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class PacienteNegocio
    {
        private readonly PacienteDatos pacienteDatos;

        public PacienteNegocio()
        {
            pacienteDatos = new PacienteDatos();
        }

        public List<Paciente> Listar(bool? activo = null)
        {
            try
            {
                return pacienteDatos.Listar(activo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Paciente> ListarFiltroRapido(string filtro)
        {
            try
            {
                return pacienteDatos.ListarFiltroRapido(filtro);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Paciente> ListarConFiltros(string campo, string criterio, string filtro, bool? activo = null)
        {
            try
            {
                return pacienteDatos.ListarConFiltros(campo, criterio, filtro, activo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Paciente ObtenerPorId(int idPaciente)
        {
            try
            {
                return pacienteDatos.ObtenerPorId(idPaciente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Desactivar(int idPaciente)
        {
            try
            {
                return pacienteDatos.Desactivar(idPaciente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
