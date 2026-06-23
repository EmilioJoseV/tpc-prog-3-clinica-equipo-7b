using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class TurnoDatos : IMapeable<Turno>
    {
        private readonly AccesoDatosBase accesoDatos;

        public TurnoDatos()
        {
            accesoDatos = new AccesoDatosBase();
        }

        public List<Turno> ListarPorMedicoYFecha(int idMedico, DateTime fecha)
        {
            List<Turno> turnos = new List<Turno>();

            try
            {
                accesoDatos.setearConsulta(
                    "SELECT T.IdTurno, T.NumeroTurno, T.IdPaciente, T.IdMedico, T.IdEspecialidad, T.IdEstadoTurno,"
                    + " T.FechaTurno, T.HoraInicio, T.HoraFin, T.Observaciones, T.DiagnosticoMedico,"
                    + " T.FechaAlta, T.IdUsuarioAlta, T.FechaModificacion, T.IdUsuarioModificacion,"
                    + " ET.Nombre AS EstadoNombre, ET.Descripcion AS EstadoDescripcion, ET.EsFinal, ET.Activo AS EstadoActivo"
                    + " FROM Turnos T"
                    + " INNER JOIN EstadosTurno ET ON ET.IdEstadoTurno = T.IdEstadoTurno"
                    + " WHERE T.IdMedico = @idMedico"
                    + " AND T.FechaTurno = @fecha"
                    + " ORDER BY T.HoraInicio");
                accesoDatos.setearParametro("@idMedico", idMedico);
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

        public List<Turno> Listar(int? idPaciente, int? idMedico, int? idUsuario, int? idUsuarioAlta, int? idUsuarioModificacion, bool? activo)
        {
            return new List<Turno>();
        }

        public Turno ObtenerPorId(int idTurno)
        {
            if (idTurno <= 0)
            {
                return null;
            }

            Turno turno = null;

            try
            {
                accesoDatos.setearConsulta(
                    "SELECT T.IdTurno, T.NumeroTurno, T.IdPaciente, T.IdMedico, T.IdEspecialidad, T.IdEstadoTurno,"
                    + " T.FechaTurno, T.HoraInicio, T.HoraFin, T.Observaciones, T.DiagnosticoMedico,"
                    + " T.FechaAlta, T.IdUsuarioAlta, T.FechaModificacion, T.IdUsuarioModificacion,"
                    + " ET.Nombre AS EstadoNombre, ET.Descripcion AS EstadoDescripcion, ET.EsFinal, ET.Activo AS EstadoActivo"
                    + " FROM Turnos T"
                    + " INNER JOIN EstadosTurno ET ON ET.IdEstadoTurno = T.IdEstadoTurno"
                    + " WHERE T.IdTurno = @idTurno");
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
            return 0;
        }

        public bool Modificar(Turno turno)
        {
            return false;
        }

        public List<Turno> ListarPorPacienteYFecha(int idPaciente, DateTime fecha)
        {
            List<Turno> turnos = new List<Turno>();

            try
            {
                accesoDatos.setearConsulta(
                    "SELECT T.IdTurno, T.NumeroTurno, T.IdPaciente, T.IdMedico, T.IdEspecialidad, T.IdEstadoTurno,"
                    + " T.FechaTurno, T.HoraInicio, T.HoraFin, T.Observaciones, T.DiagnosticoMedico,"
                    + " T.FechaAlta, T.IdUsuarioAlta, T.FechaModificacion, T.IdUsuarioModificacion,"
                    + " ET.Nombre AS EstadoNombre, ET.Descripcion AS EstadoDescripcion, ET.EsFinal, ET.Activo AS EstadoActivo"
                    + " FROM Turnos T"
                    + " INNER JOIN EstadosTurno ET ON ET.IdEstadoTurno = T.IdEstadoTurno"
                    + " WHERE T.IdPaciente = @idPaciente"
                    + " AND T.FechaTurno = @fecha"
                    + " ORDER BY T.HoraInicio");
                accesoDatos.setearParametro("@idPaciente", idPaciente);
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

        public Turno MapearFilaAEntidad(SqlDataReader fila)
        {
            Turno turno = new Turno
            {
                IdTurno = Convert.ToInt32(fila["IdTurno"]),
                NumeroTurno = fila["NumeroTurno"].ToString(),
                FechaTurno = Convert.ToDateTime(fila["FechaTurno"]),
                HoraInicio = TimeSpan.Parse(fila["HoraInicio"].ToString()),
                HoraFin = TimeSpan.Parse(fila["HoraFin"].ToString()),
                Observaciones = fila["Observaciones"].ToString(),
                DiagnosticoMedico = fila["DiagnosticoMedico"] is DBNull ? null : fila["DiagnosticoMedico"].ToString(),
                FechaAlta = Convert.ToDateTime(fila["FechaAlta"]),
                FechaModificacion = fila["FechaModificacion"] is DBNull ? default(DateTime) : Convert.ToDateTime(fila["FechaModificacion"]),
                Paciente = new Paciente
                {
                    IdPaciente = Convert.ToInt32(fila["IdPaciente"])
                },
                Medico = new Medico
                {
                    IdMedico = Convert.ToInt32(fila["IdMedico"])
                },
                Especialidad = new Especialidad
                {
                    IdEspecialidad = Convert.ToInt32(fila["IdEspecialidad"])
                },
                EstadoTurno = new EstadoTurno
                {
                    IdEstadoTurno = Convert.ToInt32(fila["IdEstadoTurno"]),
                    Nombre = fila["EstadoNombre"].ToString(),
                    Descripcion = fila["EstadoDescripcion"] is DBNull ? string.Empty : fila["EstadoDescripcion"].ToString(),
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
    }
}
