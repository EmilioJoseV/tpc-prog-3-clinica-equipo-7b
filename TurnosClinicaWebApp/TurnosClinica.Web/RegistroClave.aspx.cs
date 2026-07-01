using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class RegistroClave : System.Web.UI.Page
    {
        private UsuarioDatos usuarioDatos;
        private AccesoDatosBase conexionBase;

        protected void Page_Init(object sender, EventArgs e)
        {
            conexionBase = new AccesoDatosBase();
            usuarioDatos = new UsuarioDatos(conexionBase);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void TxtEmail_TextChanged(object sender, EventArgs e)
        {
            PnlMensaje.Visible = false;
            string filtro = TxtEmail.Text.Trim();

            if (string.IsNullOrEmpty(filtro) || filtro.Length < 2)
            {
                DgvUsuarios.Visible = false;
                PnlFormularioClave.Visible = false;
                return;
            }

            try
            {
                List<Usuario> todosLosUsuarios = usuarioDatos.Listar();

                var usuariosFiltrados = todosLosUsuarios
                    .Where(u => u.Persona != null &&
                                !string.IsNullOrEmpty(u.Persona.Email) &&
                                u.Persona.Email.ToLower().Contains(filtro.ToLower()))
                    .ToList();

                if (usuariosFiltrados.Count > 0)
                {
                    DgvUsuarios.DataSource = usuariosFiltrados;
                    DgvUsuarios.DataBind();
                    DgvUsuarios.Visible = true;
                }
                else
                {
                    DgvUsuarios.Visible = false;
                    PnlFormularioClave.Visible = false;
                    MostrarError("No se encontraron correos que coincidan.");
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al filtrar los usuarios: " + ex.Message);
            }
        }
        protected void DgvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Seleccionar")
            {
                PnlMensaje.Visible = false;

                string argumento = e.CommandArgument.ToString();
                string[] partes = argumento.Split('|');

                if (partes.Length == 2)
                {
                    string emailSeleccionado = partes[0];
                    string estadoSeleccionado = partes[1];

                    if (estadoSeleccionado.Equals(EstadoUsuarioEnum.Activo.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        PnlFormularioClave.Visible = false;

                        MostrarError($"El usuario con el correo {emailSeleccionado} ya se encuentra dado de alta en el sistema y operativo.");
                        return;
                    }

                    TxtEmail.Text = emailSeleccionado;
                    LblUsuarioSeleccionado.Text = emailSeleccionado;

                    DgvUsuarios.Visible = false;
                    PnlFormularioClave.Visible = true;
                }
            }
        }
        protected string ObtenerClaseEstado(object estado)
        {
            if (estado == null) return "badge bg-secondary";

            string nombreEstado = estado.ToString().ToLower();
            if (nombreEstado == "activo") return "badge bg-success";
            if (nombreEstado == "cambioclavependiente") return "badge bg-warning text-dark";

            return "badge bg-danger";
        }
        protected void BtnLimpiar_Click(object sender, EventArgs e)
        {
            TxtEmail.Text = string.Empty;
            DgvUsuarios.Visible = false;
            PnlFormularioClave.Visible = false;
            PnlMensaje.Visible = false;
        }

        protected void BtnActivar_Click(object sender, EventArgs e)
        {
            PnlMensaje.Visible = false;

            string emailIngresado = TxtEmail.Text.Trim();
            string clave = TxtClave.Text;
            string claveConfirmar = TxtClaveConfirmar.Text;

            if (string.IsNullOrWhiteSpace(emailIngresado))
            {
                MostrarError("Por favor, completá el email.");
                return;
            }

            try
            {
                SeguridadService seguridadService = new SeguridadService();
                seguridadService.ValidarNuevaContrasena(clave, claveConfirmar);

                Usuario usuario = usuarioDatos.ObtenerPorEmail(emailIngresado);

                if (usuario == null)
                {
                    MostrarError("El correo ingresado no corresponde a ningún usuario del sistema.");
                    return;
                }

                usuario.PasswordHash = seguridadService.CalcularHash(clave.Trim());

                usuario.EstadoUsuario = new EstadoUsuario
                {
                    Nombre = EstadoUsuarioEnum.Activo.ToString()
                };

                usuarioDatos.Modificar(usuario);

                Session["UsuarioActual"] = usuario;

                Response.Redirect("~/PanelPrincipal.aspx", false);
            }
            catch (Exception ex)
            {
                MostrarError("Ocurrió un error al activar la cuenta: " + ex.Message);
            }
        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Ingresar.aspx", false);
        }
        private void MostrarError(string mensaje)
        {
            PnlMensaje.Visible = true;
            PnlMensaje.CssClass = "alert alert-danger alert-dismissible fade show mt-3";
            LblMensajeTexto.Text = mensaje;
        }
    }
}
