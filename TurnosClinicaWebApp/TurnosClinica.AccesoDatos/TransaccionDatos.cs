using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TurnosClinica.AccesoDatos
{
    public class TransaccionDatos : IDisposable
    {
        private readonly SqlConnection conexion;
        private SqlTransaction transaccion;
        public AccesoDatos AccesoDatos { get; private set; }

        public TransaccionDatos()
        {
            conexion = new SqlConnection(ConfigurationManager.AppSettings["cadenaConexion"]);
            AccesoDatos = new AccesoDatos(conexion, transaccion);
        }

        public void IniciarTransaccion()
        {
            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
            }

            transaccion = conexion.BeginTransaction();
            AccesoDatos = new AccesoDatos(conexion, transaccion);
        }

        public void Confirmar()
        {
            if (transaccion != null)
            {
                transaccion.Commit();
                LiberarRecursos();
            }
        }

        public void Cancelar()
        {
            if (transaccion != null)
            {
                transaccion.Rollback();
                LiberarRecursos();
            }
        }

        public void Dispose()
        {
            if (transaccion != null)
            {
                transaccion.Dispose();
                transaccion = null;
            }

            if (conexion != null)
            {
                conexion.Dispose();
            }
        }

        private void LiberarRecursos()
        {
            transaccion.Dispose();
            transaccion = null;
            AccesoDatos = new AccesoDatos(conexion, transaccion);
        }
    }
}
