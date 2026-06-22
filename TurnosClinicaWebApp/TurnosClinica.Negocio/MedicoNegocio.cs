using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Negocio
{
    public class MedicoNegocio : IEntidadGestionableNegocio<Medico>
    {
        private readonly MedicoDatos medicoDatos;
        private readonly EspecialidadNegocio especialidadNegocio;

        public MedicoNegocio()
        {
            medicoDatos = new MedicoDatos(new AccesoDatosBase());
            especialidadNegocio = new EspecialidadNegocio();
        }

        public List<Medico> Listar(bool? activo = null)
        {
            return medicoDatos.Listar(activo);
        }

        public List<Medico> ListarFiltroRapido(string palabra, bool? activo = null)
        {
            return medicoDatos.ListarFiltroRapido(palabra, activo);
        }

        public List<Medico> ListarFiltroAvanzado(string campo, string criterio, string filtro, bool? activo = null)
        {
            return medicoDatos.ListarFiltroAvanzado(campo, criterio, filtro, activo);
        }

        public List<Medico> ListarPorEspecialidad(int idEspecialidad)
        {
            if (idEspecialidad <= 0)
            {
                throw new Exception("El id de la especialidad no es valido.");
            }

            Especialidad especialidad = especialidadNegocio.ObtenerPorId(idEspecialidad);
            if (especialidad == null)
            {
                throw new Exception("La especialidad no existe.");
            }

            return medicoDatos.ListarPorEspecialidad(idEspecialidad);
        }

        public Medico ObtenerPorId(int id)
        {
            if (id <= 0)
            {
                throw new Exception("El id del medico no es valido.");
            }

            return medicoDatos.ObtenerPorId(id);
        }

        public void Agregar(Medico medico)
        {
            ValidarAlta(medico);

            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    PersonaNegocio personaNegocio = new PersonaNegocio(manejador.CrearAccesoDatos());
                    MedicoDatos medicoDatos = new MedicoDatos(manejador.CrearAccesoDatos());
                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio(manejador.CrearAccesoDatos());
                    medico.Persona.IdPersona = personaNegocio.Agregar(medico.Persona);
                    medicoDatos.Agregar(medico);
                    usuarioNegocio.Agregar(ConstruirUsuarioAsociado(medico));
                    GuardarEspecialidades(medico, manejador.CrearAccesoDatos());
                    GuardarHorarios(medico, manejador.CrearAccesoDatos());

                    manejador.Confirmar();
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }
        }

        public void Modificar(Medico medico)
        {
            ValidarModificacion(medico);

            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    PersonaNegocio personaNegocio = new PersonaNegocio(manejador.CrearAccesoDatos());
                    MedicoDatos datos = new MedicoDatos(manejador.CrearAccesoDatos());
                    personaNegocio.Modificar(medico.Persona);
                    datos.Modificar(medico);
                    GuardarEspecialidades(medico, manejador.CrearAccesoDatos());
                    GuardarHorarios(medico, manejador.CrearAccesoDatos());

                    manejador.Confirmar();
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }
        }

        public void Desactivar(int id)
        {
            Medico medico = ObtenerPorId(id);
            ValidarDesactivacion(medico);
            medicoDatos.Desactivar(id);
        }

        public void Activar(int id)
        {
            Medico medico = ObtenerPorId(id);
            ValidarActivacion(medico);
            medicoDatos.Activar(id);
        }

        private void ValidarAlta(Medico medico)
        {
            ValidarMedico(medico);

            if (medicoDatos.ExisteMatricula(medico.Matricula.Trim()))
            {
                throw new Exception("Ya existe un medico registrado con esa matricula.");
            }
        }

        private void ValidarModificacion(Medico medico)
        {
            ValidarMedico(medico);

            if (medico.IdMedico <= 0)
            {
                throw new Exception("El id del medico no es valido.");
            }

            if (medico.Persona.IdPersona <= 0)
            {
                throw new Exception("El id de persona no es valido.");
            }

            Medico medicoActual = ObtenerPorId(medico.IdMedico);
            if (medicoActual == null)
            {
                throw new Exception("El medico no existe.");
            }

            if (!string.Equals(medicoActual.Matricula, medico.Matricula, StringComparison.OrdinalIgnoreCase)
                && medicoDatos.ExisteMatricula(medico.Matricula.Trim()))
            {
                throw new Exception("Ya existe un medico registrado con esa matricula.");
            }
        }

        private void ValidarDesactivacion(Medico medico)
        {
            if (medico == null)
            {
                throw new Exception("El medico no existe.");
            }

            if (!medico.Activo)
            {
                throw new Exception("El medico ya esta inactivo.");
            }
        }

        private void ValidarActivacion(Medico medico)
        {
            if (medico == null)
            {
                throw new Exception("El medico no existe.");
            }

            if (medico.Activo)
            {
                throw new Exception("El medico ya esta activo.");
            }
        }

        private void ValidarMedico(Medico medico)
        {
            if (medico == null)
            {
                throw new Exception("El medico es obligatorio.");
            }

            if (medico.Persona == null)
            {
                throw new Exception("La persona del medico es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(medico.Matricula))
            {
                throw new Exception("La matricula es obligatoria.");
            }

            medico.Matricula = medico.Matricula.Trim();
            ValidarEspecialidades(medico.Especialidades);
            ValidarHorarios(medico.HorariosDisponibilidad);
        }

        private void ValidarEspecialidades(List<Especialidad> especialidades)
        {
            if (especialidades == null)
            {
                return;
            }

            for (int i = 0; i < especialidades.Count; i++)
            {
                if (especialidades[i] == null || especialidades[i].IdEspecialidad <= 0)
                {
                    throw new Exception("La especialidad seleccionada no es valida.");
                }

                Especialidad especialidad = especialidadNegocio.ObtenerPorId(especialidades[i].IdEspecialidad);
                if (especialidad == null || especialidad.IdEspecialidad <= 0 || !especialidad.Activo)
                {
                    throw new Exception("La especialidad seleccionada no esta disponible.");
                }

                for (int j = i + 1; j < especialidades.Count; j++)
                {
                    if (especialidades[j] != null
                        && especialidades[i].IdEspecialidad == especialidades[j].IdEspecialidad)
                    {
                        throw new Exception("No se puede repetir una especialidad.");
                    }
                }
            }
        }

        //Validamos bien los horarios para que no se solapen
        private void ValidarHorarios(List<HorarioDisponibilidadMedico> horarios)
        {
            if (horarios == null)
            {
                return;
            }

            for (int i = 0; i < horarios.Count; i++)
            {
                HorarioDisponibilidadMedico horario = horarios[i];

                if (horario == null || !Enum.IsDefined(typeof(DiaSemanaEnum), horario.DiaSemana))
                {
                    throw new Exception("El dia del horario no es valido.");
                }

                if (horario.HoraHasta <= horario.HoraDesde)
                {
                    throw new Exception("La hora hasta debe ser mayor a la hora desde.");
                }

                for (int j = i + 1; j < horarios.Count; j++)
                {
                    HorarioDisponibilidadMedico otroHorario = horarios[j];
                    if (otroHorario != null
                        && horario.DiaSemana == otroHorario.DiaSemana
                        && horario.HoraDesde < otroHorario.HoraHasta
                        && otroHorario.HoraDesde < horario.HoraHasta)
                    {
                        throw new Exception("Los horarios del mismo dia no pueden superponerse.");
                    }
                }
            }
        }

        private void GuardarEspecialidades(Medico medico, AccesoDatosBase accesoDatos)
        {
            MedicoEspecialidadesDatos especialidadesDatos = new MedicoEspecialidadesDatos(accesoDatos);
            especialidadesDatos.ReemplazarPorMedico(medico.IdMedico, medico.Especialidades);
        }

        private void GuardarHorarios(Medico medico, AccesoDatosBase accesoDatos)
        {
            HorarioDisponibilidadMedicoDatos horariosDatos = new HorarioDisponibilidadMedicoDatos(accesoDatos);
            horariosDatos.ReemplazarPorMedico(medico.IdMedico, medico.HorariosDisponibilidad);
        }

        private Usuario ConstruirUsuarioAsociado(Medico medico)
        {
            return new Usuario
            {
                Persona = medico.Persona,
                NombreUsuario = null,
                PasswordHash = null,
                Imagen = null,
                Rol = new Rol
                {
                    Nombre = RolEnum.Medico.ToString()
                }
            };
        }
    }
}
