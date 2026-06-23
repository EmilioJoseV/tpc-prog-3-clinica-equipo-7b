using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class HorarioDisponibilidadMedicoNegocio
    {
        private readonly HorarioDisponibilidadMedicoDatos horarioDisponibilidadMedicoDatos;

        public HorarioDisponibilidadMedicoNegocio()
        {
            horarioDisponibilidadMedicoDatos = new HorarioDisponibilidadMedicoDatos(new AccesoDatosBase());
        }

        public List<HorarioDisponibilidadMedico> ListarPorMedico(int idMedico)
        {
            if (idMedico <= 0)
            {
                throw new Exception("El id del medico no es valido.");
            }

            return horarioDisponibilidadMedicoDatos.ObtenerHorariosAsociadosAMedico(idMedico);
        }
    }
}
