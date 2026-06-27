using System;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class CambiarContrasenaPendiente : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = ObtenerUsuarioPendiente();
                if (usuario == null)
                {
                    return;
                }

                if (!IsPostBack)
                {
                    LblCuenta.Text = Server.HtmlEncode(usuario.NombreUsuario + " - " + usuario.Persona.Apellido + ", " + usuario.Persona.Nombre);
                    PnlCuenta.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = ObtenerUsuarioPendiente();
                if (usuario == null)
                {
                    return;
                }

                Usuario usuarioActualizado = new AccesoService().CambiarContrasenaPendiente(
                    usuario.IdUsuario,
                    TxtNuevaContrasena.Text,
                    TxtConfirmacionContrasena.Text);

                Session["UsuarioActual"] = usuarioActualizado;
                Response.Redirect("PanelPrincipal.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        private Usuario ObtenerUsuarioPendiente()
        {
            if (!(Session["UsuarioActual"] is Usuario usuario) || usuario.IdUsuario <= 0)
            {
                Response.Redirect("Ingresar.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return null;
            }

            if (usuario.EstadoUsuario == null
                || !string.Equals(
                    usuario.EstadoUsuario.Nombre,
                    EstadoUsuarioEnum.CambioClavePendiente.ToString(),
                    StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("PanelPrincipal.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return null;
            }

            return usuario;
        }
    }
}
