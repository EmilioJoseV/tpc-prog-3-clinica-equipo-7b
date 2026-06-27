using System;
using System.Web;
using System.Web.SessionState;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Web
{
    public static class AutorizacionRutasService
    {
        private static readonly string[] RutasPublicas =
        {
            "~/Inicio.aspx",
            "~/Default.aspx",
            "~/Error.aspx",
            "~/Ingresar.aspx",
            "~/Registro.aspx",
            "~/RecuperarContrasena.aspx"
        };

        private static readonly string[] RutasCambioClave =
        {
            "~/CambiarContrasenaPendiente.aspx",
            "~/Ingresar.aspx",
            "~/RecuperarContrasena.aspx"
        };

        private static readonly string[] RutasOperativasGenerales =
        {
            "~/PanelPrincipal.aspx",
            "~/Perfil/MiPerfil.aspx",
            "~/CambiarContrasenaPendiente.aspx"
        };

        private static readonly string[] RutasRecepcion =
        {
            "~/Pacientes/FormularioPaciente.aspx",
            "~/Pacientes/ListaPacientes.aspx",
            "~/Medicos/FormularioMedico.aspx",
            "~/Medicos/ListaMedicos.aspx",
            "~/Turnos/FormularioTurno.aspx"
        };

        private static readonly string[] RutasAdministrador =
        {
            "~/Especialidades/FormularioEspecialidad.aspx",
            "~/Especialidades/ListaEspecialidades.aspx",
            "~/Usuarios/FormularioUsuario.aspx",
            "~/Usuarios/ListaUsuarios.aspx",
            "~/Turnos/ConfiguracionTurnos.aspx"
        };

        public static Usuario ObtenerUsuarioActual(HttpSessionState session)
        {
            return session["UsuarioActual"] as Usuario;
        }

        public static bool EstaAutenticado(Usuario usuario)
        {
            return usuario != null && usuario.IdUsuario > 0;
        }

        public static bool TieneRol(Usuario usuario, RolEnum rol)
        {
            return usuario != null
                && usuario.Rol != null
                && string.Equals(usuario.Rol.Nombre, rol.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public static bool EsAdministrador(Usuario usuario)
        {
            return TieneRol(usuario, RolEnum.Administrador);
        }

        public static bool EsRecepcionista(Usuario usuario)
        {
            return TieneRol(usuario, RolEnum.Recepcionista);
        }

        public static bool EsMedico(Usuario usuario)
        {
            return TieneRol(usuario, RolEnum.Medico);
        }

        public static bool TieneAlgunRol(Usuario usuario, params RolEnum[] roles)
        {
            foreach (RolEnum rol in roles)
            {
                if (TieneRol(usuario, rol))
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

        public static bool TieneAccesoOperativo(Usuario usuario)
        {
            return EsAdministrador(usuario) || EsRecepcionista(usuario) || EsMedico(usuario);
        }

        public static bool PuedeGestionarRecepcion(Usuario usuario)
        {
            return EsAdministrador(usuario) || EsRecepcionista(usuario);
        }

        public static bool RutaEsPublica(string ruta)
        {
            return EsUnaDeEstasRutas(ruta, RutasPublicas);
        }

        public static bool RutaPermitidaCambioClave(string ruta)
        {
            return EsUnaDeEstasRutas(ruta, RutasCambioClave);
        }

        public static bool UsuarioPuedeAccederRuta(Usuario usuario, string ruta)
        {
            if (!EstaAutenticado(usuario))
            {
                return RutaEsPublica(ruta);
            }

            if (EstaCambioClavePendiente(usuario))
            {
                return RutaPermitidaCambioClave(ruta);
            }

            if (!TieneAccesoOperativo(usuario))
            {
                return false;
            }

            if (EsUnaDeEstasRutas(ruta, RutasOperativasGenerales))
            {
                return true;
            }

            if (EsUnaDeEstasRutas(ruta, RutasRecepcion))
            {
                return PuedeGestionarRecepcion(usuario);
            }

            if (EsUnaDeEstasRutas(ruta, RutasAdministrador))
            {
                return EsAdministrador(usuario);
            }

            if (EsRuta(ruta, "~/Turnos/MisTurnos.aspx"))
            {
                return EsMedico(usuario);
            }

            if (EsRuta(ruta, "~/Turnos/DetalleTurno.aspx"))
            {
                return TieneAccesoOperativo(usuario);
            }

            if (EsRuta(ruta, "~/Turnos/ListaTurnos.aspx"))
            {
                return PuedeGestionarRecepcion(usuario);
            }

            return true;
        }

        private static bool EsUnaDeEstasRutas(string rutaActual, string[] rutas)
        {
            foreach (string ruta in rutas)
            {
                if (EsRuta(rutaActual, ruta))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool EsRuta(string actual, string esperada)
        {
            return string.Equals(actual, esperada, StringComparison.OrdinalIgnoreCase);
        }
    }
}
