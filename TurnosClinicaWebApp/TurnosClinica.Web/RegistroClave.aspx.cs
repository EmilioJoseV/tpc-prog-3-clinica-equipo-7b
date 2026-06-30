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
                LstCorreosSugeridos.Visible = false;
                PnlCuadroInfoUsuario.Visible = false;
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
                    LstCorreosSugeridos.Items.Clear();
                    foreach (var usu in usuariosFiltrados)
                    {
                        string datosConcatenados = $"{usu.Persona.Nombre}|{usu.Persona.Apellido}|{usu.Rol.Nombre}|{usu.EstadoUsuario.Nombre}";
                        ListItem item = new ListItem(usu.Persona.Email, datosConcatenados);
                        LstCorreosSugeridos.Items.Add(item);
                    }
                    LstCorreosSugeridos.Visible = true;
                }
                else
                {
                    LstCorreosSugeridos.Visible = false;
                    PnlCuadroInfoUsuario.Visible = false;
                    MostrarError("No se encontraron correos que coincidan.");
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al filtrar los usuarios: " + ex.Message);
            }
        }
        protected void LstCorreosSugeridos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstCorreosSugeridos.SelectedItem == null) return;

            string emailSeleccionado = LstCorreosSugeridos.SelectedItem.Text;
            string datosConcatenados = LstCorreosSugeridos.SelectedItem.Value;

            TxtEmail.Text = emailSeleccionado;
            LstCorreosSugeridos.Visible = false;

            string[] partes = datosConcatenados.Split('|');
            if (partes.Length == 4)
            {
                LblInfoNombreCompleto.Text = partes[1] + ", " + partes[0];
                LblInfoCorreo.Text = emailSeleccionado;
                LblInfoRol.Text = partes[2];
                LblInfoEstado.Text = partes[3];

                if (partes[3].Equals(EstadoUsuarioEnum.Activo.ToString(), StringComparison.OrdinalIgnoreCase))
                    LblInfoEstado.CssClass = "badge bg-success";
                else if (partes[3].Equals(EstadoUsuarioEnum.CambioClavePendiente.ToString(), StringComparison.OrdinalIgnoreCase))
                    LblInfoEstado.CssClass = "badge bg-warning text-dark";
                else
                    LblInfoEstado.CssClass = "badge bg-danger";

                PnlCuadroInfoUsuario.Visible = true;
            }
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