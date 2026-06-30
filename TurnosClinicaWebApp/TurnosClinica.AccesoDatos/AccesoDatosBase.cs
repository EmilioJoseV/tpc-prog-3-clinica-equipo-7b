using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace TurnosClinica.AccesoDatos
{
    public class AccesoDatosBase
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

        public AccesoDatosBase()
        {
            conexion = new SqlConnection(ConfigurationManager.AppSettings["cadenaConexion"]);
            comando = new SqlCommand();
        }

        public AccesoDatosBase(SqlConnection conexionCompartida, SqlTransaction transaccionCompartida)
        {
            conexion = conexionCompartida;
            transaccion = transaccionCompartida;
            esContextoExterno = true;
            comando = new SqlCommand();
        }

        public AccesoDatosBase CrearContextoCompartido()
        {
            return new AccesoDatosBase(conexion, transaccion);
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            if (nombreParam.ToLower() == "@imagen" && (valorParam == null || valorParam == DBNull.Value))
            {
                SqlParameter param = new SqlParameter(nombreParam, System.Data.SqlDbType.VarBinary);
                param.Value = DBNull.Value;
                comando.Parameters.Add(param);
            }
            else
            {
                comando.Parameters.AddWithValue(nombreParam, valorParam ?? DBNull.Value);
            }
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
