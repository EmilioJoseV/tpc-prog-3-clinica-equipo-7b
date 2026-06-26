using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;


namespace TurnosClinica.Web
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void BtnCrearCuenta_Click(object sender, EventArgs e)
        {
            try
            {
                Persona nuevaPersona = new Persona();
                nuevaPersona.Nombre = TxtNombre.Text;
                nuevaPersona.Apellido = TxtApellido.Text;
                nuevaPersona.DNI = TxtDni.Text;
                nuevaPersona.Email = TxtEmail.Text;

                AccesoService accesoService = new AccesoService();
                accesoService.RegistrarUsuarioWeb(nuevaPersona, TxtContrasena.Text);

              
                Response.Redirect("Ingresar.aspx", false);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
            }
        }

    }
}