using System;
using TurnosClinica.AccesoDatos;

namespace TurnosClinica.Negocio
{
    public class ManejadorTransaccionNegocio : IDisposable
    {
        private readonly TransaccionDatos transaccionDatos;

        public ManejadorTransaccionNegocio()
        {
            transaccionDatos = new TransaccionDatos();
        }

        public void Iniciar()
        {
            transaccionDatos.IniciarTransaccion();
        }

        public AccesoDatosBase CrearAccesoDatos()
        {
            return transaccionDatos.CrearAccesoDatos();
        }

        public void Confirmar()
        {
            transaccionDatos.Confirmar();
        }

        public void Cancelar()
        {
            transaccionDatos.Cancelar();
        }

        public void Dispose()
        {
            transaccionDatos.Dispose();
        }
    }
}
