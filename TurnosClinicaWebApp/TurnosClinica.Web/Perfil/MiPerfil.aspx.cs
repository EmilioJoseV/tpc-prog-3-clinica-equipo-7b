using System;
using System.IO;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio; 

namespace TurnosClinica.Web
{
    public partial class MiPerfil : Page
    {
        
        private const string ImagenTemporalSessionKey = "ImagenTemporalMiPerfil";
        private const string ImagenTemporalTipoSessionKey = "ImagenTemporalMiPerfilTipo";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LimpiarImagenTemporal();
                    CargarDatosIniciales();
                }
                else if (Session[ImagenTemporalSessionKey] is byte[] imagenTemporal)
                {
                    MostrarImagen(
                        imagenTemporal,
                        Convert.ToString(Session[ImagenTemporalTipoSessionKey]));
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        private void CargarDatosIniciales()
        {
            Usuario user = (Usuario)Session["UsuarioActual"];
            if (user != null)
            {
                txtNombre.Text = user.Persona.Nombre;
                txtApellido.Text = user.Persona.Apellido;
                txtEmail.Text = user.Persona.Email;
                txtNombreUsuario.Text = user.NombreUsuario;


                MostrarImagenActual(user);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario user = (Usuario)Session["UsuarioActual"];

                string userPasswordAnterior = user.PasswordHash;
                if (user != null)
                {
                    
                    user.Persona.Nombre = txtNombre.Text;
                    user.Persona.Apellido = txtApellido.Text;
                    user.Persona.Email = txtEmail.Text; user.NombreUsuario = txtNombreUsuario.Text;

                    if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                    {

                        user.PasswordHash = new SeguridadService().CalcularHash(txtPassword.Text);
                    }
                    else {
                        user.PasswordHash = userPasswordAnterior;
                    }


                        user.Imagen = ObtenerImagenParaGuardar(user);

                    UsuarioNegocio negocio = new UsuarioNegocio();
                    negocio.ModificarConPersona(user);

                    Session["UsuarioActual"] = user;

                    LimpiarImagenTemporal();

                    Response.Redirect("~/PanelPrincipal.aspx", false);
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            
            LimpiarImagenTemporal();
            Response.Redirect("~/PanelPrincipal.aspx", false);
        }



        protected void btnPrevisualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!fileImagen.HasFile)
                {
                    throw new Exception("Debe seleccionar una imagen para previsualizar.");
                }

                string extension = Path.GetExtension(fileImagen.FileName);
                if (!string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("La imagen debe tener formato JPG, JPEG o PNG.");
                }

                Session[ImagenTemporalSessionKey] = fileImagen.FileBytes;
                Session[ImagenTemporalTipoSessionKey] = ObtenerTipoImagen(extension);
                MostrarImagen(fileImagen.FileBytes, ObtenerTipoImagen(extension));
            }
            catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        private byte[] ObtenerImagenParaGuardar(Usuario usuarioActual)
        {
            if (Session[ImagenTemporalSessionKey] is byte[] imagenTemporal)
            {
                return imagenTemporal;
            }

            if (fileImagen.HasFile)
            {
                return fileImagen.FileBytes;
            }

            return usuarioActual != null ? usuarioActual.Imagen : null;
        }

        private void MostrarImagenActual(Usuario usuario)
        {
            if (usuario.Imagen != null && usuario.Imagen.Length > 0)
            {
                MostrarImagen(usuario.Imagen, "image/jpeg");
                return;
            }

            MostrarInicial(usuario.Persona.Nombre, usuario.Persona.Apellido);
        }

        private void MostrarImagen(byte[] imagen, string tipo)
        {
            imgPerfil.ImageUrl = "data:" + tipo + ";base64," + Convert.ToBase64String(imagen);
            imgPerfil.Visible = true;
            pnlInicial.Visible = false;
        }

        private void MostrarInicial(string nombre, string apellido)
        {
            string inicial = !string.IsNullOrWhiteSpace(nombre)
                ? nombre.Trim().Substring(0, 1)
                : !string.IsNullOrWhiteSpace(apellido)
                    ? apellido.Trim().Substring(0, 1)
                    : "U";

            litInicial.Text = inicial.ToUpper();
            imgPerfil.Visible = false;
            pnlInicial.Visible = true;
        }

        private string ObtenerTipoImagen(string extension)
        {
            return string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase)
                ? "image/png"
                : "image/jpeg";
        }

        private void LimpiarImagenTemporal()
        {
            Session.Remove(ImagenTemporalSessionKey);
            Session.Remove(ImagenTemporalTipoSessionKey);
        }

        private void MostrarError(string mensaje)
        {
            ((MasterLayout)Master).MostrarError(mensaje);
        }
    }
}
