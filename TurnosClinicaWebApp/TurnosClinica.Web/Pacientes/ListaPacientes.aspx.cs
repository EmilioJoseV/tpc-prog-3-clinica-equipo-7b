using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class ListaPacientes : Page
    {
        private readonly PacienteNegocio pacienteNegocio = new PacienteNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CargarCriterios();
                    CargarLista();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkAvanzado.Checked)
                {
                    BuscarAvanzado();
                    return;
                }

                CargarLista(txtFiltro.Text);
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
            ddlEstado.SelectedIndex = 0;
            CargarCriterios();
            CargarLista();
        }

        protected void chkAvanzado_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAvanzado.Checked)
            {
                txtFiltro.Text = string.Empty;
                CargarCriterios();
                return;
            }

            txtFiltroAvanzado.Text = string.Empty;
            ddlEstado.SelectedIndex = 0;
            CargarLista();
        }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCriterios();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarAvanzado();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void dgvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Ver" && e.CommandName != "Toggle")
            {
                return;
            }

            try
            {
                string[] argumentos = e.CommandArgument.ToString().Split('|');
                int idPaciente = Convert.ToInt32(argumentos[0]);
                bool activo = argumentos.Length > 1 && Convert.ToBoolean(argumentos[1]);

                if (e.CommandName == "Ver")
                {
                    Response.Redirect("FormularioPaciente.aspx?id=" + idPaciente, false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                if (activo)
                {
                    pacienteNegocio.Desactivar(idPaciente);
                }
                else
                {
                    pacienteNegocio.Activar(idPaciente);
                }

                CargarLista(txtFiltro.Text);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        private void CargarLista(string palabra = null)
        {
            dgvPacientes.DataSource = string.IsNullOrWhiteSpace(palabra)
                ? pacienteNegocio.Listar()
                : pacienteNegocio.ListarFiltroRapido(palabra);
            dgvPacientes.DataBind();
        }

        private void BuscarAvanzado()
        {
            bool? activo = ObtenerActivoSeleccionado();
            dgvPacientes.DataSource = pacienteNegocio.ListarFiltroAvanzado(
                ddlCampo.SelectedValue,
                ddlCriterio.SelectedValue,
                txtFiltroAvanzado.Text,
                activo);
            dgvPacientes.DataBind();
        }

        private void CargarCriterios()
        {
            ddlCriterio.Items.Clear();

            if (ddlCampo.SelectedValue == "DNI")
            {
                ddlCriterio.Items.Add("Igual a");
                ddlCriterio.Items.Add("Contiene");
            }
            else
            {
                ddlCriterio.Items.Add("Contiene");
                ddlCriterio.Items.Add("Comienza con");
                ddlCriterio.Items.Add("Termina con");
            }
        }

        private bool? ObtenerActivoSeleccionado()
        {
            if (ddlEstado.SelectedValue == "Activo")
            {
                return true;
            }

            if (ddlEstado.SelectedValue == "Inactivo")
            {
                return false;
            }

            return null;
        }
    }
}
