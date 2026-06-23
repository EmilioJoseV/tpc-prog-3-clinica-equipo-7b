using System;
using System.IO;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class FormularioUsuario : Page
    {
        private readonly UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
        private const string ImagenTemporalSessionKey = "ImagenTemporalUsuario";
        private const string ImagenTemporalTipoSessionKey = "ImagenTemporalUsuarioTipo";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LimpiarImagenTemporal();
                    CargarUsuario();
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
                MostrarError(ex);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = ObtenerId(hfIdUsuario.Value);
                Usuario usuarioActual = idUsuario > 0 ? usuarioNegocio.ObtenerPorId(idUsuario) : null;

                Usuario usuario = new Usuario
                {
                    IdUsuario = idUsuario,
                    Persona = new Persona
                    {
                        IdPersona = ObtenerId(hfIdPersona.Value),
                        DNI = txtDni.Text,
                        Nombre = txtNombre.Text,
                        Apellido = txtApellido.Text,
                        Telefono = txtTelefono.Text,
                        Email = txtEmail.Text
                    },
                    Rol = new Rol
                    {
                        Nombre = ddlRol.SelectedValue
                    },
                    NombreUsuario = usuarioActual != null ? usuarioActual.NombreUsuario : null,
                    PasswordHash = usuarioActual != null ? usuarioActual.PasswordHash : null,
                    Imagen = ObtenerImagenParaGuardar(usuarioActual),
                    EstadoUsuario = ObtenerEstadoParaGuardar(usuarioActual)
                };

                if (usuario.IdUsuario > 0)
                {
                    usuarioNegocio.ModificarConPersona(usuario);
                }
                else
                {
                    usuarioNegocio.AgregarConPersona(usuario);
                }

                LimpiarImagenTemporal();
                Response.Redirect("ListaUsuarios.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarImagenTemporal();
            Response.Redirect("ListaUsuarios.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
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
                MostrarError(ex);
            }
        }

        private void CargarUsuario()
        {
            int idUsuario;
            if (!int.TryParse(Request.QueryString["id"], out idUsuario))
            {
                chkActivo.Checked = true;
                MostrarInicial(null, null);
                return;
            }

            Usuario usuario = usuarioNegocio.ObtenerPorId(idUsuario);
            if (usuario == null)
            {
                throw new Exception("El usuario no existe.");
            }

            ValidarRolAdministrativo(usuario);

            hfIdUsuario.Value = usuario.IdUsuario.ToString();
            hfIdPersona.Value = usuario.Persona.IdPersona.ToString();
            lblTitulo.Text = "Detalle de Usuario";
            txtDni.Text = usuario.Persona.DNI;
            txtNombre.Text = usuario.Persona.Nombre;
            txtApellido.Text = usuario.Persona.Apellido;
            txtTelefono.Text = usuario.Persona.Telefono;
            txtEmail.Text = usuario.Persona.Email;
            ddlRol.SelectedValue = usuario.Rol.Nombre;
            lblEstado.Text = usuario.EstadoUsuario.Nombre;
            lblEstado.CssClass = ObtenerClaseEstado(usuario.EstadoUsuario.Nombre);
            chkActivo.Checked = !EsEstado(usuario, EstadoUsuarioEnum.Inactivo);
            MostrarImagenActual(usuario);
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

        private EstadoUsuario ObtenerEstadoParaGuardar(Usuario usuarioActual)
        {
            if (usuarioActual == null)
            {
                return new EstadoUsuario
                {
                    Nombre = EstadoUsuarioEnum.Pendiente.ToString()
                };
            }

            if (!chkActivo.Checked)
            {
                return new EstadoUsuario
                {
                    Nombre = EstadoUsuarioEnum.Inactivo.ToString()
                };
            }

            if (EsEstado(usuarioActual, EstadoUsuarioEnum.Inactivo))
            {
                return new EstadoUsuario
                {
                    Nombre = EstadoUsuarioEnum.Activo.ToString()
                };
            }

            return usuarioActual.EstadoUsuario;
        }

        private void ValidarRolAdministrativo(Usuario usuario)
        {
            bool esAdministrador = usuario.Rol != null
                && string.Equals(
                    usuario.Rol.Nombre,
                    RolEnum.Administrador.ToString(),
                    StringComparison.OrdinalIgnoreCase);
            bool esRecepcionista = usuario.Rol != null
                && string.Equals(
                    usuario.Rol.Nombre,
                    RolEnum.Recepcionista.ToString(),
                    StringComparison.OrdinalIgnoreCase);

            if (!esAdministrador && !esRecepcionista)
            {
                throw new Exception("El usuario no pertenece a esta funcionalidad.");
            }
        }

        private bool EsEstado(Usuario usuario, EstadoUsuarioEnum estado)
        {
            return usuario.EstadoUsuario != null
                && string.Equals(
                    usuario.EstadoUsuario.Nombre,
                    estado.ToString(),
                    StringComparison.OrdinalIgnoreCase);
        }

        private string ObtenerClaseEstado(string estado)
        {
            if (string.Equals(estado, EstadoUsuarioEnum.Activo.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "badge bg-success";
            }

            if (string.Equals(estado, EstadoUsuarioEnum.Pendiente.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "badge bg-warning text-dark";
            }

            if (string.Equals(estado, EstadoUsuarioEnum.Bloqueado.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "badge bg-danger";
            }

            if (string.Equals(estado, EstadoUsuarioEnum.CambioClavePendiente.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "badge bg-info text-dark";
            }

            return "badge bg-secondary";
        }

        private int ObtenerId(string valor)
        {
            int id;
            return int.TryParse(valor, out id) ? id : 0;
        }

        private void MostrarError(Exception ex)
        {
            ((MasterLayout)Master).MostrarError(ex.Message);
        }
    }
}
