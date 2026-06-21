using System;
using System.Linq;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class FormularioUsuario : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Preguntamos si viene un ID por la URL (Modificación)
                if (Request.QueryString["id"] != null)
                {
                    int idUsuario = Convert.ToInt32(Request.QueryString["id"]);
                    UsuarioNegocio negocio = new UsuarioNegocio();
                    Usuario usuario = negocio.ObtenerPorId(idUsuario);

                    if (usuario != null)
                    {
                        // SOLO cargamos los datos en los controles la PRIMERA VEZ
                        if (!IsPostBack)
                        {
                            lblTitulo.InnerText = "Modificar Usuario";
                            if (usuario.Rol != null && string.Equals(usuario.Rol.Nombre, RolEnum.Medico.ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                // Medico queda fuera de este formulario.
                                Response.Redirect("ListaUsuarios.aspx", false);
                                Context.ApplicationInstance.CompleteRequest();
                                return;
                            }

                            CargarDesplegables();
                            HfIdPersona.Value = usuario.IdPersona.ToString();
                            txtDni.Text = usuario.DNI;
                            txtNombre.Text = usuario.Nombre;
                            txtApellido.Text = usuario.Apellido;
                            txtTelefono.Text = usuario.Telefono;
                            txtNombreUsuario.Text = usuario.NombreUsuario;
                            txtEmail.Text = usuario.Email;
                            txtPassword.Text = string.Empty;

                            if (usuario.Rol != null)
                            {
                                ddlRol.SelectedValue = usuario.Rol.Nombre;
                            }

                        }

                        // ESTO SE EJECUTA SIEMPRE (En la primera carga y en cada PostBack)
                        btnInactivar.Visible = true;
                        btnEliminar.Visible = true;

                        if (usuario.EstadoUsuario == EstadoUsuarioEnum.Activo)
                        {
                            btnInactivar.Text = "Inactivar";
                            btnInactivar.CssClass = "btn btn-warning";
                        }
                        else
                        {
                            btnInactivar.Text = "Activar";
                            btnInactivar.CssClass = "btn btn-success";
                        }
                    }
                }
                else
                {
                    // 2. SI ES UN ALTA NUEVA (No hay ID en la URL)
                    if (!IsPostBack)
                    {
                        lblTitulo.InnerText = "Nuevo Usuario";
                        CargarDesplegables();
                    }

                    // ESTO SE EJECUTA SIEMPRE PARA EL ALTA
                    btnInactivar.Visible = false;
                    btnEliminar.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Session.Add("Error", ex.ToString());
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ddlRol.SelectedValue))
                {
                    throw new Exception("Debe seleccionar un rol.");
                }

                if (Request.QueryString["id"] == null && string.Equals(ddlRol.SelectedValue, RolEnum.Medico.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Este formulario no crea usuarios con rol Medico. Las cuentas de medico se generan automaticamente desde el formulario de medicos.");
                }

                if (Request.QueryString["id"] == null && string.Equals(ddlRol.SelectedValue, RolEnum.Paciente.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Este formulario no crea usuarios con rol Paciente. Las cuentas de paciente se generan automaticamente desde el formulario de pacientes.");
                }

                UsuarioNegocio negocio = new UsuarioNegocio();
                Usuario usuario = new Usuario();

                if (Request.QueryString["id"] != null)
                {
                    usuario.IdUsuario = Convert.ToInt32(Request.QueryString["id"]);

                    // Si modificamos, mantenemos el estado que ya tenía en la DB
                    Usuario usuarioExistente = negocio.ObtenerPorId(usuario.IdUsuario);
                    if (usuarioExistente != null)
                    {
                        usuario.EstadoUsuario = usuarioExistente.EstadoUsuario;
                    }
                }
                else
                {
                    // Si es un usuario nuevo, nace pendiente.
                    usuario.EstadoUsuario = EstadoUsuarioEnum.Pendiente;
                }

                if (!string.IsNullOrWhiteSpace(HfIdPersona.Value))
                {
                    usuario.IdPersona = Convert.ToInt32(HfIdPersona.Value);
                }
                else
                {
                    usuario.IdPersona = 0;
                }

                usuario.DNI = txtDni.Text.Trim();
                usuario.Nombre = txtNombre.Text.Trim();
                usuario.Apellido = txtApellido.Text.Trim();
                usuario.Telefono = txtTelefono.Text.Trim();
                if (Request.QueryString["id"] != null)
                {
                    usuario.NombreUsuario = string.IsNullOrWhiteSpace(txtNombreUsuario.Text)
                        ? negocio.ObtenerPorId(usuario.IdUsuario)?.NombreUsuario
                        : txtNombreUsuario.Text.Trim();
                }
                else
                {
                    usuario.NombreUsuario = null;
                }
                usuario.Email = txtEmail.Text.Trim();
                if (Request.QueryString["id"] != null && string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    Usuario usuarioExistente = negocio.ObtenerPorId(usuario.IdUsuario);
                    usuario.PasswordHash = usuarioExistente != null ? usuarioExistente.PasswordHash : null;
                }
                else
                {
                    usuario.PasswordHash = Request.QueryString["id"] != null ? txtPassword.Text : null;
                }

                if (fileImagen.HasFile)
                {
                    usuario.Imagen = fileImagen.FileBytes;
                }
                else if (usuario.IdUsuario > 0)
                {
                    Usuario usuarioActual = negocio.ObtenerPorId(usuario.IdUsuario);
                    usuario.Imagen = usuarioActual?.Imagen;
                }
                else
                {
                    usuario.Imagen = null;
                }

                usuario.Rol = new Rol();
                usuario.Rol.Nombre = ddlRol.SelectedValue;

                if (usuario.IdUsuario > 0)
                {
                    negocio.Modificar(usuario);
                }
                else
                {
                    negocio.Agregar(usuario);
                }

                Response.Redirect("ListaUsuarios.aspx");
            }
            catch (Exception ex)
            {
                Session.Add("Error", ex.ToString());
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListaUsuarios.aspx");
        }

        private void CargarDesplegables()
        {
            var roles = Enum.GetValues(typeof(TurnosClinica.Dominio.Enums.RolEnum))
                            .Cast<TurnosClinica.Dominio.Enums.RolEnum>()
                            .Where(r => r != RolEnum.Medico && r != RolEnum.Paciente)
                            .Select(r => new
                            {
                                Id = r.ToString(),
                                Nombre = r.ToString()
                            }).ToList();

            ddlRol.DataSource = roles;
            ddlRol.DataValueField = "Id";
            ddlRol.DataTextField = "Nombre";
            ddlRol.DataBind();
            ddlRol.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccione un Rol", string.Empty));

        }
        protected void btnInactivar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["id"] != null)
                {
                    int idUsuario = Convert.ToInt32(Request.QueryString["id"]);
                    UsuarioNegocio negocio = new UsuarioNegocio();

                    Usuario usuario = negocio.ObtenerPorId(idUsuario);

                    if (usuario != null)
                    {
                        if (usuario.EstadoUsuario == EstadoUsuarioEnum.Activo)
                        {
                            negocio.EliminarLogico(idUsuario);
                        }
                        else
                        {
                            negocio.AltaLogica(idUsuario);
                        }

                        Response.Redirect("ListaUsuarios.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                Session.Add("Error", ex.ToString());
            }
        }
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["id"] != null)
                {
                    int idUsuario = Convert.ToInt32(Request.QueryString["id"]);

                    TurnosClinica.AccesoDatos.UsuarioDatos datos = new TurnosClinica.AccesoDatos.UsuarioDatos();

                    datos.EliminarFisico(idUsuario);

                    Response.Redirect("ListaUsuarios.aspx");
                }
            }
            catch (Exception ex)
            {
                Session.Add("Error", ex.ToString());
            }
        }
    }
}
