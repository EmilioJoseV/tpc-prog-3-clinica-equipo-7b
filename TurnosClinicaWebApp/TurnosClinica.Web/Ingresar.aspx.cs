using System;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class Ingresar : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Clear();

                Usuario usuario = new UsuarioNegocio().ValidarCredenciales(
                    TxtUsuario.Text,
                    TxtContrasena.Text);

                if (usuario == null)
                {
                    throw new Exception("Los datos ingresados no son validos.");
                }

                Session["UsuarioActual"] = usuario;
                Response.Redirect("PanelPrincipal.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }
    }
}
