using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class TurnoDatos : IMapeable<Turno>
    {
        private readonly AccesoDatosBase accesoDatos;

        public TurnoDatos(AccesoDatosBase accesoDatos)
        {
            this.accesoDatos = accesoDatos;
        }

        public List<Turno> ListarPorMedicoYFecha(int idMedico, DateTime fecha)
        {
            return ListarPorFecha("T.IdMedico = @id", idMedico, fecha);
        }

        public List<Turno> ListarPorPacienteYFecha(int idPaciente, DateTime fecha)
        {
            return ListarPorFecha("T.IdPaciente = @id", idPaciente, fecha);
        }

        public Turno ObtenerPorId(int idTurno)
        {
            Turno turno = null;

            try
            {
                accesoDatos.setearConsulta(ObtenerConsultaBase() + " WHERE T.IdTurno = @idTurno");
                accesoDatos.setearParametro("@idTurno", idTurno);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    turno = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return turno;
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public int Agregar(Turno turno)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "INSERT INTO Turnos "
                    + "(IdPaciente, IdMedico, IdEspecialidad, IdEstadoTurno, "
                    + "FechaTurno, HoraInicio, HoraFin, Observaciones, DiagnosticoMedico, IdUsuarioAlta) "
                    + "VALUES (@idPaciente, @idMedico, @idEspecialidad, @idEstadoTurno, "
                    + "@fechaTurno, @horaInicio, @horaFin, @observaciones, @diagnosticoMedico, @idUsuarioAlta); "
                    + "SELECT CAST(SCOPE_IDENTITY() AS INT);");
                accesoDatos.setearParametro("@idPaciente", turno.Paciente.IdPaciente);
                accesoDatos.setearParametro("@idMedico", turno.Medico.IdMedico);
                accesoDatos.setearParametro("@idEspecialidad", turno.Especialidad.IdEspecialidad);
                accesoDatos.setearParametro("@idEstadoTurno", turno.EstadoTurno.IdEstadoTurno);
                accesoDatos.setearParametro("@fechaTurno", turno.FechaTurno.Date);
                accesoDatos.setearParametro("@horaInicio", turno.HoraInicio);
                accesoDatos.setearParametro("@horaFin", turno.HoraFin);
                accesoDatos.setearParametro("@observaciones", turno.Observaciones);
                accesoDatos.setearParametro("@diagnosticoMedico", string.IsNullOrWhiteSpace(turno.DiagnosticoMedico)
                    ? (object)DBNull.Value
                    : turno.DiagnosticoMedico.Trim());
                accesoDatos.setearParametro("@idUsuarioAlta", turno.UsuarioAlta.IdUsuario);
                return accesoDatos.ejecutarAccionScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public bool ExisteSuperposicionPaciente(
            int idPaciente,
            DateTime fecha,
            TimeSpan horaInicio,
            TimeSpan horaFin)
        {
            return ExisteSuperposicion("T.IdPaciente = @id", idPaciente, fecha, horaInicio, horaFin);
        }

        public Turno MapearFilaAEntidad(SqlDataReader fila)
        {
            Turno turno = new Turno
            {
                IdTurno = Convert.ToInt32(fila["IdTurno"]),
                FechaTurno = Convert.ToDateTime(fila["FechaTurno"]),
                HoraInicio = TimeSpan.Parse(fila["HoraInicio"].ToString()),
                HoraFin = TimeSpan.Parse(fila["HoraFin"].ToString()),
                Observaciones = fila["Observaciones"].ToString(),
                DiagnosticoMedico = fila["DiagnosticoMedico"] is DBNull
                    ? null
                    : fila["DiagnosticoMedico"].ToString(),
                FechaAlta = Convert.ToDateTime(fila["FechaAlta"]),
                FechaModificacion = fila["FechaModificacion"] is DBNull
                    ? default(DateTime)
                    : Convert.ToDateTime(fila["FechaModificacion"]),
                Paciente = new Paciente
                {
                    IdPaciente = Convert.ToInt32(fila["IdPaciente"]),
                    Persona = new Persona
                    {
                        IdPersona = Convert.ToInt32(fila["IdPersonaPaciente"]),
                        DNI = fila["DniPaciente"].ToString(),
                        Nombre = fila["NombrePaciente"].ToString(),
                        Apellido = fila["ApellidoPaciente"].ToString(),
                        Telefono = fila["TelefonoPaciente"] is DBNull ? string.Empty : fila["TelefonoPaciente"].ToString(),
                        Email = fila["EmailPaciente"].ToString()
                    },
                    Activo = Convert.ToBoolean(fila["PacienteActivo"])
                },
                Medico = new Medico
                {
                    IdMedico = Convert.ToInt32(fila["IdMedico"]),
                    Matricula = fila["Matricula"].ToString(),
                    Persona = new Persona
                    {
                        IdPersona = Convert.ToInt32(fila["IdPersonaMedico"]),
                        DNI = fila["DniMedico"].ToString(),
                        Nombre = fila["NombreMedico"].ToString(),
                        Apellido = fila["ApellidoMedico"].ToString(),
                        Telefono = fila["TelefonoMedico"] is DBNull ? string.Empty : fila["TelefonoMedico"].ToString(),
                        Email = fila["EmailMedico"].ToString()
                    },
                    Activo = Convert.ToBoolean(fila["MedicoActivo"])
                },
                Especialidad = new Especialidad
                {
                    IdEspecialidad = Convert.ToInt32(fila["IdEspecialidad"]),
                    Nombre = fila["NombreEspecialidad"].ToString(),
                    Descripcion = fila["DescripcionEspecialidad"] is DBNull
                        ? string.Empty
                        : fila["DescripcionEspecialidad"].ToString(),
                    Activo = Convert.ToBoolean(fila["EspecialidadActiva"])
                },
                EstadoTurno = new EstadoTurno
                {
                    IdEstadoTurno = Convert.ToInt32(fila["IdEstadoTurno"]),
                    Nombre = fila["EstadoNombre"].ToString(),
                    Descripcion = fila["EstadoDescripcion"] is DBNull
                        ? string.Empty
                        : fila["EstadoDescripcion"].ToString(),
                    EsFinal = Convert.ToBoolean(fila["EsFinal"]),
                    Activo = Convert.ToBoolean(fila["EstadoActivo"])
                },
                UsuarioAlta = new Usuario
                {
                    IdUsuario = Convert.ToInt32(fila["IdUsuarioAlta"])
                }
            };

            if (!(fila["IdUsuarioModificacion"] is DBNull))
            {
                turno.UsuarioModificacion = new Usuario
                {
                    IdUsuario = Convert.ToInt32(fila["IdUsuarioModificacion"])
                };
            }

            return turno;
        }

        private List<Turno> ListarPorFecha(string condicion, int id, DateTime fecha)
        {
            List<Turno> turnos = new List<Turno>();

            try
            {
                accesoDatos.setearConsulta(
                    ObtenerConsultaBase()
                    + " WHERE " + condicion
                    + " AND T.FechaTurno = @fecha"
                    + " ORDER BY T.HoraInicio");
                accesoDatos.setearParametro("@id", id);
                accesoDatos.setearParametro("@fecha", fecha.Date);
                accesoDatos.ejecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    turnos.Add(MapearFilaAEntidad(accesoDatos.Lector));
                }

                return turnos;
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        private bool ExisteSuperposicion(
            string condicion,
            int id,
            DateTime fecha,
            TimeSpan horaInicio,
            TimeSpan horaFin)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT COUNT(1)"
                    + " FROM Turnos T"
                    + " INNER JOIN EstadosTurno ET ON ET.IdEstadoTurno = T.IdEstadoTurno"
                    + " WHERE " + condicion
                    + " AND T.FechaTurno = @fecha"
                    + " AND ET.EsFinal = 0"
                    + " AND @horaInicio < T.HoraFin"
                    + " AND T.HoraInicio < @horaFin");
                accesoDatos.setearParametro("@id", id);
                accesoDatos.setearParametro("@fecha", fecha.Date);
                accesoDatos.setearParametro("@horaInicio", horaInicio);
                accesoDatos.setearParametro("@horaFin", horaFin);
                return accesoDatos.ejecutarAccionScalar() > 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        private string ObtenerConsultaBase()
        {
            return "SELECT T.IdTurno, T.IdPaciente, T.IdMedico, T.IdEspecialidad, T.IdEstadoTurno,"
                + " T.FechaTurno, T.HoraInicio, T.HoraFin, T.Observaciones, T.DiagnosticoMedico,"
                + " T.FechaAlta, T.IdUsuarioAlta, T.FechaModificacion, T.IdUsuarioModificacion,"
                + " ET.Nombre AS EstadoNombre, ET.Descripcion AS EstadoDescripcion, ET.EsFinal, ET.Activo AS EstadoActivo,"
                + " PA.IdPersona AS IdPersonaPaciente, PPA.DNI AS DniPaciente, PPA.Nombre AS NombrePaciente,"
                + " PPA.Apellido AS ApellidoPaciente, PPA.Telefono AS TelefonoPaciente, PPA.Email AS EmailPaciente,"
                + " PA.Activo AS PacienteActivo,"
                + " M.IdPersona AS IdPersonaMedico, PM.DNI AS DniMedico, PM.Nombre AS NombreMedico,"
                + " PM.Apellido AS ApellidoMedico, PM.Telefono AS TelefonoMedico, PM.Email AS EmailMedico,"
                + " M.Matricula, M.Activo AS MedicoActivo,"
                + " E.Nombre AS NombreEspecialidad, E.Descripcion AS DescripcionEspecialidad,"
                + " E.Activo AS EspecialidadActiva"
                + " FROM Turnos T"
                + " INNER JOIN EstadosTurno ET ON ET.IdEstadoTurno = T.IdEstadoTurno"
                + " INNER JOIN Pacientes PA ON PA.IdPaciente = T.IdPaciente"
                + " INNER JOIN Personas PPA ON PPA.IdPersona = PA.IdPersona"
                + " INNER JOIN Medicos M ON M.IdMedico = T.IdMedico"
                + " INNER JOIN Personas PM ON PM.IdPersona = M.IdPersona"
                + " INNER JOIN Especialidades E ON E.IdEspecialidad = T.IdEspecialidad";
        }
    }
}
