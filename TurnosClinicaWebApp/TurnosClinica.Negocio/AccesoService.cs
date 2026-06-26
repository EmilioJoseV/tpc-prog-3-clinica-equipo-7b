using System;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;
using TurnosClinica.AccesoDatos;

namespace TurnosClinica.Negocio
{
    public class AccesoService
    {
        public void RegistrarUsuarioWeb(Persona persona, string password)
        {
            using (ManejadorTransaccionNegocio manejador = new ManejadorTransaccionNegocio())
            {
                try
                {
                    manejador.Iniciar();

                    PersonaNegocio personaNegocio = new PersonaNegocio(manejador.CrearAccesoDatos());
                    UsuarioDatos usuarioDatos = new UsuarioDatos(manejador.CrearAccesoDatos());

                    //  Guardo Persona y obtengo su ID
                    persona.IdPersona = personaNegocio.Agregar(persona);

                    // creo usua
                    Usuario usuario = new Usuario();
                    usuario.Persona = persona;
                    usuario.NombreUsuario = persona.Email;
                    usuario.PasswordHash = new SeguridadService().CalcularHash(password);

                    // Le asigno Rol Paciente y Estado 
                    usuario.Rol = new Rol { Nombre = RolEnum.Paciente.ToString() };
                    usuario.EstadoUsuario = new EstadoUsuario { Nombre = EstadoUsuarioEnum.Activo.ToString() };

                    //lo guardo
                    usuarioDatos.Agregar(usuario);

                    manejador.Confirmar();
                }
                catch
                {
                    manejador.Cancelar();
                    throw;
                }
            }
        }
    }
}
