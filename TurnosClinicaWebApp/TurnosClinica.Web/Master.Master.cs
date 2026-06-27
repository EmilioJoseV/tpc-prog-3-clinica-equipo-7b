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
            bool usuarioAutenticado = Session["UsuarioActual"] != null;
            LnkPanelPrincipal.Visible = usuarioAutenticado;
            LnkIngresar.Visible = !usuarioAutenticado;
            LnkSalir.Visible = usuarioAutenticado;
            imgAvatar.Visible = usuarioAutenticado;

            if (usuarioAutenticado)
            {
                Usuario user = (Usuario)Session["UsuarioActual"];
                bool cambioClavePendiente = user.EstadoUsuario != null
                    && string.Equals(
                        user.EstadoUsuario.Nombre,
                        EstadoUsuarioEnum.CambioClavePendiente.ToString(),
                        StringComparison.OrdinalIgnoreCase);

                if (cambioClavePendiente
                    && !EsPaginaPermitidaCambioClavePendiente())
                {
                    Response.Redirect("~/CambiarContrasenaPendiente.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                LnkPanelPrincipal.Visible = !cambioClavePendiente;
                
                // Mostrar la imagen asumiendo el nombre fijo que configuramos al guardar.
                // Tal como indicaste, la validación de si es null se hará en otro commit.
                imgAvatar.ImageUrl = "~/Images/Perfiles/perfil-" + user.IdUsuario + ".jpg";
            }
        }

        protected void LnkSalir_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Ingresar.aspx", false);
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

        private bool EsPaginaPermitidaCambioClavePendiente()
        {
            string ruta = VirtualPathUtility.ToAppRelative(Request.AppRelativeCurrentExecutionFilePath);

            return string.Equals(ruta, "~/CambiarContrasenaPendiente.aspx", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ruta, "~/Ingresar.aspx", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ruta, "~/RecuperarContrasena.aspx", StringComparison.OrdinalIgnoreCase);
        }
    }
}
