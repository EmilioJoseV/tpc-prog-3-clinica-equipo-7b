using System;
using System.Linq;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class FormularioUsuario : Page
    {
        public Usuario usuarioActual { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Siempre recuperamos la entidad para que la foto no se rompa al redibujar
                if (Request.QueryString["id"] != null || Request.QueryString["Id"] != null)
                {
                    string idUrl = Request.QueryString["id"] ?? Request.QueryString["Id"];
                    int idUsuario = Convert.ToInt32(idUrl);
                    UsuarioNegocio negocio = new UsuarioNegocio();
                    usuarioActual = negocio.ObtenerPorId(idUsuario);

                    if (usuarioActual != null)
                    {
                        // PROTECCIÓN TOTAL: La asignación a los TextBox SOLO ocurre en la carga inicial de la página
                        if (!IsPostBack)
                        {
                            CargarDesplegables();

                            txtNombre.Text = usuarioActual.Nombre;
                            txtApellido.Text = usuarioActual.Apellido;
                            txtNombreUsuario.Text = usuarioActual.NombreUsuario;
                            txtEmail.Text = usuarioActual.Email;
                            txtPassword.Text = string.Empty; // Inicia limpio

                            if (usuarioActual.Rol != null)
                            {
                                ddlRol.SelectedValue = usuarioActual.Rol.IdRol.ToString();
                            }

                            if (usuarioActual.Medico != null)
                            {
                                ddlMedico.SelectedValue = usuarioActual.Medico.IdMedico.ToString();
                            }
                        }

                        // Esto se ejecuta siempre para mantener estables los estilos visuales de los botones
                        btnInactivar.Visible = true;
                        btnEliminar.Visible = true;

                        if (usuarioActual.Activo)
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
                    // MODO ALTA NUEVA
                    if (!IsPostBack)
                    {
                        CargarDesplegables();
                    }

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
                if (ddlRol.SelectedValue == "0")
                {
                    return;
                }

                UsuarioNegocio negocio = new UsuarioNegocio();
                Usuario usuario = new Usuario();

                if (Request.QueryString["id"] != null || Request.QueryString["Id"] != null)
                {
                    string idUrl = Request.QueryString["id"] ?? Request.QueryString["Id"];
                    int idUsuario = Convert.ToInt32(idUrl);

                    Usuario usuarioDB = negocio.ObtenerPorId(idUsuario);

                    if (usuarioDB != null)
                    {
                        usuario.IdUsuario = usuarioDB.IdUsuario;
                        usuario.Activo = usuarioDB.Activo;

                        usuario.Nombre = !string.IsNullOrWhiteSpace(txtNombre.Text) ? txtNombre.Text.Trim() : usuarioDB.Nombre;
                        usuario.Apellido = !string.IsNullOrWhiteSpace(txtApellido.Text) ? txtApellido.Text.Trim() : usuarioDB.Apellido;
                        usuario.NombreUsuario = !string.IsNullOrWhiteSpace(txtNombreUsuario.Text) ? txtNombreUsuario.Text.Trim() : usuarioDB.NombreUsuario;
                        usuario.Email = !string.IsNullOrWhiteSpace(txtEmail.Text) ? txtEmail.Text.Trim() : usuarioDB.Email;
                        usuario.PasswordHash = !string.IsNullOrEmpty(txtPassword.Text) ? txtPassword.Text : usuarioDB.PasswordHash;

                        // === LOGICA DE LA IMAGEN MODIFICADA PARA MODIFICACIÓN ===
                        if (Session["ImagenTemporal"] != null)
                        {
                            // Si el usuario le dio al botón "Cargar", la foto está guardada en la Session
                            usuario.Imagen = (byte[])Session["ImagenTemporal"];
                        }
                        else if (fileImagen.HasFile)
                        {
                            // Por las dudas, si no le dio a "Cargar" pero subió un archivo directo antes de poner Aceptar
                            usuario.Imagen = fileImagen.FileBytes;
                        }
                        else
                        {
                            // Si no tocó nada de la foto, mantiene la que ya venía de la Base de Datos
                            usuario.Imagen = usuarioDB.Imagen;
                        }

                        usuario.Rol = new Rol();
                        usuario.Rol.IdRol = ddlRol.SelectedValue != "0" ? Convert.ToInt32(ddlRol.SelectedValue) : usuarioDB.Rol.IdRol;

                        if (ddlMedico.SelectedValue != "0")
                        {
                            usuario.Medico = new Medico();
                            usuario.Medico.IdMedico = Convert.ToInt32(ddlMedico.SelectedValue);
                        }
                        else
                        {
                            usuario.Medico = usuarioDB.Medico;
                        }
                    }

                    negocio.Modificar(usuario);

                    Session["ImagenTemporal"] = null;

                    if (!string.IsNullOrEmpty(txtPassword.Text))
                    {
                        string mensaje = "Contraseña actualizada correctamente.";
                        string script = $"alert('{mensaje}'); window.location='ListaUsuarios.aspx';";
                        ScriptManager.RegisterClientScriptBlock((Control)sender, sender.GetType(), "alertPassword", script, true);
                        return;
                    }
                }
                else
                {
                    usuario.Activo = true;
                    usuario.Nombre = txtNombre.Text.Trim();
                    usuario.Apellido = txtApellido.Text.Trim();
                    usuario.NombreUsuario = txtNombreUsuario.Text.Trim();
                    usuario.Email = txtEmail.Text.Trim();
                    usuario.PasswordHash = txtPassword.Text;

                    if (Session["ImagenTemporal"] != null)
                    {
                        usuario.Imagen = (byte[])Session["ImagenTemporal"];
                    }
                    else if (fileImagen.HasFile)
                    {
                        usuario.Imagen = fileImagen.FileBytes;
                    }

                    usuario.Rol = new Rol { IdRol = Convert.ToInt32(ddlRol.SelectedValue) };

                    if (ddlMedico.SelectedValue != "0")
                    {
                        usuario.Medico = new Medico { IdMedico = Convert.ToInt32(ddlMedico.SelectedValue) };
                    }

                    negocio.Agregar(usuario);

                    Session["ImagenTemporal"] = null;
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
        protected void btnPrevisualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (fileImagen.HasFile)
                {
                    // Guardamos los bytes en la Session para que no se pierdan en los PostBacks
                    Session["ImagenTemporal"] = fileImagen.FileBytes;

                    // Mostramos la imagen en el control de servidor convirtiendo los bytes a Base64
                    imgPerfil.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(fileImagen.FileBytes);

                    // Hacemos visible la foto y ocultamos el panel de la inicial
                    imgPerfil.Visible = true;
                    pnlInicial.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Session.Add("Error", ex.ToString());
            }
        }

        private void CargarDesplegables()
        {
            var roles = Enum.GetValues(typeof(TurnosClinica.Dominio.Enums.RolEnum))
                            .Cast<TurnosClinica.Dominio.Enums.RolEnum>()
                            .Select(r => new
                            {
                                Id = (int)r,
                                Nombre = r.ToString()
                            }).ToList();

            ddlRol.DataSource = roles;
            ddlRol.DataValueField = "Id";
            ddlRol.DataTextField = "Nombre";
            ddlRol.DataBind();
            ddlRol.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccione un Rol", "0"));

            MedicoNegocio medicoNegocio = new MedicoNegocio();
            ddlMedico.DataSource = medicoNegocio.Listar();
            ddlMedico.DataValueField = "IdMedico";
            ddlMedico.DataTextField = "Apellido";
            ddlMedico.DataBind();
            ddlMedico.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccione un Medico", "0"));
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
                        if (usuario.Activo == true)
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