using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

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
                    // Enlazamos el resultado directamente a la grilla corporativa
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
                string emailSeleccionado = e.CommandArgument.ToString();

                // Fijamos el TextBox con el valor seleccionado y seteamos la etiqueta del panel
                TxtEmail.Text = emailSeleccionado;
                LblUsuarioSeleccionado.Text = emailSeleccionado;

                // Ocultamos la grilla y desplegamos el formulario para cargar la contraseña
                DgvUsuarios.Visible = false;
                PnlFormularioClave.Visible = true;
            }
        }

        /// <summary>
        /// Nuevo método: Retorna la clase CSS del badge según el estado (idéntico a tu ListaUsuarios)
        /// </summary>
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

            if (string.IsNullOrWhiteSpace(emailIngresado) || string.IsNullOrWhiteSpace(clave))
            {
                MostrarError("Por favor, completá todos los campos requeridos.");
                return;
            }

            if (clave != claveConfirmar)
            {
                MostrarError("Las contraseñas ingresadas no coinciden.");
                return;
            }

            try
            {
                Usuario usuario = usuarioDatos.ObtenerPorEmail(emailIngresado);

                if (usuario == null)
                {
                    MostrarError("El correo ingresado no corresponde a ningún usuario del sistema.");
                    return;
                }

                usuario.PasswordHash = clave;

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