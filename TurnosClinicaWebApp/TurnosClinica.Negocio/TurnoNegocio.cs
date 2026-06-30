using System;
using System.Collections.Generic;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;
using TurnosClinica.Negocio.DTO;

namespace TurnosClinica.Negocio
{
    public class TurnoNegocio
    {
        private readonly TurnoDatos turnoDatos;

        public TurnoNegocio()
        {
            turnoDatos = new TurnoDatos(new AccesoDatosBase());
        }

        public List<Turno> Listar()
        {
            return turnoDatos.Listar();
        }

        public List<Turno> ListarFiltroRapido(string palabra)
        {
            return turnoDatos.ListarFiltroRapido(palabra);
        }

        public List<Turno> ListarPorFiltros(string palabra, int? idEstadoTurno, DateTime? fechaTurno)
        {
            return turnoDatos.ListarPorFiltros(palabra, idEstadoTurno, fechaTurno);
        }

        public List<Turno> ListarPorMedicoConFiltros(int idMedico, string palabra, int? idEstadoTurno, DateTime? fechaTurno)
        {
            if (idMedico <= 0)
            {
                throw new Exception("El id del medico no es valido.");
            }

            return turnoDatos.ListarPorMedicoConFiltros(idMedico, palabra, idEstadoTurno, fechaTurno);
        }

        public Turno ObtenerPorId(int idTurno)
        {
            if (idTurno <= 0)
            {
                throw new Exception("El id del turno no es valido.");
            }

            return turnoDatos.ObtenerPorId(idTurno);
        }

        public List<Turno> ListarPorMedicoYFecha(int idMedico, DateTime fecha)
        {
            if (idMedico <= 0)
            {
                throw new Exception("El id del medico no es valido.");
            }

            return turnoDatos.ListarPorMedicoYFecha(idMedico, fecha);
        }

        public List<Turno> ListarPorPacienteYFecha(int idPaciente, DateTime fecha)
        {
            if (idPaciente <= 0)
            {
                throw new Exception("El id del paciente no es valido.");
            }

            return turnoDatos.ListarPorPacienteYFecha(idPaciente, fecha);
        }

        public void Agregar(Turno turno)
        {
            ValidarAlta(turno);
            PrepararTurno(turno);

            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    EstadoTurnoNegocio estadoTurnoNegocio = new EstadoTurnoNegocio(manejador.CrearAccesoDatos());
                    TurnoDatos datos = new TurnoDatos(manejador.CrearAccesoDatos());
                    turno.EstadoTurno = estadoTurnoNegocio.ObtenerPorNombre(EstadoTurnoEnum.Nuevo.ToString());
                    turno.IdTurno = datos.Agregar(turno);
                    manejador.Confirmar();
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }

            EnviarConfirmacion(turno);
        }

        public void Modificar(Turno turno)
        {
            ValidarModificacion(turno);

            Turno turnoActual = ObtenerPorId(turno.IdTurno);
            ValidarTurnoEditable(turnoActual);

            turno.Paciente = turnoActual.Paciente;
            turno.Medico = turnoActual.Medico;
            turno.Especialidad = turnoActual.Especialidad;
            turno.FechaTurno = turnoActual.FechaTurno;
            turno.HoraInicio = turnoActual.HoraInicio;
            turno.HoraFin = turnoActual.HoraFin;
            turno.EstadoTurno = turnoActual.EstadoTurno;
            turno.UsuarioAlta = turnoActual.UsuarioAlta;
            turno.Observaciones = turno.Observaciones.Trim();
            turno.DiagnosticoMedico = string.IsNullOrWhiteSpace(turno.DiagnosticoMedico)
                ? null
                : turno.DiagnosticoMedico.Trim();

            turnoDatos.Modificar(turno);
        }

