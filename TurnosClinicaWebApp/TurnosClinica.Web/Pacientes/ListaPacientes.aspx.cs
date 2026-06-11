using System;
using System.Web.UI;

namespace TurnosClinica.Web
{
    public partial class ListaPacientes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnAltaPaciente_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioPaciente.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
