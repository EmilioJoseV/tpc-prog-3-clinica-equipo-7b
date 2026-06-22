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

        public HorarioDisponibilidadMedicoDatos(AccesoDatos accesoDatosCompartido)
        {
            accesoDatos = accesoDatosCompartido;
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
            return Listar(idMedico, null, null, null);
        }

        public void Agregar(HorarioDisponibilidadMedico horarioDisponibilidadMedico)
        {
            Agregar(accesoDatos, horarioDisponibilidadMedico);
        }

        public void Agregar(AccesoDatos datosCompartidos, HorarioDisponibilidadMedico horarioDisponibilidadMedico)
        {
            try
            {
                datosCompartidos.setearConsulta(
                    "INSERT INTO HorariosDisponiblidadMedicos (IdMedico, DiaSemana, HoraDesde, HoraHasta)"
                    + " VALUES (@idMedico, @diaSemana, @horaDesde, @horaHasta)");
                datosCompartidos.setearParametro("@idMedico", horarioDisponibilidadMedico.IdMedico);
                datosCompartidos.setearParametro("@diaSemana", horarioDisponibilidadMedico.DiaSemana);
                datosCompartidos.setearParametro("@horaDesde", horarioDisponibilidadMedico.HoraDesde);
                datosCompartidos.setearParametro("@horaHasta", horarioDisponibilidadMedico.HoraHasta);
                datosCompartidos.ejecutarAccion();
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (ReferenceEquals(datosCompartidos, accesoDatos))
                {
                    accesoDatos.cerrarConexion();
                }
            }
        }

        public bool EliminarPorMedico(int idMedico)
        {
            return EliminarPorMedico(accesoDatos, idMedico);
        }

        public bool EliminarPorMedico(AccesoDatos datosCompartidos, int idMedico)
        {
            try
            {
                datosCompartidos.setearConsulta(
                    "DELETE FROM HorariosDisponiblidadMedicos"
                    + " WHERE IdMedico = @idMedico");
                datosCompartidos.setearParametro("@idMedico", idMedico);
                datosCompartidos.ejecutarAccion();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (ReferenceEquals(datosCompartidos, accesoDatos))
                {
                    accesoDatos.cerrarConexion();
                }
            }
        }

        public bool AgregarActualizarPorMedico(int idMedico, IEnumerable<HorarioDisponibilidadMedico> horarios)
        {
            return ReemplazarPorMedico(accesoDatos, idMedico, horarios);
        }

        public bool ReemplazarPorMedico(AccesoDatos datosCompartidos, int idMedico, IEnumerable<HorarioDisponibilidadMedico> horarios)
        {
            List<HorarioDisponibilidadMedico> lista = horarios == null
                ? new List<HorarioDisponibilidadMedico>()
                : horarios.Where(horario => horario != null)
                    .ToList();

            EliminarPorMedico(datosCompartidos, idMedico);

            foreach (HorarioDisponibilidadMedico horario in lista)
            {
                horario.IdMedico = idMedico;
                Agregar(datosCompartidos, horario);
            }

            return true;
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
            return horario;
        }
    }
}
