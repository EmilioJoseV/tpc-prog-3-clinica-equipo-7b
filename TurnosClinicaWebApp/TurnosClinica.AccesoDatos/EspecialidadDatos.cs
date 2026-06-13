using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class EspecialidadDatos : IFiltrable<Especialidad>, IMapeable<Especialidad>
    {
        private readonly AccesoDatos accesoDatos;

        public EspecialidadDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        
        public List<Especialidad> Listar(bool activo)
        {
            List<Especialidad> especialidades = new List<Especialidad>();

            try
            {
                string consulta = "SELECT IdEspecialidad, Nombre, Descripcion, Activo FROM Especialidades";
                if (activo)
                {
                    consulta += " WHERE Activo = 1";
                }

                accesoDatos.setearConsulta(consulta);
                accesoDatos.ejecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    Especialidad aux = new Especialidad();

                    aux.IdEspecialidad = (int)accesoDatos.Lector["IdEspecialidad"];
                    aux.Nombre = (string)accesoDatos.Lector["Nombre"];
                    aux.Activo = (bool)accesoDatos.Lector["Activo"];

                    if (!(accesoDatos.Lector["Descripcion"] is DBNull))
                    {
                        aux.Descripcion = (string)accesoDatos.Lector["Descripcion"];
                    }

                    especialidades.Add(aux);
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

        public Especialidad ObtenerPorId(int idEspecialidad)
        {
            Especialidad especialidad = new Especialidad();
            try { return especialidad; } catch (Exception ex) { throw ex; }
        }

        public bool ExisteNombre(string nombre)
        {
            try { return false; } catch (Exception ex) { throw ex; }
        }

        public int Agregar(Especialidad especialidad)
        {
            try { return 0; } catch (Exception ex) { throw ex; }
        }

        public bool Modificar(Especialidad especialidad)
        {
            try { return false; } catch (Exception ex) { throw ex; }
        }

        public List<Especialidad> ListarConFiltros(string campo, string criterio, string filtro, bool? activo)
        {
            throw new NotImplementedException();
        }

        public Especialidad MapearFilaAEntidad(SqlDataReader fila)
        {
            throw new NotImplementedException();
        }
    }
}
