using System;
using System.Web;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Web
{
    public partial class MasterLayout : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rutaActual = VirtualPathUtility.ToAppRelative(Request.AppRelativeCurrentExecutionFilePath);
            Usuario usuario = Session["UsuarioActual"] as Usuario;

            if (!AutorizacionRutasService.UsuarioPuedeAccederRuta(usuario, rutaActual))
            {
                RedirigirSinPermiso(usuario);
                return;
            }

            ConfigurarNavegacion(usuario);
            ConfigurarContenedor(rutaActual);

            string errorAutorizacion = Session["ErrorAutorizacion"] as string;
            if (!string.IsNullOrWhiteSpace(errorAutorizacion))
            {
                MostrarError(errorAutorizacion);
                Session.Remove("ErrorAutorizacion");
            }
        }

        private void ConfigurarContenedor(string rutaActual)
        {
            if (string.Equals(rutaActual, "~/Inicio.aspx", StringComparison.OrdinalIgnoreCase))
            {
                ContenedorPrincipal.Attributes["class"] = "container-fluid p-0";
                return;
            }

            ContenedorPrincipal.Attributes["class"] = "container py-4";
        }

        protected void LnkSalir_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Ingresar.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void ConfigurarNavegacion(Usuario usuario)
        {
            bool usuarioAutenticado = AutorizacionRutasService.EstaAutenticado(usuario);
            bool puedeIrAlPanel = usuarioAutenticado
                && AutorizacionRutasService.TieneRol(usuario, RolEnum.Administrador, RolEnum.Recepcionista, RolEnum.Medico)
                && !AutorizacionRutasService.EstaCambioClavePendiente(usuario);

            LnkPanelPrincipal.Visible = puedeIrAlPanel;
            ItemUsuario.Visible = usuarioAutenticado;
            LnkIngresar.Visible = !usuarioAutenticado;
            imgAvatar.Visible = usuarioAutenticado;
            lblAvatarPlaceholder.Visible = false;

            if (usuarioAutenticado)
            {
                CargarDatosUsuario(usuario);
                CargarAvatar(usuario);
            }
        }

        private void CargarDatosUsuario(Usuario usuario)
        {
            LblNombreUsuario.Text = ObtenerNombreVisible(usuario);
            LblRolUsuario.Text = ObtenerRolVisible(usuario);
        }

        private void CargarAvatar(Usuario usuario)
        {
            if (usuario != null && usuario.Imagen != null && usuario.Imagen.Length > 0)
            {
                imgAvatar.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(usuario.Imagen);
                imgAvatar.Visible = true;
                lblAvatarPlaceholder.Visible = false;
                return;
            }

            imgAvatar.Visible = false;
            lblAvatarPlaceholder.Text = ObtenerInicial(usuario);
            lblAvatarPlaceholder.Visible = true;
        }

        private string ObtenerInicial(Usuario usuario)
        {
            if (usuario != null
                && usuario.Persona != null
                && !string.IsNullOrWhiteSpace(usuario.Persona.Nombre))
            {
                return usuario.Persona.Nombre.Trim().Substring(0, 1).ToUpper();
            }

            return "U";
        }

        private string ObtenerNombreVisible(Usuario usuario)
        {
            if (usuario != null && usuario.Persona != null)
            {
                string nombre = usuario.Persona.Nombre;
                string apellido = usuario.Persona.Apellido;
                string nombreCompleto = (nombre + " " + apellido).Trim();

                if (!string.IsNullOrWhiteSpace(nombreCompleto))
                {
                    return nombreCompleto;
                }
            }

            if (usuario != null && !string.IsNullOrWhiteSpace(usuario.NombreUsuario))
            {
                return usuario.NombreUsuario;
            }

            return "Usuario";
        }

        private string ObtenerRolVisible(Usuario usuario)
        {
            if (usuario != null
                && usuario.Rol != null
                && !string.IsNullOrWhiteSpace(usuario.Rol.Nombre))
            {
                return usuario.Rol.Nombre;
            }

            return "Sin rol";
        }

        private void RedirigirSinPermiso(Usuario usuario)
        {
            string destino = "~/Ingresar.aspx";

            if (AutorizacionRutasService.EstaAutenticado(usuario))
            {
                Session["ErrorAutorizacion"] = "No tiene permisos para acceder a esta pantalla.";

                if (AutorizacionRutasService.EstaCambioClavePendiente(usuario))
                {
                    Session["ErrorAutorizacion"] = "Debe cambiar su contrasena para continuar.";
                    destino = "~/CambiarContrasenaPendiente.aspx";
                }
                else if (AutorizacionRutasService.TieneRol(usuario, RolEnum.Administrador, RolEnum.Recepcionista, RolEnum.Medico))
                {
                    destino = "~/PanelPrincipal.aspx";
                }
            }

            Response.Redirect(destino, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        public void MostrarError(string mensaje)
        {
            string mensajeVisible = string.IsNullOrWhiteSpace(mensaje)
                ? "Ocurrio un error inesperado."
                : mensaje;
            LblMensajeError.Text = Server.HtmlEncode(mensajeVisible);
            PnlMensajeError.Visible = true;
        }

    }
}
