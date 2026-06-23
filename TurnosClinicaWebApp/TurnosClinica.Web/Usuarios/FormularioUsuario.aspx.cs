using System;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class FormularioUsuario : Page
    {
        private readonly UsuarioNegocio usuarioNegocio = new UsuarioNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CargarUsuario();
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
                    Imagen = fileImagen.HasFile
                        ? fileImagen.FileBytes
                        : usuarioActual != null ? usuarioActual.Imagen : null,
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
            Response.Redirect("ListaUsuarios.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void CargarUsuario()
        {
            int idUsuario;
            if (!int.TryParse(Request.QueryString["id"], out idUsuario))
            {
                chkActivo.Checked = true;
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
            Session.Add("error", ex.ToString());
            Response.Redirect("../Error.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
