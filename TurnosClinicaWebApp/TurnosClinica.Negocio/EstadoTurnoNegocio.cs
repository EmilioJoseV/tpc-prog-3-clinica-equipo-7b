using System;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class EstadoTurnoNegocio
    {
        private readonly EstadoTurnoDatos estadoTurnoDatos;

        public EstadoTurnoNegocio()
        {
            estadoTurnoDatos = new EstadoTurnoDatos(new AccesoDatosBase());
        }

        public EstadoTurnoNegocio(AccesoDatosBase accesoDatos)
        {
            estadoTurnoDatos = new EstadoTurnoDatos(accesoDatos);
        }

        public EstadoTurno ObtenerPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new Exception("El nombre del estado del turno es obligatorio.");
            }

            return estadoTurnoDatos.ObtenerPorNombre(nombre);
        }
    }
}
