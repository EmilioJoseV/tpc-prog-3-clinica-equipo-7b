using System;
using System.Collections.Generic;
using System.Web.UI;
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
            if (!IsPostBack)
            {
                CargarCorreosDatalist();
            }
        }
        private void CargarCorreosDatalist()
        {
            try
            {
                List<Usuario> listaUsuarios = usuarioDatos.Listar();
                List<string> listaCorreos = new List<string>();

                foreach (var usu in listaUsuarios)
                {
                    if (usu.Persona != null && !string.IsNullOrWhiteSpace(usu.Persona.Email))
                    {
                        if (!listaCorreos.Contains(usu.Persona.Email))
                        {
                            listaCorreos.Add(usu.Persona.Email);
                        }
                    }
                }

                RepCorreos.DataSource = listaCorreos;
                RepCorreos.DataBind();
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar el listado de validación: " + ex.Message);
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
                    MostrarError("El correo ingresado no se encuentra registrado en el sistema clínico.");
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
                MostrarError("Ocurrió un error al procesar el alta: " + ex.Message);
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