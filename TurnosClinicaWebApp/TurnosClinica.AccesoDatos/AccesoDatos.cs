using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace TurnosClinica.AccesoDatos
{
    public class AccesoDatos
    {
        private readonly SqlConnection conexion;
        private readonly SqlCommand comando;
        private readonly SqlTransaction transaccion;
        private readonly bool esContextoExterno;
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

        public AccesoDatos(SqlConnection conexionCompartida, SqlTransaction transaccionCompartida)
        {
            conexion = conexionCompartida;
            transaccion = transaccionCompartida;
            esContextoExterno = true;
            comando = new SqlCommand();
        }

        public void setearConsulta(string consulta)
        {
            comando.Parameters.Clear();
            comando.CommandType = CommandType.Text;
            comando.CommandText = consulta;
        }

        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            comando.Transaction = transaccion;

            try
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }

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
            comando.Transaction = transaccion;

            try
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }

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
            comando.Transaction = transaccion;

            try
            {
                bool debeCerrar = conexion.State != ConnectionState.Open;
                if (debeCerrar)
                {
                    conexion.Open();
                }

                return int.Parse(comando.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (transaccion == null && !esContextoExterno)
                {
                    conexion.Close();
                }
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
                lector = null;
            }

            if (transaccion != null)
            {
                return;
            }

            if (!esContextoExterno)
            {
                conexion.Close();
            }
        }
    }
}
