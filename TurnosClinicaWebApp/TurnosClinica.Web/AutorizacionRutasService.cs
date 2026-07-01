using System;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Web
{
    public static class AutorizacionRutasService
    {
        private static readonly string[] RutasSinValidacion =
        {
            "~/Inicio.aspx",
            "~/Default.aspx",
            "~/Error.aspx",
            "~/Ingresar.aspx",
            "~/RegistroClave.aspx",
            "~/RecuperarContrasena.aspx"
        };

        private static readonly RutaPermitida[] RutasProtegidas =
        {
            new RutaPermitida("~/PanelPrincipal.aspx", RolEnum.Administrador, RolEnum.Recepcionista, RolEnum.Medico),
            new RutaPermitida("~/Perfil/MiPerfil.aspx", RolEnum.Administrador, RolEnum.Recepcionista, RolEnum.Medico),
            new RutaPermitida("~/CambiarContrasenaPendiente.aspx", RolEnum.Administrador, RolEnum.Recepcionista, RolEnum.Medico),
            new RutaPermitida("~/Pacientes/FormularioPaciente.aspx", RolEnum.Administrador, RolEnum.Recepcionista),
            new RutaPermitida("~/Pacientes/ListaPacientes.aspx", RolEnum.Administrador, RolEnum.Recepcionista),
            new RutaPermitida("~/Medicos/FormularioMedico.aspx", RolEnum.Administrador, RolEnum.Recepcionista),
            new RutaPermitida("~/Medicos/ListaMedicos.aspx", RolEnum.Administrador, RolEnum.Recepcionista),
            new RutaPermitida("~/Turnos/FormularioTurno.aspx", RolEnum.Administrador, RolEnum.Recepcionista),
            new RutaPermitida("~/Turnos/ListaTurnos.aspx", RolEnum.Administrador, RolEnum.Recepcionista),
            new RutaPermitida("~/Especialidades/FormularioEspecialidad.aspx", RolEnum.Administrador),
            new RutaPermitida("~/Especialidades/ListaEspecialidades.aspx", RolEnum.Administrador),
            new RutaPermitida("~/Usuarios/FormularioUsuario.aspx", RolEnum.Administrador),
            new RutaPermitida("~/Usuarios/ListaUsuarios.aspx", RolEnum.Administrador),
            new RutaPermitida("~/Turnos/ConfiguracionTurnos.aspx", RolEnum.Administrador),
            new RutaPermitida("~/Turnos/MisTurnos.aspx", RolEnum.Medico),
            new RutaPermitida("~/Turnos/DetalleTurno.aspx", RolEnum.Administrador, RolEnum.Recepcionista, RolEnum.Medico)
        };

        public static bool EstaAutenticado(Usuario usuario)
        {
            return usuario != null && usuario.IdUsuario > 0;
        }

        public static bool TieneRol(Usuario usuario, params RolEnum[] roles)
        {
            if (!EstaAutenticado(usuario) || usuario.Rol == null)
            {
                return false;
            }

            foreach (RolEnum rol in roles)
            {
                if (string.Equals(usuario.Rol.Nombre, rol.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool EstaCambioClavePendiente(Usuario usuario)
        {
            return usuario != null
                && usuario.EstadoUsuario != null
                && string.Equals(
                    usuario.EstadoUsuario.Nombre,
                    EstadoUsuarioEnum.CambioClavePendiente.ToString(),
                    StringComparison.OrdinalIgnoreCase);
        }

        public static bool UsuarioPuedeAccederRuta(Usuario usuario, string ruta)
        {
            if (EsRutaSinValidacion(ruta))
            {
                return true;
            }

            if (!EstaAutenticado(usuario))
            {
                return false;
            }

            if (EstaCambioClavePendiente(usuario))
            {
                return string.Equals(
                    ruta,
                    "~/CambiarContrasenaPendiente.aspx",
                    StringComparison.OrdinalIgnoreCase);
            }

            RutaPermitida rutaPermitida = ObtenerRutaPermitida(ruta);
            if (rutaPermitida == null)
            {
                return false;
            }

            return TieneRol(usuario, rutaPermitida.RolesPermitidos);
        }

        private static bool EsRutaSinValidacion(string rutaActual)
        {
            foreach (string ruta in RutasSinValidacion)
            {
                if (string.Equals(rutaActual, ruta, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static RutaPermitida ObtenerRutaPermitida(string rutaActual)
        {
            foreach (RutaPermitida rutaPermitida in RutasProtegidas)
            {
                if (string.Equals(rutaActual, rutaPermitida.Ruta, StringComparison.OrdinalIgnoreCase))
                {
                    return rutaPermitida;
                }
            }

            return null;
        }

        private class RutaPermitida
        {
            public RutaPermitida(string ruta, params RolEnum[] rolesPermitidos)
            {
                Ruta = ruta;
                RolesPermitidos = rolesPermitidos;
            }

            public string Ruta { get; private set; }
            public RolEnum[] RolesPermitidos { get; private set; }
        }
    }
}
