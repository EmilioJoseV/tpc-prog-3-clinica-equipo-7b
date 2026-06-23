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
            personaDatos = new PersonaDatos(new AccesoDatosBase());
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
            catch
            {
                throw;
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
            catch
            {
                throw;
            }
        }

        public int Agregar(Persona persona)
        {
            try
            {
                ValidarPersona(persona, true);
                return personaDatos.Agregar(persona);
            }
            catch
            {
                throw;
            }
        }

        public void Modificar(Persona persona)
        {
            try
            {
                ValidarPersona(persona, false);

                if (persona.IdPersona <= 0)
                {
                    throw new Exception("La persona no existe.");
                }

                personaDatos.Modificar(persona);
            }
            catch
            {
                throw;
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
                    throw new Exception("El correo electronico es obligatorio.");
                }

                Persona personaPorDni = personaDatos.ObtenerPorDni(persona.DNI.Trim());
                if (personaPorDni != null && personaPorDni.IdPersona != persona.IdPersona)
                {
                    throw new Exception("Ya existe una persona registrada con ese DNI.");
                }

                Persona personaPorEmail = personaDatos.ObtenerPorEmail(persona.Email.Trim());
                if (personaPorEmail != null && personaPorEmail.IdPersona != persona.IdPersona)
                {
                    throw new Exception("Ya existe una persona registrada con ese correo electronico.");
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
