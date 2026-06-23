using System;
using System.Web.UI;

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
    }
}
