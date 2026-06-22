using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class EspecialidadNegocio : IEntidadGestionableNegocio<Especialidad>
    {
        private readonly EspecialidadDatos especialidadDatos;

        public EspecialidadNegocio()
        {
            especialidadDatos = new EspecialidadDatos(new AccesoDatosBase());
        }

        public List<Especialidad> Listar(bool? activo = null)
        {
            return especialidadDatos.Listar(activo);
        }

        public List<Especialidad> ListarFiltroRapido(string palabra, bool? activo = null)
        {
            return especialidadDatos.ListarFiltroRapido(palabra, activo);
        }

        public List<Especialidad> ListarFiltroAvanzado(string campo, string criterio, string filtro, bool? activo = null)
        {
            return especialidadDatos.ListarFiltroAvanzado(campo, criterio, filtro, activo);
        }

        public Especialidad ObtenerPorId(int id)
        {
            if (id <= 0)
            {
                throw new Exception("El id de la especialidad no es valido.");
            }

            return especialidadDatos.ObtenerPorId(id);
        }

        public void Agregar(Especialidad especialidad)
        {
            ValidarAlta(especialidad);
            PrepararEspecialidad(especialidad);
            especialidadDatos.Agregar(especialidad);
        }

        public void Modificar(Especialidad especialidad)
        {
            ValidarModificacion(especialidad);
            PrepararEspecialidad(especialidad);
            especialidadDatos.Modificar(especialidad);
        }

        public void Desactivar(int id)
        {
            Especialidad especialidad = ObtenerPorId(id);
            ValidarDesactivacion(especialidad);
            especialidadDatos.Desactivar(id);
        }

        public void Activar(int id)
        {
            Especialidad especialidad = ObtenerPorId(id);
            ValidarActivacion(especialidad);
            especialidadDatos.Activar(id);
        }

        private void ValidarAlta(Especialidad especialidad)
        {
            ValidarEspecialidad(especialidad);

            if (especialidadDatos.ExisteNombre(especialidad.Nombre.Trim()))
            {
                throw new Exception("Ya existe una especialidad registrada con ese nombre.");
            }
        }

        private void ValidarModificacion(Especialidad especialidad)
        {
            ValidarEspecialidad(especialidad);

            if (especialidad.IdEspecialidad <= 0)
            {
                throw new Exception("El id de la especialidad no es valido.");
            }

            Especialidad especialidadActual = ObtenerPorId(especialidad.IdEspecialidad);
            if (especialidadActual == null)
            {
                throw new Exception("La especialidad no existe.");
            }

            if (especialidadActual.Activo && !especialidad.Activo)
            {
                ValidarDesactivacion(especialidadActual);
            }

            if (!especialidadActual.Activo && especialidad.Activo)
            {
                ValidarActivacion(especialidadActual);
            }

            if (especialidadDatos.ExisteNombre(especialidad.Nombre.Trim(), especialidad.IdEspecialidad))
            {
                throw new Exception("Ya existe una especialidad registrada con ese nombre.");
            }
        }

        private void ValidarDesactivacion(Especialidad especialidad)
        {
            if (especialidad == null)
            {
                throw new Exception("La especialidad no existe.");
            }

            if (!especialidad.Activo)
            {
                throw new Exception("La especialidad ya esta inactiva.");
            }

            if (especialidadDatos.EstaAsociadaAMedico(especialidad.IdEspecialidad))
            {
                throw new Exception("No se puede desactivar la especialidad porque esta asignada a uno o mas medicos.");
            }
        }

        private void ValidarActivacion(Especialidad especialidad)
        {
            if (especialidad == null)
            {
                throw new Exception("La especialidad no existe.");
            }

            if (especialidad.Activo)
            {
                throw new Exception("La especialidad ya esta activa.");
            }
        }

        private void ValidarEspecialidad(Especialidad especialidad)
        {
            if (especialidad == null)
            {
                throw new Exception("La especialidad es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(especialidad.Nombre))
            {
                throw new Exception("El nombre de la especialidad es obligatorio.");
            }
        }

        //Le sacamos los espacios demas al nombre y a la descripcion
        private void PrepararEspecialidad(Especialidad especialidad)
        {
            especialidad.Nombre = especialidad.Nombre.Trim();
            especialidad.Descripcion = string.IsNullOrWhiteSpace(especialidad.Descripcion)
                ? null
                : especialidad.Descripcion.Trim();
        }
    }
}
