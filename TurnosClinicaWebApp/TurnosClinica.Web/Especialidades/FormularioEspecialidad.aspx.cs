using System;
using System.Web.UI;

namespace TurnosClinica.Web
{
    public partial class FormularioEspecialidad : Page
    {

        public bool ConfirmaEliminacion { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmaEliminacion = false;
            try
            {
                string id = Request.QueryString["id"];

                if (id != null && !IsPostBack)
                {
                    TurnosClinica.Negocio.EspecialidadNegocio negocio = new TurnosClinica.Negocio.EspecialidadNegocio();
                    TurnosClinica.Dominio.Entidades.Especialidad seleccionada = negocio.ObtenerPorId(int.Parse(id));

                    txtNombre.Text = seleccionada.Nombre;
                    txtDescripcion.Text = seleccionada.Descripcion;
                }


                // Si el id es nulo,quiere decir que estoy agregando entonces Oculto el botón eliminar.
                if (id == null)
                {
                   
                    UpdatePanel1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("../Error.aspx", false);
            }
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                TurnosClinica.Dominio.Entidades.Especialidad especialidad = new TurnosClinica.Dominio.Entidades.Especialidad();
                especialidad.Nombre = txtNombre.Text;
                especialidad.Descripcion = txtDescripcion.Text;

                TurnosClinica.Negocio.EspecialidadNegocio negocio = new TurnosClinica.Negocio.EspecialidadNegocio();

                if (Request.QueryString["id"] != null)
                {
                    especialidad.IdEspecialidad = int.Parse(Request.QueryString["id"]);
                    negocio.Modificar(especialidad);
                }
                else
                {
                    negocio.Agregar(especialidad);
                }

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

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            ConfirmaEliminacion = true;
        }

        protected void btnConfirmaEliminar_Click(object sender, EventArgs e)
        {


            try
            {
                if (chkConfirmaEliminacion.Checked)
                {

                    int id = int.Parse(Request.QueryString["id"]);
                    TurnosClinica.Negocio.EspecialidadNegocio negocio = new TurnosClinica.Negocio.EspecialidadNegocio();
                    negocio.Eliminar(id);
                    Response.Redirect("ListaEspecialidades.aspx", false);
                }
            }
            catch (Exception ex)
            {

                Session.Add("error", ex.Message);
                Response.Redirect("../Error.aspx", false);
            }
        } 
    }  
}
