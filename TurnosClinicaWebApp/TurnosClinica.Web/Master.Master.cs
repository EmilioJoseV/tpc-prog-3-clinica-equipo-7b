using System;
using System.Web.UI;

namespace TurnosClinica.Web
{
    public partial class MasterLayout : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
