using System;
using System.Web.UI;

namespace TurnosClinica.Web
{
    public partial class Ingresar : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PanelPrincipal.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