        public void Reprogramar(Turno turno)
        {
            ValidarReprogramacion(turno);

            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    TurnoDatos datos = new TurnoDatos(manejador.CrearAccesoDatos());
                    EstadoTurnoNegocio estadoTurnoNegocio = new EstadoTurnoNegocio(manejador.CrearAccesoDatos());
                    Turno turnoActual = datos.ObtenerPorId(turno.IdTurno);
                    ValidarTurnoEditable(turnoActual);

                    Paciente paciente = new PacienteNegocio().ObtenerPorId(turnoActual.Paciente.IdPaciente);
                    if (paciente == null || !paciente.Activo)
                    {
                        throw new Exception("El paciente del turno no esta disponible.");
                    }

                    Especialidad especialidad = new EspecialidadNegocio().ObtenerPorId(turnoActual.Especialidad.IdEspecialidad);
                    if (especialidad == null || !especialidad.Activo)
                    {
                        throw new Exception("La especialidad del turno no esta disponible.");
                    }

                    Medico medico = new MedicoNegocio().ObtenerPorId(turno.Medico.IdMedico);
                    if (medico == null || !medico.Activo)
                    {
                        throw new Exception("El medico seleccionado no esta disponible.");
                    }

                    bool atiendeEspecialidad = AtiendeEspecialidad(medico, especialidad.IdEspecialidad);
                    if (!atiendeEspecialidad)
                    {
                        throw new Exception("El medico no atiende la especialidad del turno.");
                    }

                    turno.Paciente = paciente;
                    turno.Especialidad = especialidad;
                    turno.UsuarioAlta = turnoActual.UsuarioAlta;
                    turno.EstadoTurno = estadoTurnoNegocio.ObtenerPorNombre(EstadoTurnoEnum.Reprogramado.ToString());
                    turno.Observaciones = turno.Observaciones.Trim();
                    turno.DiagnosticoMedico = string.IsNullOrWhiteSpace(turno.DiagnosticoMedico)
                        ? null
                        : turno.DiagnosticoMedico.Trim();

                    ValidarHorarioDisponible(turno);

                    if (datos.ExisteSuperposicionPacienteExcluyendoTurno(
                        turno.Paciente.IdPaciente,
                        turno.FechaTurno,
                        turno.HoraInicio,
                        turno.HoraFin,
                        turno.IdTurno))
                    {
                        throw new Exception("El paciente ya tiene un turno en ese horario.");
                    }

                    if (datos.ExisteSuperposicionMedicoExcluyendoTurno(
                        turno.Medico.IdMedico,
                        turno.FechaTurno,
                        turno.HoraInicio,
                        turno.HoraFin,
                        turno.IdTurno))
                    {
                        throw new Exception("El medico ya tiene un turno en ese horario.");
                    }

                    datos.Modificar(turno);
                    manejador.Confirmar();
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }
        }

        public void Cancelar(int idTurno, int idUsuarioModificacion)
        {
            CambiarEstado(idTurno, idUsuarioModificacion, EstadoTurnoEnum.Cancelado);
        }

        public void MarcarNoAsistio(int idTurno, int idUsuarioModificacion)
        {
            CambiarEstado(idTurno, idUsuarioModificacion, EstadoTurnoEnum.NoAsistio);
        }

        public void Cerrar(int idTurno, int idUsuarioModificacion)
        {
            CambiarEstado(idTurno, idUsuarioModificacion, EstadoTurnoEnum.Cerrado);
        }

        public void Cerrar(int idTurno, int idUsuarioModificacion, string diagnosticoMedico)
        {
            CambiarEstado(idTurno, idUsuarioModificacion, EstadoTurnoEnum.Cerrado, diagnosticoMedico);
        }

        public Turno ObtenerPorIdParaMedico(int idTurno, int idMedicoAutenticado)
        {
            Turno turno = ObtenerPorId(idTurno);
            ValidarPertenenciaMedico(turno, idMedicoAutenticado);
            return turno;
        }

        public void ModificarDiagnosticoMedico(int idTurno, string diagnosticoMedico, int idMedicoAutenticado, int idUsuarioModificacion)
        {
            if (idTurno <= 0)
            {
                throw new Exception("El id del turno no es valido.");
            }

            if (idMedicoAutenticado <= 0)
            {
                throw new Exception("El medico autenticado no es valido.");
            }

            if (idUsuarioModificacion <= 0)
            {
                throw new Exception("El usuario autenticado no es valido.");
            }

            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    TurnoDatos datos = new TurnoDatos(manejador.CrearAccesoDatos());
                    Turno turnoActual = datos.ObtenerPorId(idTurno);
                    ValidarPertenenciaMedico(turnoActual, idMedicoAutenticado);
                    ValidarTurnoEditable(turnoActual);

                    Usuario usuario = new UsuarioNegocio().ObtenerPorId(idUsuarioModificacion);
                    if (usuario == null)
                    {
                        throw new Exception("El usuario que modifica el turno no existe.");
                    }

                    ValidarDiagnosticoMedico(diagnosticoMedico);
                    datos.ModificarDiagnosticoMedico(idTurno, diagnosticoMedico, idUsuarioModificacion);
                    manejador.Confirmar();
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }
        }

        public void MarcarNoAsistioComoMedico(int idTurno, int idUsuarioModificacion, int idMedicoAutenticado)
        {
            CambiarEstadoComoMedico(idTurno, idUsuarioModificacion, idMedicoAutenticado, EstadoTurnoEnum.NoAsistio);
        }

        public void CerrarComoMedico(int idTurno, int idUsuarioModificacion, int idMedicoAutenticado, string diagnosticoMedico)
        {
            CambiarEstadoComoMedico(
                idTurno,
                idUsuarioModificacion,
                idMedicoAutenticado,
                EstadoTurnoEnum.Cerrado,
                diagnosticoMedico);
        }

        private void ValidarAlta(Turno turno)
        {
            if (turno == null)
            {
                throw new Exception("El turno es obligatorio.");
            }

            if (turno.Paciente == null || turno.Paciente.IdPaciente <= 0)
            {
                throw new Exception("Debe seleccionar un paciente.");
            }

            if (turno.Medico == null || turno.Medico.IdMedico <= 0)
            {
                throw new Exception("Debe seleccionar un medico.");
            }

            if (turno.Especialidad == null || turno.Especialidad.IdEspecialidad <= 0)
            {
                throw new Exception("Debe seleccionar una especialidad.");
            }

            if (turno.UsuarioAlta == null || turno.UsuarioAlta.IdUsuario <= 0)
            {
                throw new Exception("No se pudo identificar al usuario que registra el turno.");
            }

            if (string.IsNullOrWhiteSpace(turno.Observaciones))
            {
                throw new Exception("Las observaciones son obligatorias.");
            }

            if (turno.Observaciones.Trim().Length > 500)
            {
                throw new Exception("Las observaciones no pueden superar los 500 caracteres.");
            }

            Paciente paciente = new PacienteNegocio().ObtenerPorId(turno.Paciente.IdPaciente);
            if (paciente == null || !paciente.Activo)
            {
                throw new Exception("El paciente seleccionado no esta disponible.");
            }

            if (paciente.Persona == null || string.IsNullOrWhiteSpace(paciente.Persona.Email))
            {
                throw new Exception("El paciente debe tener un email para recibir la confirmacion.");
            }

            Especialidad especialidad = new EspecialidadNegocio().ObtenerPorId(turno.Especialidad.IdEspecialidad);
            if (especialidad == null || !especialidad.Activo)
            {
                throw new Exception("La especialidad seleccionada no esta disponible.");
            }

            Medico medico = new MedicoNegocio().ObtenerPorId(turno.Medico.IdMedico);
            if (medico == null || !medico.Activo)
            {
                throw new Exception("El medico seleccionado no esta disponible.");
            }

            bool atiendeEspecialidad = AtiendeEspecialidad(medico, especialidad.IdEspecialidad);
            if (!atiendeEspecialidad)
            {
                throw new Exception("El medico no atiende la especialidad seleccionada.");
            }

            Usuario usuario = new UsuarioNegocio().ObtenerPorId(turno.UsuarioAlta.IdUsuario);
            if (usuario == null)
            {
                throw new Exception("El usuario que registra el turno no existe.");
            }

            ValidarHorarioDisponible(turno);

            if (turnoDatos.ExisteSuperposicionPaciente(
                paciente.IdPaciente,
                turno.FechaTurno,
                turno.HoraInicio,
                turno.HoraFin))
            {
                throw new Exception("El paciente ya tiene un turno en ese horario.");
            }

            if (turnoDatos.ExisteSuperposicionMedico(
                medico.IdMedico,
                turno.FechaTurno,
                turno.HoraInicio,
                turno.HoraFin))
            {
                throw new Exception("El medico ya tiene un turno en ese horario.");
            }

            turno.Paciente = paciente;
            turno.Medico = medico;
            turno.Especialidad = especialidad;
            turno.UsuarioAlta = usuario;
        }

        private void ValidarModificacion(Turno turno)
        {
            if (turno == null)
            {
                throw new Exception("El turno es obligatorio.");
            }

            if (turno.IdTurno <= 0)
            {
                throw new Exception("El id del turno no es valido.");
            }

            if (turno.UsuarioModificacion == null || turno.UsuarioModificacion.IdUsuario <= 0)
            {
                throw new Exception("No se pudo identificar al usuario que modifica el turno.");
            }

            if (string.IsNullOrWhiteSpace(turno.Observaciones))
            {
                throw new Exception("Las observaciones son obligatorias.");
            }

            if (turno.Observaciones.Trim().Length > 500)
            {
                throw new Exception("Las observaciones no pueden superar los 500 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(turno.DiagnosticoMedico)
                && turno.DiagnosticoMedico.Trim().Length > 500)
            {
                throw new Exception("El diagnostico no puede superar los 500 caracteres.");
            }

            Usuario usuario = new UsuarioNegocio().ObtenerPorId(turno.UsuarioModificacion.IdUsuario);
            if (usuario == null)
            {
                throw new Exception("El usuario que modifica el turno no existe.");
            }

            turno.UsuarioModificacion = usuario;
        }

        private void ValidarDiagnosticoMedico(string diagnosticoMedico)
        {
            if (!string.IsNullOrWhiteSpace(diagnosticoMedico)
                && diagnosticoMedico.Trim().Length > 500)
            {
                throw new Exception("El diagnostico no puede superar los 500 caracteres.");
            }
        }

        private void ValidarReprogramacion(Turno turno)
        {
            ValidarModificacion(turno);

            if (turno.Medico == null || turno.Medico.IdMedico <= 0)
            {
                throw new Exception("Debe seleccionar un medico.");
            }
        }

        private void ValidarTurnoEditable(Turno turno)
        {
            if (turno == null)
            {
                throw new Exception("El turno no existe.");
            }

            if (turno.EstadoTurno != null && turno.EstadoTurno.EsFinal)
            {
                throw new Exception("El turno esta en un estado final y no admite cambios.");
            }
        }

        private void ValidarHorarioDisponible(Turno turno)
        {
            List<TurnoDisponibleDTO> disponibles = new TurnoCalculoService().ListarTurnosDisponibles(
                turno.Especialidad.IdEspecialidad,
                turno.FechaTurno,
                turno.Paciente.IdPaciente,
                turno.IdTurno);

            bool horarioDisponible = false;
            foreach (TurnoDisponibleDTO disponible in disponibles)
            {
                if (disponible.Medico != null
                    && disponible.Medico.IdMedico == turno.Medico.IdMedico
                    && disponible.FechaTurno.Date == turno.FechaTurno.Date
                    && disponible.HoraInicio == turno.HoraInicio
                    && disponible.HoraFin == turno.HoraFin)
                {
                    horarioDisponible = true;
                    break;
                }
            }

            if (!horarioDisponible)
            {
                throw new Exception("El horario seleccionado ya no esta disponible.");
            }
        }

        private void PrepararTurno(Turno turno)
        {
            turno.FechaTurno = turno.FechaTurno.Date;
            turno.Observaciones = turno.Observaciones.Trim();
            turno.DiagnosticoMedico = null;
        }

        private bool AtiendeEspecialidad(Medico medico, int idEspecialidad)
        {
            if (medico == null || medico.Especialidades == null)
            {
                return false;
            }

            foreach (Especialidad especialidad in medico.Especialidades)
            {
                if (especialidad != null && especialidad.IdEspecialidad == idEspecialidad)
                {
                    return true;
                }
            }

            return false;
        }

        private void ValidarPertenenciaMedico(Turno turno, int idMedicoAutenticado)
        {
            if (idMedicoAutenticado <= 0)
            {
                throw new Exception("El medico autenticado no es valido.");
            }

            if (turno == null)
            {
                throw new Exception("El turno no existe.");
            }

            if (turno.Medico == null || turno.Medico.IdMedico != idMedicoAutenticado)
            {
                throw new Exception("No tiene permisos para acceder a este turno.");
            }
        }

        private void CambiarEstado(
            int idTurno,
            int idUsuarioModificacion,
            EstadoTurnoEnum estadoDestino,
            string diagnosticoMedico = null)
        {
            if (idTurno <= 0)
            {
                throw new Exception("El id del turno no es valido.");
            }

            if (idUsuarioModificacion <= 0)
            {
                throw new Exception("El id del usuario no es valido.");
            }

            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    TurnoDatos datos = new TurnoDatos(manejador.CrearAccesoDatos());
                    EstadoTurnoNegocio estadoTurnoNegocio = new EstadoTurnoNegocio(manejador.CrearAccesoDatos());
                    Turno turnoActual = datos.ObtenerPorId(idTurno);
                    ValidarTurnoEditable(turnoActual);

                    Usuario usuario = new UsuarioNegocio().ObtenerPorId(idUsuarioModificacion);
                    if (usuario == null)
                    {
                        throw new Exception("El usuario que modifica el turno no existe.");
                    }

                    ValidarDiagnosticoMedico(diagnosticoMedico);

                    EstadoTurno estadoTurno = estadoTurnoNegocio.ObtenerPorNombre(estadoDestino.ToString());
                    if (estadoTurno == null)
                    {
                        throw new Exception("El estado del turno no existe.");
                    }

                    if (estadoDestino == EstadoTurnoEnum.Cerrado)
                    {
                        datos.ModificarDiagnosticoMedico(idTurno, diagnosticoMedico, idUsuarioModificacion);
                    }

                    datos.CambiarEstado(idTurno, estadoTurno.IdEstadoTurno, idUsuarioModificacion);
                    manejador.Confirmar();
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }
        }

        private void CambiarEstadoComoMedico(
            int idTurno,
            int idUsuarioModificacion,
            int idMedicoAutenticado,
            EstadoTurnoEnum estadoDestino,
            string diagnosticoMedico = null)
        {
            if (idTurno <= 0)
            {
                throw new Exception("El id del turno no es valido.");
            }

            if (idUsuarioModificacion <= 0)
            {
                throw new Exception("El id del usuario no es valido.");
            }

            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    TurnoDatos datos = new TurnoDatos(manejador.CrearAccesoDatos());
                    EstadoTurnoNegocio estadoTurnoNegocio = new EstadoTurnoNegocio(manejador.CrearAccesoDatos());
                    Turno turnoActual = datos.ObtenerPorId(idTurno);
                    ValidarPertenenciaMedico(turnoActual, idMedicoAutenticado);
                    ValidarTurnoEditable(turnoActual);

                    Usuario usuario = new UsuarioNegocio().ObtenerPorId(idUsuarioModificacion);
                    if (usuario == null)
                    {
                        throw new Exception("El usuario que modifica el turno no existe.");
                    }

                    ValidarDiagnosticoMedico(diagnosticoMedico);

                    if (estadoDestino == EstadoTurnoEnum.Cerrado)
                    {
                        datos.ModificarDiagnosticoMedico(idTurno, diagnosticoMedico, idUsuarioModificacion);
                    }

                    EstadoTurno estadoTurno = estadoTurnoNegocio.ObtenerPorNombre(estadoDestino.ToString());
                    if (estadoTurno == null)
                    {
                        throw new Exception("El estado del turno no existe.");
                    }

                    datos.CambiarEstado(idTurno, estadoTurno.IdEstadoTurno, idUsuarioModificacion);
                    manejador.Confirmar();
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }
        }

        private void EnviarConfirmacion(Turno turno)
        {
            try
            {
                new MailService().EnviarConfirmacionTurnoNuevo(turno);
            }
            catch
            {
                //No hacemos nada, si no se pudo enviar el mail, no es un error critico para la aplicacion o que bloquee la accion
            }
        }
    }
}
