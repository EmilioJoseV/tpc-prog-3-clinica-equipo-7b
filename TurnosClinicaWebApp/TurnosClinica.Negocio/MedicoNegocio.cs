using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class MedicoNegocio
    {
        private readonly MedicoDatos medicoDatos;

        public MedicoNegocio()
        {
            medicoDatos = new MedicoDatos();
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
            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();

                    MedicoDatos medicoDatos = new MedicoDatos(transaccionDatos.AccesoDatos);
                    MedicoEspecialidadesDatos medicoEspecialidadesDatos = new MedicoEspecialidadesDatos(transaccionDatos.AccesoDatos);
                    HorarioDisponibilidadMedicoDatos horarioDisponibilidadMedicoDatos = new HorarioDisponibilidadMedicoDatos(transaccionDatos.AccesoDatos);

                    medicoDatos.Agregar(medico);
                    medicoEspecialidadesDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.Especialidades);
                    horarioDisponibilidadMedicoDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.HorariosDisponibilidad);

                    transaccionDatos.Confirmar();
                    return;
                }
                catch (Exception ex)
                {
                    transaccionDatos.Cancelar();
                    throw ex;
                }
            }
        }

        public void Modificar(Medico medico)
        {
            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();

                    MedicoDatos medicoDatosTransaccional = new MedicoDatos(transaccionDatos.AccesoDatos);
                    MedicoEspecialidadesDatos medicoEspecialidadesDatos = new MedicoEspecialidadesDatos(transaccionDatos.AccesoDatos);
                    HorarioDisponibilidadMedicoDatos horarioDisponibilidadMedicoDatos = new HorarioDisponibilidadMedicoDatos(transaccionDatos.AccesoDatos);

                    medicoDatosTransaccional.Modificar(medico);
                    medicoEspecialidadesDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.Especialidades);
                    horarioDisponibilidadMedicoDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.HorariosDisponibilidad);

                    transaccionDatos.Confirmar();
                    return;
                }
                catch (Exception ex)
                {
                    transaccionDatos.Cancelar();
                    throw ex;
                }
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
    }
}
