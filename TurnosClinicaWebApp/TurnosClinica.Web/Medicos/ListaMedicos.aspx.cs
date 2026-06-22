using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class ListaMedicos : Page
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
                MedicoNegocio negocio = new MedicoNegocio();
                dgvMedicos.DataSource = negocio.Listar();
                dgvMedicos.DataBind();
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
                MedicoNegocio negocio = new MedicoNegocio();
                dgvMedicos.DataSource = negocio.ListarFiltroRapido(txtFiltro.Text);
                dgvMedicos.DataBind();
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

                MedicoNegocio negocio = new MedicoNegocio();
                dgvMedicos.DataSource = negocio.ListarFiltroAvanzado(
                    ddlCampo.SelectedItem.ToString(),
                    ddlCriterio.SelectedItem.ToString(),
                    txtFiltroAvanzado.Text,
                    ObtenerEstadoSeleccionado());
                dgvMedicos.DataBind();
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

        protected void BtnNuevoMedico_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioMedico.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
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

        protected void dgvMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Editar" && e.CommandName != "Desactivar")
            {
                return;
            }

            try
            {
                int idMedico = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "Editar")
                {
                    Response.Redirect("FormularioMedico.aspx?id=" + idMedico, false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                MedicoNegocio negocio = new MedicoNegocio();
                negocio.Desactivar(idMedico);
                CargarLista();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
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
