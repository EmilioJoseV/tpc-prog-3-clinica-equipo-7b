using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class ListaEspecialidades : Page
    {
        private readonly EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CargarLista();
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
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
                MostrarError(ex);
            }
        }

        protected void chkAvanzado_CheckedChanged(object sender, EventArgs e)
        {
            txtFiltro.Text = string.Empty;
            txtFiltroAvanzado.Text = string.Empty;
            ddlEstado.SelectedIndex = 0;
            CargarLista();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = string.Empty;
            txtFiltroAvanzado.Text = string.Empty;
            chkAvanzado.Checked = false;
            ddlCampo.SelectedIndex = 0;
            ddlCriterio.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
            CargarLista();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarAvanzado();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        protected void dgvEspecialidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Ver" && e.CommandName != "Toggle")
            {
                return;
            }

            try
            {
                string[] argumentos = e.CommandArgument.ToString().Split('|');
                int idEspecialidad = Convert.ToInt32(argumentos[0]);
                bool activo = argumentos.Length > 1 && Convert.ToBoolean(argumentos[1]);

                if (e.CommandName == "Ver")
                {
                    Response.Redirect("FormularioEspecialidad.aspx?id=" + idEspecialidad, false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                if (activo)
                {
                    especialidadNegocio.Desactivar(idEspecialidad);
                }
                else
                {
                    especialidadNegocio.Activar(idEspecialidad);
                }

                CargarLista(txtFiltro.Text);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void CargarLista(string palabra = null)
        {
            dgvEspecialidades.DataSource = string.IsNullOrWhiteSpace(palabra)
                ? especialidadNegocio.Listar()
                : especialidadNegocio.ListarFiltroRapido(palabra);
            dgvEspecialidades.DataBind();
        }

        private void BuscarAvanzado()
        {
            dgvEspecialidades.DataSource = especialidadNegocio.ListarFiltroAvanzado(
                ddlCampo.SelectedValue,
                ddlCriterio.SelectedValue,
                txtFiltroAvanzado.Text,
                ObtenerActivoSeleccionado());
            dgvEspecialidades.DataBind();
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

        private void MostrarError(Exception ex)
        {
            ((MasterLayout)Master).MostrarError(ex.Message);
        }
    }
}
