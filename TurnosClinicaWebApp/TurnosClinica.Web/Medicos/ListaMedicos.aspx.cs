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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (chkAvanzado.Checked && ddlCriterio.Items.Count == 0)
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
            txtFiltro.Enabled = !chkAvanzado.Checked;

            if (chkAvanzado.Checked)
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

            ddlCriterio.Items.Add("Contiene");
            ddlCriterio.Items.Add("Igual a");
            ddlCriterio.Items.Add("Comienza con");
            ddlCriterio.Items.Add("Termina con");
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = string.Empty;
            txtFiltroAvanzado.Text = string.Empty;
            chkAvanzado.Checked = false;
            txtFiltro.Enabled = true;
            ddlCriterio.Items.Clear();
            CargarLista();
        }

        protected void dgvMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Ver" && e.CommandName != "Toggle")
            {
                return;
            }

            try
            {
                string[] argumentos = e.CommandArgument.ToString().Split('|');
                int idMedico = Convert.ToInt32(argumentos[0]);
                bool activo = Convert.ToBoolean(argumentos[1]);

                if (e.CommandName == "Ver")
                {
                    Response.Redirect("FormularioMedico.aspx?id=" + idMedico, false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                MedicoNegocio negocio = new MedicoNegocio();
                if (activo)
                {
                    negocio.Desactivar(idMedico);
                }
                else
                {
                    negocio.Activar(idMedico);
                }

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
