using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class MedicoNegocio
    {
        private readonly MedicoDatos medicoDatos;
        private readonly MedicoEspecialidadesDatos medicoEspecialidadesDatos;
        private readonly HorarioDisponibilidadMedicoDatos horarioDisponibilidadMedicoDatos;

        public MedicoNegocio()
        {
            medicoDatos = new MedicoDatos();
            medicoEspecialidadesDatos = new MedicoEspecialidadesDatos();
            horarioDisponibilidadMedicoDatos = new HorarioDisponibilidadMedicoDatos();
        }

        public List<Medico> Listar(bool? activo = null)
        {
            try
            {
                return medicoDatos.Listar(activo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Medico> ListarConFiltros(string campo, string criterio, string filtro, bool? activo = null)
        {
            try
            {
                return medicoDatos.ListarConFiltros(campo, criterio, filtro, activo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Medico ObtenerPorId(int idMedico)
        {
            try
            {
                return medicoDatos.ObtenerPorId(idMedico);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Agregar(Medico medico)
        {
            try
            {
                medicoDatos.Agregar(medico);
                medico.IdMedico = medicoDatos.ObtenerIdPorMatricula(medico.Matricula);
                GuardarRelaciones(medico);
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificar(Medico medico)
        {
            try
            {
                medicoDatos.Modificar(medico);
                GuardarRelaciones(medico);
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Desactivar(int idMedico)
        {
            try
            {
                return medicoDatos.Desactivar(idMedico);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GuardarRelaciones(Medico medico)
        {
            medicoEspecialidadesDatos.ReemplazarPorMedico(medico.IdMedico, medico.Especialidades);
            horarioDisponibilidadMedicoDatos.ReemplazarPorMedico(medico.IdMedico, medico.HorariosDisponibilidad);
        }
    }
}
