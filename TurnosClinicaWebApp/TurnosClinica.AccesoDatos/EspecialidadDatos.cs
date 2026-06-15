using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class EspecialidadDatos : IFiltrable<Especialidad>, IMapeable<Especialidad>
    {
        private readonly AccesoDatos accesoDatos;
        private readonly MedicoEspecialidadesDatos medicoEspecialidadesDatos;

        public EspecialidadDatos()
        {
            accesoDatos = new AccesoDatos();
            medicoEspecialidadesDatos = new MedicoEspecialidadesDatos();
        }

        public List<Especialidad> Listar(bool? activo)
        {
            List<Especialidad> especialidades = new List<Especialidad>();

            try
            {
                string consulta = "SELECT IdEspecialidad, Nombre, Descripcion, Activo"
                    + " FROM Especialidades";

                if (activo.HasValue)
                {
                    consulta += " WHERE Activo = 1";
                }

                consulta += " ORDER BY Nombre ASC";

                accesoDatos.setearConsulta(consulta);
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

        public List<Especialidad> ListarPorMedico(int idMedico, bool? activo)
        {
            return medicoEspecialidadesDatos.ListarPorMedico(idMedico);
        }

        public Especialidad ObtenerPorId(int idEspecialidad)
        {
            Especialidad especialidad = new Especialidad();
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT IdEspecialidad, Nombre, Descripcion, Activo"
                    + " FROM Especialidades"
                    + " WHERE IdEspecialidad = @idEspecialidad");
                accesoDatos.setearParametro("@idEspecialidad", idEspecialidad);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    especialidad = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return especialidad;
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

        public bool ExisteNombre(string nombre, int excluirId = 0)
        {
            try
            {
                // Cuento si hayotra especialidad con este nombre,pero excluyendo el id actual
                accesoDatos.setearConsulta("SELECT COUNT(*) FROM Especialidades WHERE Nombre = @Nombre AND IdEspecialidad != @Id");
                accesoDatos.setearParametro("@Nombre", nombre);
                accesoDatos.setearParametro("@Id", excluirId);

                int cantidad = accesoDatos.ejecutarAccionScalar();
                return cantidad > 0;
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


        public int Agregar(Especialidad especialidad)
        {
            try
            {
              
                string consulta = "INSERT INTO Especialidades (Nombre, Descripcion, Activo) OUTPUT INSERTED.IdEspecialidad VALUES (@Nombre, @Descripcion, 1)";
                accesoDatos.setearConsulta(consulta);

                
                accesoDatos.setearParametro("@Nombre", especialidad.Nombre);

                
                if (string.IsNullOrWhiteSpace(especialidad.Descripcion))
                {
                    accesoDatos.setearParametro("@Descripcion", DBNull.Value);
                }
                else
                {
                    accesoDatos.setearParametro("@Descripcion", especialidad.Descripcion);
                }

                
                return accesoDatos.ejecutarCreacionRetornandoId();
            }
            catch (Exception ex)
            {
                throw ex;
            }
             
        }


        public void Modificar(Especialidad especialidad)
        {
            try
            {
                accesoDatos.setearConsulta("UPDATE Especialidades SET Nombre = @Nombre, Descripcion = @Descripcion WHERE IdEspecialidad = @Id");
                accesoDatos.setearParametro("@Nombre", especialidad.Nombre); 
                accesoDatos.setearParametro("@Descripcion", string.IsNullOrWhiteSpace(especialidad.Descripcion) ? (object)DBNull.Value : especialidad.Descripcion);
                accesoDatos.setearParametro("@Id", especialidad.IdEspecialidad);

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


        public List<Especialidad> ListarConFiltros(string campo, string criterio, string filtro, bool? activo)
        {
            throw new NotImplementedException();
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
