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
            ValidarEspecialidad(especialidad);

            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();
                    EspecialidadDatos datos = new EspecialidadDatos(transaccionDatos.AccesoDatos);
                    datos.Agregar(especialidad);
                    transaccionDatos.Confirmar();
                }
                catch (Exception ex)
                {
                    transaccionDatos.Cancelar();
                    throw ex;
                }
            }
        }

        public void Modificar(Especialidad especialidad)
        {
            ValidarEspecialidad(especialidad);

            using (TransaccionDatos transaccionDatos = new TransaccionDatos())
            {
                try
                {
                    transaccionDatos.IniciarTransaccion();
                    EspecialidadDatos datos = new EspecialidadDatos(transaccionDatos.AccesoDatos);
                    datos.Modificar(especialidad);
                    transaccionDatos.Confirmar();
                }
                catch (Exception ex)
                {
                    transaccionDatos.Cancelar();
                    throw ex;
                }
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                if (especialidadDatos.EstaAsociadaAMedico(id))
                {
                    throw new Exception("Accion incorrecta. No se puede eliminar la especialidad porque esta asignada a uno o mas medicos.");
                }

                using (TransaccionDatos transaccionDatos = new TransaccionDatos())
                {
                    try
                    {
                        transaccionDatos.IniciarTransaccion();
                        EspecialidadDatos datos = new EspecialidadDatos(transaccionDatos.AccesoDatos);
                        datos.Eliminar(id);
                        transaccionDatos.Confirmar();
                    }
                    catch (Exception ex)
                    {
                        transaccionDatos.Cancelar();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValidarEspecialidad(Especialidad especialidad)
        {
            if (especialidad == null)
            {
                throw new Exception("La especialidad es obligatoria");
            }

            if (string.IsNullOrWhiteSpace(especialidad.Nombre))
            {
                throw new Exception("El nombre de la especialidad es obligatorio");
            }

            if (especialidadDatos.ExisteNombre(especialidad.Nombre, especialidad.IdEspecialidad))
            {
                throw new Exception("Ya existe una especialidad registrada con ese nombre");
            }
        }
    }
}
