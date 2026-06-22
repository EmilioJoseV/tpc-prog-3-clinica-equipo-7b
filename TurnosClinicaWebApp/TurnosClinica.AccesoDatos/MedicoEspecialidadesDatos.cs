using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class MedicoEspecialidadesDatos : IMapeable<Especialidad>
    {
        private readonly AccesoDatosBase accesoDatos;

        public MedicoEspecialidadesDatos()
        {
            accesoDatos = new AccesoDatosBase();
        }

        public MedicoEspecialidadesDatos(AccesoDatosBase accesoDatosCompartido)
        {
            accesoDatos = accesoDatosCompartido;
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
            Agregar(accesoDatos, idMedico, idEspecialidad);
        }

        public void Agregar(AccesoDatosBase datosCompartidos, int idMedico, int idEspecialidad)
        {
            try
            {
                datosCompartidos.setearConsulta(
                    "INSERT INTO MedicosEspecialidades (IdMedico, IdEspecialidad)"
                    + " VALUES (@idMedico, @idEspecialidad)");
                datosCompartidos.setearParametro("@idMedico", idMedico);
                datosCompartidos.setearParametro("@idEspecialidad", idEspecialidad);
                datosCompartidos.ejecutarAccion();
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

        public bool EliminarPorMedico(AccesoDatosBase datosCompartidos, int idMedico)
        {
            try
            {
                datosCompartidos.setearConsulta(
                    "DELETE FROM MedicosEspecialidades"
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
        public bool AgregarActualizarPorMedico(int idMedico, List<Especialidad> especialidadesRequeridas)
        {
            List<Especialidad> especialidadesFiltradas = especialidadesRequeridas == null
                ? new List<Especialidad>()
                : especialidadesRequeridas.Where(especialidad => especialidad != null && especialidad.IdEspecialidad > 0)
                    .GroupBy(especialidad => especialidad.IdEspecialidad)
                    .Select(grupo => grupo.First())
                    .ToList();

            EliminarPorMedico(accesoDatos, idMedico);

            foreach (Especialidad especialidad in especialidadesFiltradas)
            {
                Agregar(accesoDatos, idMedico, especialidad.IdEspecialidad);
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
