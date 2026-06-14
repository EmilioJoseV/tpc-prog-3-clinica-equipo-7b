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
            try
            {
                TurnosClinica.Dominio.Entidades.Especialidad nuevaEspecialidad = new TurnosClinica.Dominio.Entidades.Especialidad();
                nuevaEspecialidad.Nombre = txtNombre.Text;
                nuevaEspecialidad.Descripcion = txtDescripcion.Text;

                TurnosClinica.Negocio.EspecialidadNegocio negocio = new TurnosClinica.Negocio.EspecialidadNegocio();
                negocio.Agregar(nuevaEspecialidad);

                Response.Redirect("ListaEspecialidades.aspx", false);
            }
            catch (Exception ex)
            {
                
                Session.Add("error", ex.Message);

                
                Response.Redirect("../Error.aspx", false);
            }

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("ListaEspecialidades.aspx");
        }

    }
}
