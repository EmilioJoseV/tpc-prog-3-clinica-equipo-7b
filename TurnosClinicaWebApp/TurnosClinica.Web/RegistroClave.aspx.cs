using System;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class RegistroClave : Page
    {
        private readonly UsuarioNegocio usuarioNegocio = new UsuarioNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnContinuar_Click(object sender, EventArgs e)
        {
            ValidarEmailPendiente();
        }

        protected void BtnActivar_Click(object sender, EventArgs e)
        {
            PnlMensaje.Visible = false;

            try
            {
                Usuario usuario = usuarioNegocio.RegistrarCredencialesPendientes(
                    TxtEmail.Text,
                    TxtNombreUsuario.Text,
                    TxtClave.Text,
                    TxtClaveConfirmar.Text);

                Session["UsuarioActual"] = usuario;
                Response.Redirect("~/PanelPrincipal.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrio un error al procesar el registro: " + ex.Message);
            }
        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Ingresar.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected string ObtenerClaseEstado(object estado)
        {
            if (estado == null)
            {
                return "badge bg-secondary";
            }

            string nombreEstado = estado.ToString();
            if (string.Equals(nombreEstado, EstadoUsuarioEnum.Activo.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "badge bg-success";
            }

            if (string.Equals(nombreEstado, EstadoUsuarioEnum.Pendiente.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "badge bg-warning text-dark";
            }

            if (string.Equals(nombreEstado, EstadoUsuarioEnum.CambioClavePendiente.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "badge bg-info text-dark";
            }

            return "badge bg-danger";
        }

        private void ValidarEmailPendiente()
        {
            PnlMensaje.Visible = false;
            PnlFormularioClave.Visible = false;

            string email = TxtEmail.Text.Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            try
            {
                Usuario usuario = usuarioNegocio.ObtenerPorEmail(email);
                if (usuario == null)
                {
                    MostrarError("No existe un usuario registrado con ese correo.");
                    return;
                }

                if (!EsEstado(usuario, EstadoUsuarioEnum.Pendiente))
                {
                    MostrarError(ObtenerMensajeEstadoNoPendiente(usuario, email));
                    return;
                }

                TxtEmail.Text = usuario.Persona.Email;
                LblUsuarioSeleccionado.Text = usuario.Persona.Email;
                TxtNombreUsuario.Text = string.Empty;
                TxtClave.Text = string.Empty;
                TxtClaveConfirmar.Text = string.Empty;
                PnlFormularioClave.Visible = true;
            }
            catch (Exception ex)
            {
                MostrarError("Error al validar el correo: " + ex.Message);
            }
        }

        private bool EsEstado(Usuario usuario, EstadoUsuarioEnum estado)
        {
            return usuario != null
                && usuario.EstadoUsuario != null
                && string.Equals(
                    usuario.EstadoUsuario.Nombre,
                    estado.ToString(),
                    StringComparison.OrdinalIgnoreCase);
        }

        private string ObtenerMensajeEstadoNoPendiente(Usuario usuario, string email)
        {
            if (EsEstado(usuario, EstadoUsuarioEnum.Activo))
            {
                return "El usuario con el correo " + email + " ya se encuentra activo.";
            }

            if (EsEstado(usuario, EstadoUsuarioEnum.Inactivo))
            {
                return "El usuario con el correo " + email + " se encuentra inactivo.";
            }

            if (EsEstado(usuario, EstadoUsuarioEnum.Bloqueado))
            {
                return "El usuario con el correo " + email + " se encuentra bloqueado.";
            }

            if (EsEstado(usuario, EstadoUsuarioEnum.CambioClavePendiente))
            {
                return "El usuario con el correo " + email + " debe cambiar su contrasena desde el ingreso.";
            }

            return "La cuenta no esta pendiente de registro.";
        }

        private void MostrarError(string mensaje)
        {
            PnlMensaje.Visible = true;
            PnlMensaje.CssClass = "alert alert-danger alert-dismissible fade show mt-3";
            LblMensajeTexto.Text = mensaje;
        }
    }
}
