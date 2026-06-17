using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class ListaPacientes : Page
    {
        public bool FiltroAvanzado { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            FiltroAvanzado = chkAvanzado.Checked;

            if (FiltroAvanzado && ddlCriterio.Items.Count == 0)
            {
                CargarCriterios();
            }

            if (!IsPostBack)
            {
                CargarLista();
            }
        }

        private void CargarLista()
        {
            try
            {
                PacienteNegocio negocio = new PacienteNegocio();
                dgvPacientes.DataSource = negocio.Listar();
                dgvPacientes.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void filtro_TextChanged(object sender, EventArgs e)
        {
            try
            {
                PacienteNegocio negocio = new PacienteNegocio();
                dgvPacientes.DataSource = negocio.ListarFiltroRapido(txtFiltro.Text);
                dgvPacientes.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void chkAvanzado_CheckedChanged(object sender, EventArgs e)
        {
            FiltroAvanzado = chkAvanzado.Checked;
            txtFiltro.Enabled = !FiltroAvanzado;

            if (FiltroAvanzado)
            {
                CargarCriterios();
            }
        }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCriterios();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlCampo.SelectedItem == null || ddlCriterio.Items.Count == 0)
                {
                    CargarCriterios();
                }

                if (ddlCampo.SelectedItem == null || ddlCriterio.SelectedItem == null)
                {
                    CargarLista();
                    return;
                }

                PacienteNegocio negocio = new PacienteNegocio();
                dgvPacientes.DataSource = negocio.ListarConFiltros(
                    ddlCampo.SelectedItem.ToString(),
                    ddlCriterio.SelectedItem.ToString(),
                    txtFiltroAvanzado.Text,
                    ObtenerEstadoSeleccionado());
                dgvPacientes.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = string.Empty;
            txtFiltroAvanzado.Text = string.Empty;
            chkAvanzado.Checked = false;
            FiltroAvanzado = false;
            txtFiltro.Enabled = true;
            ddlCriterio.Items.Clear();
            CargarLista();
        }

        protected void dgvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Editar" && e.CommandName != "Desactivar")
            {
                return;
            }

            try
            {
                int idPaciente = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "Editar")
                {
                    Response.Redirect("FormularioPaciente.aspx?id=" + idPaciente, false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                PacienteNegocio negocio = new PacienteNegocio();
                negocio.Desactivar(idPaciente);
                CargarLista();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        private void CargarCriterios()
        {
            ddlCriterio.Items.Clear();

            if (ddlCampo.SelectedItem != null && ddlCampo.SelectedItem.ToString() == "DNI")
            {
                ddlCriterio.Items.Add("Igual a");
                ddlCriterio.Items.Add("Mayor a");
                ddlCriterio.Items.Add("Menor a");
            }
            else
            {
                ddlCriterio.Items.Add("Contiene");
                ddlCriterio.Items.Add("Comienza con");
                ddlCriterio.Items.Add("Termina con");
            }
        }

        private bool? ObtenerEstadoSeleccionado()
        {
            if (ddlEstado.SelectedItem.ToString() == "Activo")
            {
                return true;
            }

            if (ddlEstado.SelectedItem.ToString() == "Inactivo")
            {
                return false;
            }

            return null;
        }
    }
}
