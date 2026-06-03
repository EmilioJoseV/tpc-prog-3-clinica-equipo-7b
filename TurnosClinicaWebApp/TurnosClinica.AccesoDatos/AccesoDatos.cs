using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnosClinica.AccesoDatos
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=TPC_TURNOS_CLINICA_P3_DB; integrated security=true");
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

        public int ejecutarCreacionRetornandoId()
        {
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                object resultado = comando.ExecuteScalar();
                if (resultado != null && resultado != DBNull.Value)
                    return Convert.ToInt32(resultado);

                return 0;
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
    }
}
