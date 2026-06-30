using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class MedicoEspecialidadesDatos : IMapeable<Especialidad>
    {
        private readonly AccesoDatosBase accesoDatos;

        public MedicoEspecialidadesDatos(AccesoDatosBase accesoDatos)
        {
            this.accesoDatos = accesoDatos;
        }

        public List<Especialidad> ObtenerEspecialidadesAsociadasAMedico(int idMedico)
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
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        private void Agregar(int idMedico, int idEspecialidad)
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
                    "DELETE FROM MedicosEspecialidades"
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

        public void ReemplazarPorMedico(int idMedico, List<Especialidad> especialidadesRequeridas)
        {
            List<Especialidad> especialidadesFiltradas = new List<Especialidad>();
            if (especialidadesRequeridas != null)
            {
                foreach (Especialidad especialidad in especialidadesRequeridas)
                {
                    if (especialidad != null
                        && especialidad.IdEspecialidad > 0
                        && !ExisteEspecialidad(especialidadesFiltradas, especialidad.IdEspecialidad))
                    {
                        especialidadesFiltradas.Add(especialidad);
                    }
                }
            }

            EliminarPorMedico(idMedico);

            foreach (Especialidad especialidad in especialidadesFiltradas)
            {
                Agregar(idMedico, especialidad.IdEspecialidad);
            }

        }

        private bool ExisteEspecialidad(List<Especialidad> especialidades, int idEspecialidad)
        {
            foreach (Especialidad especialidad in especialidades)
            {
                if (especialidad != null && especialidad.IdEspecialidad == idEspecialidad)
                {
                    return true;
                }
            }

            return false;
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
