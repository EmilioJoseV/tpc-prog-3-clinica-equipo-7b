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

        public MedicoNegocio()
        {
            medicoDatos = new MedicoDatos(new AccesoDatosBase());
        }

        public List<Medico> Listar(bool? activo = null)
        {
            return medicoDatos.Listar(activo);
        }

        public List<Medico> ListarFiltroRapido(string palabra, bool? activo = null)
        {
            return medicoDatos.ListarConFiltros(null, null, palabra, activo);
        }

        public List<Medico> ListarFiltroAvanzado(string campo, string criterio, string filtro, bool? activo = null)
        {
            return medicoDatos.ListarConFiltros(campo, criterio, filtro, activo);
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
                    MedicoEspecialidadesDatos especialidadesDatos = new MedicoEspecialidadesDatos(manejador.CrearAccesoDatos());
                    HorarioDisponibilidadMedicoDatos horariosDatos = new HorarioDisponibilidadMedicoDatos(manejador.CrearAccesoDatos());

                    medico.Persona.IdPersona = personaNegocio.Agregar(medico.Persona);
                    medicoDatos.Agregar(medico);
                    usuarioNegocio.Agregar(ConstruirUsuarioAsociado(medico));
                    especialidadesDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.Especialidades);
                    horariosDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.HorariosDisponibilidad);

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
                    MedicoEspecialidadesDatos especialidadesDatos = new MedicoEspecialidadesDatos(manejador.CrearAccesoDatos());
                    HorarioDisponibilidadMedicoDatos horariosDatos = new HorarioDisponibilidadMedicoDatos(manejador.CrearAccesoDatos());

                    personaNegocio.Modificar(medico.Persona);
                    datos.Modificar(medico);
                    especialidadesDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.Especialidades);
                    horariosDatos.AgregarActualizarPorMedico(medico.IdMedico, medico.HorariosDisponibilidad);

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
