using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.AccesoDatos
{
    public class HorarioDisponibilidadMedicoDatos : IMapeable<HorarioDisponibilidadMedico>
    {
        private readonly AccesoDatosBase accesoDatos;

        public HorarioDisponibilidadMedicoDatos(AccesoDatosBase accesoDatos)
        {
            this.accesoDatos = accesoDatos;
        }

        public List<HorarioDisponibilidadMedico> Listar(int? idMedico, DiaSemanaEnum? diaSemana, TimeSpan? horaDesde, TimeSpan? horaHasta)
        {
            List<HorarioDisponibilidadMedico> horarios = new List<HorarioDisponibilidadMedico>();
            try
            {
                string consulta = "SELECT IdHorarioDisponiblidadMedico, IdMedico, DiaSemana, HoraDesde, HoraHasta"
                    + " FROM HorariosDisponiblidadMedicos WHERE 1=1";

                if (idMedico.HasValue)
                {
                    consulta += " AND IdMedico = @idMedico";
                }

                if (diaSemana.HasValue)
                {
                    consulta += " AND DiaSemana = @diaSemana";
                }

                if (horaDesde.HasValue)
                {
                    consulta += " AND HoraDesde >= @horaDesde";
                }

                if (horaHasta.HasValue)
                {
                    consulta += " AND HoraHasta <= @horaHasta";
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
                accesoDatos.ejecutarLectura();
                while (accesoDatos.Lector.Read())
                {
                    horarios.Add(MapearFilaAEntidad(accesoDatos.Lector));
                }

                return horarios;
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

        public List<HorarioDisponibilidadMedico> ObtenerHorariosAsociadosAMedico(int idMedico)
        {
            return Listar(idMedico, null, null, null);
        }

        private void Agregar(HorarioDisponibilidadMedico horarioDisponibilidadMedico)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "INSERT INTO HorariosDisponiblidadMedicos (IdMedico, DiaSemana, HoraDesde, HoraHasta)"
                    + " VALUES (@idMedico, @diaSemana, @horaDesde, @horaHasta)");
                accesoDatos.setearParametro("@idMedico", horarioDisponibilidadMedico.IdMedico);
                accesoDatos.setearParametro("@diaSemana", horarioDisponibilidadMedico.DiaSemana);
                accesoDatos.setearParametro("@horaDesde", horarioDisponibilidadMedico.HoraDesde);
                accesoDatos.setearParametro("@horaHasta", horarioDisponibilidadMedico.HoraHasta);
                accesoDatos.ejecutarAccion();
                return;
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

        private void EliminarPorMedico(int idMedico)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "DELETE FROM HorariosDisponiblidadMedicos"
                    + " WHERE IdMedico = @idMedico");
                accesoDatos.setearParametro("@idMedico", idMedico);
                accesoDatos.ejecutarAccion();
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

        public void ReemplazarPorMedico(int idMedico, IEnumerable<HorarioDisponibilidadMedico> horarios)
        {
            List<HorarioDisponibilidadMedico> lista = new List<HorarioDisponibilidadMedico>();
            if (horarios != null)
            {
                foreach (HorarioDisponibilidadMedico horario in horarios)
                {
                    if (horario != null)
                    {
                        lista.Add(horario);
                    }
                }
            }

            EliminarPorMedico(idMedico);

            foreach (HorarioDisponibilidadMedico horario in lista)
            {
                horario.IdMedico = idMedico;
                Agregar(horario);
            }

        }

        public void Modificar(HorarioDisponibilidadMedico horarioDisponibilidadMedico)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "UPDATE HorariosDisponiblidadMedicos"
                    + " SET IdMedico = @idMedico, DiaSemana = @diaSemana, HoraDesde = @horaDesde, HoraHasta = @horaHasta"
                    + " WHERE IdHorarioDisponiblidadMedico = @idHorarioDisponiblidadMedico");
                accesoDatos.setearParametro("@idMedico", horarioDisponibilidadMedico.IdMedico);
                accesoDatos.setearParametro("@diaSemana", horarioDisponibilidadMedico.DiaSemana);
                accesoDatos.setearParametro("@horaDesde", horarioDisponibilidadMedico.HoraDesde);
                accesoDatos.setearParametro("@horaHasta", horarioDisponibilidadMedico.HoraHasta);
                accesoDatos.setearParametro("@idHorarioDisponiblidadMedico", horarioDisponibilidadMedico.IdHorarioDisponibilidadMedico);
                accesoDatos.ejecutarAccion();
                return;
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

        public HorarioDisponibilidadMedico MapearFilaAEntidad(SqlDataReader fila)
        {
            HorarioDisponibilidadMedico horario = new HorarioDisponibilidadMedico();
            horario.IdHorarioDisponibilidadMedico = Convert.ToInt32(fila["IdHorarioDisponiblidadMedico"]);
            horario.IdMedico = Convert.ToInt32(fila["IdMedico"]);
            horario.DiaSemana = (DiaSemanaEnum)Convert.ToInt32(fila["DiaSemana"]);
            horario.HoraDesde = TimeSpan.Parse(fila["HoraDesde"].ToString());
            horario.HoraHasta = TimeSpan.Parse(fila["HoraHasta"].ToString());
            return horario;
        }
    }
}
