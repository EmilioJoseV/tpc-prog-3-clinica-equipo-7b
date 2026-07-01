using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Negocio
{
    public class PacienteNegocio : IEntidadGestionableNegocio<Paciente>
    {
        private readonly PacienteDatos pacienteDatos;

        public PacienteNegocio()
        {
            pacienteDatos = new PacienteDatos(new AccesoDatosBase());
        }

        public List<Paciente> Listar(bool? activo = null)
        {
            return pacienteDatos.Listar(activo);
        }

        public List<Paciente> ListarFiltroRapido(string palabra, bool? activo = null)
        {
            return pacienteDatos.ListarFiltroRapido(palabra, activo);
        }

        public List<Paciente> ListarFiltroAvanzado(string campo, string criterio, string filtro, bool? activo = null)
        {
            return pacienteDatos.ListarFiltroAvanzado(campo, criterio, filtro, activo);
        }

        public Paciente ObtenerPorId(int id)
        {
            if (id <= 0)
            {
                throw new Exception("El id del paciente no es valido.");
            }

            return pacienteDatos.ObtenerPorId(id);
        }

        public void Agregar(Paciente paciente)
        {
            ValidarAlta(paciente);

            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    PersonaNegocio personaNegocio = new PersonaNegocio(manejador.CrearAccesoDatos());
                    PacienteDatos datos = new PacienteDatos(manejador.CrearAccesoDatos());
                    UsuarioNegocio usuarioNegocio = new UsuarioNegocio(manejador.CrearAccesoDatos());

                    paciente.Persona.IdPersona = personaNegocio.Agregar(paciente.Persona);
                    datos.Agregar(paciente);
                    usuarioNegocio.Agregar(ConstruirUsuarioAsociado(paciente));

                    manejador.Confirmar();
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }
        }

        public void Modificar(Paciente paciente)
        {
            ValidarModificacion(paciente);

            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    PersonaNegocio personaNegocio = new PersonaNegocio(manejador.CrearAccesoDatos());
                    PacienteDatos datos = new PacienteDatos(manejador.CrearAccesoDatos());

                    personaNegocio.Modificar(paciente.Persona);
                    datos.Modificar(paciente);

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
            Paciente paciente = ObtenerPorId(id);
            ValidarDesactivacion(paciente);
            pacienteDatos.Desactivar(id);
        }

        public void Activar(int id)
        {
            Paciente paciente = ObtenerPorId(id);
            ValidarActivacion(paciente);
            pacienteDatos.Activar(id);
        }

        private void ValidarAlta(Paciente paciente)
        {
            ValidarPaciente(paciente);
        }

        private void ValidarModificacion(Paciente paciente)
        {
            ValidarPaciente(paciente);

            if (paciente.IdPaciente <= 0)
            {
                throw new Exception("El id del paciente no es valido.");
            }

            if (paciente.Persona.IdPersona <= 0)
            {
                throw new Exception("El id de persona no es valido.");
            }

            if (ObtenerPorId(paciente.IdPaciente) == null)
            {
                throw new Exception("El paciente no existe.");
            }
        }

        private void ValidarDesactivacion(Paciente paciente)
        {
            if (paciente == null)
            {
                throw new Exception("El paciente no existe.");
            }

            if (!paciente.Activo)
            {
                throw new Exception("El paciente ya esta inactivo.");
            }
        }

        private void ValidarActivacion(Paciente paciente)
        {
            if (paciente == null)
            {
                throw new Exception("El paciente no existe.");
            }

            if (paciente.Activo)
            {
                throw new Exception("El paciente ya esta activo.");
            }
        }

        private void ValidarPaciente(Paciente paciente)
        {
            if (paciente == null)
            {
                throw new Exception("El paciente es obligatorio.");
            }

            if (paciente.Persona == null)
            {
                throw new Exception("La persona del paciente es obligatoria");
            }

            if (paciente.FechaNacimiento == default(DateTime))
            {
                throw new Exception("La fecha de nacimiento es obligatoria");
            }

            if (paciente.FechaNacimiento.Date >= DateTime.Today)
            {
                throw new Exception("La fecha de nacimiento debe ser anterior a hoy");
            }

            if (string.IsNullOrWhiteSpace(paciente.Direccion))
            {
                throw new Exception("La direccion es obligatoria");
            }
        }

        private Usuario ConstruirUsuarioAsociado(Paciente paciente)
        {
            return new Usuario
            {
                Persona = paciente.Persona,
                NombreUsuario = null,
                PasswordHash = null,
                Imagen = null,
                Rol = new Rol
                {
                    Nombre = RolEnum.Paciente.ToString()
                }
            };
        }
    }
}
