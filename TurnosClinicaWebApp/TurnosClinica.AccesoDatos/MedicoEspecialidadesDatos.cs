using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class MedicoEspecialidadesDatos : IMapeable<Especialidad>
    {
        private readonly AccesoDatos accesoDatos;

        public MedicoEspecialidadesDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<Especialidad> ListarPorMedico(int idMedico)
        {
            List<Especialidad> especialidades = new List<Especialidad>();
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT E.IdEspecialidad, E.Nombre, E.Descripcion, E.Activo"
                    + " FROM MedicosEspecialidades ME"
                    + " INNER JOIN Especialidades E ON E.IdEspecialidad = ME.IdEspecialidad"
                    + " WHERE ME.IdMedico = @idMedico"
                    + " ORDER BY E.Nombre ASC");
                accesoDatos.setearParametro("@idMedico", idMedico);
                accesoDatos.ejecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    especialidades.Add(MapearFilaAEntidad(accesoDatos.Lector));
                }

                return especialidades;
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

        public void Agregar(int idMedico, int idEspecialidad)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "INSERT INTO MedicosEspecialidades (IdMedico, IdEspecialidad)"
                    + " VALUES (@idMedico, @idEspecialidad)");
                accesoDatos.setearParametro("@idMedico", idMedico);
                accesoDatos.setearParametro("@idEspecialidad", idEspecialidad);
                accesoDatos.ejecutarAccion();
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
                    "DELETE FROM MedicosEspecialidades"
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

        public bool ReemplazarPorMedico(int idMedico, List<Especialidad> especialidades)
        {
            List<Especialidad> lista = especialidades == null
                ? new List<Especialidad>()
                : especialidades.Where(especialidad => especialidad != null && especialidad.IdEspecialidad > 0)
                    .GroupBy(especialidad => especialidad.IdEspecialidad)
                    .Select(grupo => grupo.First())
                    .ToList();

            EliminarPorMedico(idMedico);

            foreach (Especialidad especialidad in lista)
            {
                Agregar(idMedico, especialidad.IdEspecialidad);
            }

            return true;
        }

        public Especialidad MapearFilaAEntidad(SqlDataReader fila)
        {
            Especialidad especialidad = new Especialidad();
            especialidad.IdEspecialidad = Convert.ToInt32(fila["IdEspecialidad"]);
            especialidad.Nombre = fila["Nombre"].ToString();
            especialidad.Descripcion = fila["Descripcion"] is DBNull ? string.Empty : fila["Descripcion"].ToString();
            especialidad.Activo = Convert.ToBoolean(fila["Activo"]);
            return especialidad;
        }
    }
}
