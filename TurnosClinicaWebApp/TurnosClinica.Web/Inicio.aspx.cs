using System;
using System.Web.UI;

namespace TurnosClinica.Web
{
    public partial class Inicio : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnIrLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
