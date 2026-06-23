using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class TurnoNegocio
    {
        private readonly TurnoDatos turnoDatos;

        public TurnoNegocio()
        {
            turnoDatos = new TurnoDatos();
        }

        public Turno ObtenerPorId(int idTurno)
        {
            if (idTurno <= 0)
            {
                throw new Exception("El id del turno no es valido.");
            }

            return turnoDatos.ObtenerPorId(idTurno);
        }

        public List<Turno> ListarPorMedicoYFecha(int idMedico, DateTime fecha)
        {
            if (idMedico <= 0)
            {
                throw new Exception("El id del medico no es valido.");
            }

            return turnoDatos.ListarPorMedicoYFecha(idMedico, fecha);
        }

        public List<Turno> ListarPorPacienteYFecha(int idPaciente, DateTime fecha)
        {
            if (idPaciente <= 0)
            {
                throw new Exception("El id del paciente no es valido.");
            }

            return turnoDatos.ListarPorPacienteYFecha(idPaciente, fecha);
        }
    }
}
