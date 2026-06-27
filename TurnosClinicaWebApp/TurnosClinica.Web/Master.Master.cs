using System;
using System.Web;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Web
{
    public partial class MasterLayout : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rutaActual = VirtualPathUtility.ToAppRelative(Request.AppRelativeCurrentExecutionFilePath);
            Usuario usuario = AutorizacionRutasService.ObtenerUsuarioActual(Session);

            if (!AutorizacionRutasService.UsuarioPuedeAccederRuta(usuario, rutaActual))
            {
                RedirigirSinPermiso(usuario);
                return;
            }

            ConfigurarNavegacion(usuario);

            string errorAutorizacion = Session["ErrorAutorizacion"] as string;
            if (!string.IsNullOrWhiteSpace(errorAutorizacion))
            {
                MostrarError(errorAutorizacion);
                Session.Remove("ErrorAutorizacion");
            }
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
                && AutorizacionRutasService.TieneAccesoOperativo(usuario)
                && !AutorizacionRutasService.EstaCambioClavePendiente(usuario);

            LnkPanelPrincipal.Visible = puedeIrAlPanel;
            LnkIngresar.Visible = !usuarioAutenticado;
            LnkSalir.Visible = usuarioAutenticado;
            imgAvatar.Visible = usuarioAutenticado;

            if (usuarioAutenticado)
            {
                imgAvatar.ImageUrl = "~/Images/Perfiles/perfil-" + usuario.IdUsuario + ".jpg";
            }
        }

        private void RedirigirSinPermiso(Usuario usuario)
        {
            string destino = "~/Ingresar.aspx";

            if (AutorizacionRutasService.EstaAutenticado(usuario))
            {
                Session["ErrorAutorizacion"] = "No tiene permisos para acceder a esta pantalla.";
                if (AutorizacionRutasService.TieneAccesoOperativo(usuario))
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
