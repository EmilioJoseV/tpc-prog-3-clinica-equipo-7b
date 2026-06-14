using System;
using System.Web.UI;

namespace TurnosClinica.Web
{
    public partial class FormularioEspecialidad : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("ListaEspecialidades.aspx");
        }

    }
}
