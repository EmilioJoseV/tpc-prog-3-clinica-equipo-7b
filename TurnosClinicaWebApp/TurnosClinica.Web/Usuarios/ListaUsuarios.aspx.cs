using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TurnosClinica.Dominio.Enums;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class ListaUsuarios : Page
    {
        private readonly UsuarioNegocio usuarioNegocio = new UsuarioNegocio();

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
            ddlActivo.SelectedIndex = 0;
            CargarLista();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = string.Empty;
            txtFiltroAvanzado.Text = string.Empty;
            chkAvanzado.Checked = false;
            ddlCampo.SelectedIndex = 0;
            ddlCriterio.SelectedIndex = 0;
            ddlActivo.SelectedIndex = 0;
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

        protected void dgvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Ver" && e.CommandName != "Toggle")
            {
                return;
            }

            try
            {
                string[] argumentos = e.CommandArgument.ToString().Split('|');
                int idUsuario = Convert.ToInt32(argumentos[0]);
                bool activo = argumentos.Length > 1 && Convert.ToBoolean(argumentos[1]);

                if (e.CommandName == "Ver")
                {
                    Response.Redirect("FormularioUsuario.aspx?id=" + idUsuario, false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                if (activo)
                {
                    usuarioNegocio.Desactivar(idUsuario);
                }
                else
                {
                    usuarioNegocio.Activar(idUsuario);
                }

                if (chkAvanzado.Checked)
                {
                    BuscarAvanzado();
                }
                else
                {
                    CargarLista(txtFiltro.Text);
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        protected bool EstaActivo(object estado)
        {
            return !string.Equals(
                Convert.ToString(estado),
                EstadoUsuarioEnum.Inactivo.ToString(),
                StringComparison.OrdinalIgnoreCase);
        }

        protected string ObtenerClaseEstado(object estado)
        {
            string nombre = Convert.ToString(estado);

            if (string.Equals(nombre, EstadoUsuarioEnum.Activo.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "badge bg-success";
            }

            if (string.Equals(nombre, EstadoUsuarioEnum.Pendiente.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "badge bg-warning text-dark";
            }

            if (string.Equals(nombre, EstadoUsuarioEnum.Bloqueado.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "badge bg-danger";
            }

            if (string.Equals(nombre, EstadoUsuarioEnum.CambioClavePendiente.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "badge bg-info text-dark";
            }

            return "badge bg-secondary";
        }

        private void CargarLista(string palabra = null)
        {
            dgvUsuarios.DataSource = string.IsNullOrWhiteSpace(palabra)
                ? usuarioNegocio.Listar()
                : usuarioNegocio.ListarFiltroRapido(palabra);
            dgvUsuarios.DataBind();
        }

        private void BuscarAvanzado()
        {
            dgvUsuarios.DataSource = usuarioNegocio.ListarFiltroAvanzado(
                ddlCampo.SelectedValue,
                ddlCriterio.SelectedValue,
                txtFiltroAvanzado.Text,
                ObtenerActivoSeleccionado());
            dgvUsuarios.DataBind();
        }

        private bool? ObtenerActivoSeleccionado()
        {
            if (ddlActivo.SelectedValue == "Activo")
            {
                return true;
            }

            if (ddlActivo.SelectedValue == "Inactivo")
            {
                return false;
            }

            return null;
        }

        private void MostrarError(Exception ex)
        {
            Session.Add("error", ex.ToString());
            Response.Redirect("../Error.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
