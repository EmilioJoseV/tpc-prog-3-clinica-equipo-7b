using System;
using System.Web.UI;

namespace TurnosClinica.Web
{
    public partial class ErrorPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            {
                if (Session["error"] != null)
                {

                    lblMensajeError.Text = Session["error"].ToString();
                }

            }
        }
    }
}
