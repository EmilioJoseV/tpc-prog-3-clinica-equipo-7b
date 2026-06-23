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

        public TransaccionDatos()
        {
            conexion = new SqlConnection(ConfigurationManager.AppSettings["cadenaConexion"]);
        }

        public void IniciarTransaccion()
        {
            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
            }

            transaccion = conexion.BeginTransaction();
        }

        public AccesoDatosBase CrearAccesoDatos()
        {
            if (transaccion == null)
            {
                throw new InvalidOperationException("La transaccion no fue iniciada.");
            }

            return new AccesoDatosBase(conexion, transaccion);
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
        }
    }
}
