using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.AccesoDatos
{
    public class HorarioDisponibilidadMedicoDatos : IMapeable<HorarioDisponibilidadMedico>
    {
        private readonly AccesoDatos accesoDatos;

        public HorarioDisponibilidadMedicoDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<HorarioDisponibilidadMedico> Listar(int? idMedico, DiaSemanaEnum? diaSemana, TimeSpan? horaDesde, TimeSpan? horaHasta, bool? activo)
        {
            List<HorarioDisponibilidadMedico> horarios = new List<HorarioDisponibilidadMedico>();
            try
            {
                string consulta = "SELECT IdHorarioDisponiblidadMedico, IdMedico, DiaSemana, HoraDesde, HoraHasta, Activo"
                    + " FROM HorariosDisponiblidadMedicos";
                bool tieneCondicion = false;

                if (idMedico.HasValue)
                {
                    consulta += tieneCondicion ? " AND IdMedico = @idMedico" : " WHERE IdMedico = @idMedico";
                    tieneCondicion = true;
                }

                if (diaSemana.HasValue)
                {
                    consulta += tieneCondicion ? " AND DiaSemana = @diaSemana" : " WHERE DiaSemana = @diaSemana";
                    tieneCondicion = true;
                }

                if (horaDesde.HasValue)
                {
                    consulta += tieneCondicion ? " AND HoraDesde >= @horaDesde" : " WHERE HoraDesde >= @horaDesde";
                    tieneCondicion = true;
                }

                if (horaHasta.HasValue)
                {
                    consulta += tieneCondicion ? " AND HoraHasta <= @horaHasta" : " WHERE HoraHasta <= @horaHasta";
                    tieneCondicion = true;
                }

                if (activo.HasValue)
                {
                    consulta += tieneCondicion ? " AND Activo = @activo" : " WHERE Activo = @activo";
                    tieneCondicion = true;
                }

                consulta += " ORDER BY DiaSemana, HoraDesde";

                accesoDatos.setearConsulta(consulta);
                if (idMedico.HasValue)
                {
                    accesoDatos.setearParametro("@idMedico", idMedico.Value);
                }
                if (diaSemana.HasValue)
                {
                    accesoDatos.setearParametro("@diaSemana", (int)diaSemana.Value);
                }
                if (horaDesde.HasValue)
                {
                    accesoDatos.setearParametro("@horaDesde", horaDesde.Value);
                }
                if (horaHasta.HasValue)
                {
                    accesoDatos.setearParametro("@horaHasta", horaHasta.Value);
                }
                if (activo.HasValue)
                {
                    accesoDatos.setearParametro("@activo", activo.Value);
                }

                accesoDatos.ejecutarLectura();
                while (accesoDatos.Lector.Read())
                {
                    horarios.Add(MapearFilaAEntidad(accesoDatos.Lector));
                }

                return horarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public List<HorarioDisponibilidadMedico> ListarPorMedico(int idMedico)
        {
            return Listar(idMedico, null, null, null, null);
        }

        public void Agregar(HorarioDisponibilidadMedico horarioDisponibilidadMedico)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "INSERT INTO HorariosDisponiblidadMedicos (IdMedico, DiaSemana, HoraDesde, HoraHasta, Activo)"
                    + " VALUES (@idMedico, @diaSemana, @horaDesde, @horaHasta, @activo)");
                accesoDatos.setearParametro("@idMedico", horarioDisponibilidadMedico.IdMedico);
                accesoDatos.setearParametro("@diaSemana", horarioDisponibilidadMedico.DiaSemana);
                accesoDatos.setearParametro("@horaDesde", horarioDisponibilidadMedico.HoraDesde);
                accesoDatos.setearParametro("@horaHasta", horarioDisponibilidadMedico.HoraHasta);
                accesoDatos.setearParametro("@activo", horarioDisponibilidadMedico.Activo);
                accesoDatos.ejecutarAccion();
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public bool EliminarPorMedico(int idMedico)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "DELETE FROM HorariosDisponiblidadMedicos"
                    + " WHERE IdMedico = @idMedico");
                accesoDatos.setearParametro("@idMedico", idMedico);
                accesoDatos.ejecutarAccion();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public bool ReemplazarPorMedico(int idMedico, IEnumerable<HorarioDisponibilidadMedico> horarios)
        {
            List<HorarioDisponibilidadMedico> lista = horarios == null
                ? new List<HorarioDisponibilidadMedico>()
                : horarios.Where(horario => horario != null)
                    .ToList();

            EliminarPorMedico(idMedico);

            foreach (HorarioDisponibilidadMedico horario in lista)
            {
                horario.IdMedico = idMedico;
                Agregar(horario);
            }

            return true;
        }

        public void Modificar(HorarioDisponibilidadMedico horarioDisponibilidadMedico)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "UPDATE HorariosDisponiblidadMedicos"
                    + " SET IdMedico = @idMedico, DiaSemana = @diaSemana, HoraDesde = @horaDesde, HoraHasta = @horaHasta, Activo = @activo"
                    + " WHERE IdHorarioDisponiblidadMedico = @idHorarioDisponiblidadMedico");
                accesoDatos.setearParametro("@idMedico", horarioDisponibilidadMedico.IdMedico);
                accesoDatos.setearParametro("@diaSemana", horarioDisponibilidadMedico.DiaSemana);
                accesoDatos.setearParametro("@horaDesde", horarioDisponibilidadMedico.HoraDesde);
                accesoDatos.setearParametro("@horaHasta", horarioDisponibilidadMedico.HoraHasta);
                accesoDatos.setearParametro("@activo", horarioDisponibilidadMedico.Activo);
                accesoDatos.setearParametro("@idHorarioDisponiblidadMedico", horarioDisponibilidadMedico.IdHorarioDisponibilidadMedico);
                accesoDatos.ejecutarAccion();
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public HorarioDisponibilidadMedico MapearFilaAEntidad(SqlDataReader fila)
        {
            HorarioDisponibilidadMedico horario = new HorarioDisponibilidadMedico();
            horario.IdHorarioDisponibilidadMedico = Convert.ToInt32(fila["IdHorarioDisponiblidadMedico"]);
            horario.IdMedico = Convert.ToInt32(fila["IdMedico"]);
            horario.DiaSemana = (DiaSemanaEnum)Convert.ToInt32(fila["DiaSemana"]);
            horario.HoraDesde = TimeSpan.Parse(fila["HoraDesde"].ToString());
            horario.HoraHasta = TimeSpan.Parse(fila["HoraHasta"].ToString());
            horario.Activo = Convert.ToBoolean(fila["Activo"]);
            return horario;
        }
    }
}
