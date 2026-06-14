using System;
using System.Data.SqlClient;
using System.Configuration;

namespace TurnosClinica.AccesoDatos
{
    public class AccesoDatos
    {
        private readonly SqlConnection conexion;
        private readonly SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDatos()
        {
            conexion = new SqlConnection(ConfigurationManager.AppSettings["cadenaConexion"]);
            comando = new SqlCommand();
        }

        public void setearConsulta(string consulta) 
        {
            comando.Parameters.Clear();
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void ejecutarLectura()
        {
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ejecutarAccion()
        {
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ejecutarAccionScalar()
        {
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                return int.Parse(comando.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
        }

        public void setearParametro(string nombreParam, object valorParam)
        {
            comando.Parameters.AddWithValue(nombreParam, valorParam);
        }

        public void cerrarConexion()
        {
            if (lector != null)
            {
                lector.Close();
            }

            conexion.Close();
        }

        internal int ejecutarCreacionRetornandoId()
        {
            throw new NotImplementedException();
        }
    }
}
