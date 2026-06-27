using System;
using System.Web.UI;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class RecuperarContrasena : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                bool recuperacionIniciada = new AccesoService().IniciarRecuperacionContrasenaPorEmail(TxtEmail.Text);
                PnlResultado.Visible = true;
                LblResultado.Text = "Si el email ingresado corresponde a una cuenta valida, se enviara una clave temporal";
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }
    }
}
