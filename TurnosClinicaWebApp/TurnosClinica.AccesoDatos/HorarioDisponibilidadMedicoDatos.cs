using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class HorarioDisponibilidadMedicoDatos : IMapeable<HorarioDisponibilidadMedico>
    {
        private readonly AccesoDatos accesoDatos;

        public HorarioDisponibilidadMedicoDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<HorarioDisponibilidadMedico> Listar(int idMedico, int diaSemana, TimeSpan horaDesde, TimeSpan horaHasta, bool activo)
        {
            List<HorarioDisponibilidadMedico> horarios = new List<HorarioDisponibilidadMedico>();
            try
            {
                return horarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Agregar(HorarioDisponibilidadMedico horarioDisponibilidadMedico)
        {
            try
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Modificar(HorarioDisponibilidadMedico horarioDisponibilidadMedico)
        {
            try
            {
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public HorarioDisponibilidadMedico MapearFilaAEntidad(SqlDataReader fila)
        {
            throw new NotImplementedException();
        }
    }
}
