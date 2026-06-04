using System;
using System.Web.UI;

namespace TurnosClinica.Web
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnAcceder_Click(object sender, EventArgs e)
        {
            Response.Redirect("PanelPrincipal.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
