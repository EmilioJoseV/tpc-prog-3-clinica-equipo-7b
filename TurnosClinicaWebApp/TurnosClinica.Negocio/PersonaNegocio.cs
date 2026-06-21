using System;
using AccesoDatosBase = TurnosClinica.AccesoDatos.AccesoDatos;
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

        public PersonaNegocio(AccesoDatosBase accesoDatosCompartido)
        {
            personaDatos = new PersonaDatos(accesoDatosCompartido);
        }

        public Persona ObtenerPorDni(string dni)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dni))
                {
                    return null;
                }

                return personaDatos.ObtenerPorDni(dni.Trim());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Persona ObtenerPorEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return null;
                }

                return personaDatos.ObtenerPorEmail(email.Trim());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ValidarPersona(Persona persona, bool esAlta)
        {
            try
            {
                if (persona == null)
                {
                    throw new Exception("La persona es obligatoria.");
                }

                if (string.IsNullOrWhiteSpace(persona.DNI))
                {
                    throw new Exception("El DNI es obligatorio.");
                }

                if (string.IsNullOrWhiteSpace(persona.Nombre))
                {
                    throw new Exception("El nombre es obligatorio.");
                }

                if (string.IsNullOrWhiteSpace(persona.Apellido))
                {
                    throw new Exception("El apellido es obligatorio.");
                }

                if (string.IsNullOrWhiteSpace(persona.Email))
                {
                    throw new Exception("El correo electrónico es obligatorio.");
                }

                Persona personaPorDni = personaDatos.ObtenerPorDni(persona.DNI.Trim());
                if (personaPorDni != null && personaPorDni.IdPersona != persona.IdPersona)
                {
                    throw new Exception("Ya existe una persona registrada con ese DNI.");
                }

                Persona personaPorEmail = personaDatos.ObtenerPorEmail(persona.Email.Trim());
                if (personaPorEmail != null && personaPorEmail.IdPersona != persona.IdPersona)
                {
                    throw new Exception("Ya existe una persona registrada con ese correo electrónico.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
