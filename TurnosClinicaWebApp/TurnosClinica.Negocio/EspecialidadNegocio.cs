using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class EspecialidadNegocio
    {
        private readonly EspecialidadDatos especialidadDatos;

        public EspecialidadNegocio()
        {
            especialidadDatos = new EspecialidadDatos();
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

        public void Agregar(Especialidad especialidad)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(especialidad.Nombre))
                {
                    throw new Exception("El nombre de la especialidad es obligatorio.");
                }

                ValidarNombreDuplicado(especialidad);

                especialidadDatos.Agregar(especialidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Modificar(Especialidad especialidad)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(especialidad.Nombre))
                {
                    throw new Exception("El nombre es obligatorio.");

                }

                ValidarNombreDuplicado(especialidad);

                especialidadDatos.Modificar(especialidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValidarNombreDuplicado(Especialidad especialidad)
        {
            if (especialidadDatos.ExisteNombre(especialidad.Nombre, especialidad.IdEspecialidad))
            {
                throw new Exception("ya existe una especialidad registrada con ese nombre");
            }
        }




    }
}