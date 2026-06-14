using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class EspecialidadNegocio
    {
        private readonly EspecialidadDatos especialidadDatos;
        private readonly MedicoEspecialidadesDatos medicoEspecialidadesDatos;

        public EspecialidadNegocio()
        {
            especialidadDatos = new EspecialidadDatos();
            medicoEspecialidadesDatos = new MedicoEspecialidadesDatos();
        }

        public List<Especialidad> Listar(bool activo)
        {
            try
            {
                return especialidadDatos.Listar(activo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Especialidad> ListarPorMedico(int idMedico, bool? activo = null)
        {
            try
            {
                return medicoEspecialidadesDatos.ListarPorMedico(idMedico);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Especialidad ObtenerPorId(int idEspecialidad)
        {
            try
            {
                return especialidadDatos.ObtenerPorId(idEspecialidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Agregar(Especialidad nueva)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(nueva.Nombre))
                {
                    throw new Exception("El nombre de la especialidad es obligatorio.");
                }

                especialidadDatos.Agregar(nueva);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}