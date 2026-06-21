using System;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class PersonaNegocio
    {
        private readonly PersonaDatos personaDatos;

        public PersonaNegocio()
        {
            personaDatos = new PersonaDatos();
        }

        public Persona ObtenerPorId(int idPersona)
        {
            try
            {
                if (idPersona <= 0)
                    return null;

                return personaDatos.ObtenerPorId(idPersona);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Persona ObtenerPorDni(string dni)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dni))
                    return null;

                return personaDatos.ObtenerPorDni(dni.Trim());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
