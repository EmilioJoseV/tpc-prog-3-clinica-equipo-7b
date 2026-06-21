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
                // Blindamos el QueryString para que acepte tanto "id" como "Id"
                if (Request.QueryString["id"] != null || Request.QueryString["Id"] != null)
                {
                    string idUrl = Request.QueryString["id"] ?? Request.QueryString["Id"];
                    int idUsuario = Convert.ToInt32(idUrl);

                    UsuarioNegocio negocio = new UsuarioNegocio();

                    // Traemos el usuario para que la propiedad pública esté disponible para la Foto en la vista
                    usuarioActual = negocio.ObtenerPorId(idUsuario);

                    if (usuarioActual != null)
                    {
                        // CRUCIAL: Los controles del formulario SOLO se enteran de la DB la PRIMERA VEZ
                        if (!IsPostBack)
                        {
                            CargarDesplegables();

                            // Borramos la línea de lblTitulo de acá porque ya lo maneja el HTML
                            txtNombre.Text = usuarioActual.Nombre;
                            txtApellido.Text = usuarioActual.Apellido;
                            txtNombreUsuario.Text = usuarioActual.NombreUsuario;
                            txtEmail.Text = usuarioActual.Email;
                            txtPassword.Text = string.Empty;

                            if (usuarioActual.Rol != null)
                            {
                                ddlRol.SelectedValue = usuarioActual.Rol.IdRol.ToString();
                            }

                            if (usuarioActual.Medico != null)
                            {
                                ddlMedico.SelectedValue = usuarioActual.Medico.IdMedico.ToString();
                            }
                        }

                        // Esto se ejecuta siempre (Carga inicial y PostBack) para mantener los botones visibles
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
                    // SI ES UN ALTA NUEVA
                    if (!IsPostBack)
                    {
                        CargarDesplegables();
                        // Borramos la línea de lblTitulo de acá también
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
                    return; // Validación de rol obligatorio
                }

                UsuarioNegocio negocio = new UsuarioNegocio();
                Usuario usuario = new Usuario();

                // CASO: MODIFICACIÓN (Blindamos el QueryString para que acepte tanto "id" como "Id")
                if (Request.QueryString["id"] != null || Request.QueryString["Id"] != null)
                {
                    // Capturamos el ID de forma segura sin importar cómo venga en la URL
                    string idUrl = Request.QueryString["id"] ?? Request.QueryString["Id"];
                    int idUsuario = Convert.ToInt32(idUrl);

                    // 1. Traemos el estado REAL y ACTUAL de la Base de Datos
                    Usuario usuarioDB = negocio.ObtenerPorId(idUsuario);

                    if (usuarioDB != null)
                    {
                        usuario.IdUsuario = usuarioDB.IdUsuario;
                        usuario.Activo = usuarioDB.Activo; // Mantiene el estado activo/inactivo de la DB

                        // 2. COMPARACIÓN CAMPO POR CAMPO:

                        // NOMBRE: Si cambió en la pantalla, lo toma. Si no, mantiene el de la DB
                        usuario.Nombre = !string.IsNullOrWhiteSpace(txtNombre.Text) ? txtNombre.Text.Trim() : usuarioDB.Nombre;

                        // APELLIDO:
                        usuario.Apellido = !string.IsNullOrWhiteSpace(txtApellido.Text) ? txtApellido.Text.Trim() : usuarioDB.Apellido;

                        // NOMBRE DE USUARIO:
                        usuario.NombreUsuario = !string.IsNullOrWhiteSpace(txtNombreUsuario.Text) ? txtNombreUsuario.Text.Trim() : usuarioDB.NombreUsuario;

                        // EMAIL:
                        usuario.Email = !string.IsNullOrWhiteSpace(txtEmail.Text) ? txtEmail.Text.Trim() : usuarioDB.Email;

                        // CONTRASEÑA: Si escribió algo, se cambia. Si vino vacío, mantiene el Hash original
                        usuario.PasswordHash = !string.IsNullOrEmpty(txtPassword.Text) ? txtPassword.Text : usuarioDB.PasswordHash;

                        // FOTO DE PERFIL (Imagen): Si subió un archivo nuevo, lo lee. Si no, mantiene los bytes viejos
                        if (fileImagen.HasFile)
                        {
                            usuario.Imagen = fileImagen.FileBytes;
                        }
                        else
                        {
                            usuario.Imagen = usuarioDB.Imagen;
                        }

                        // ROL ASIGNADO: Si eligió uno válido (distinto de 0), lo actualiza. Si no, deja el de la DB
                        usuario.Rol = new Rol();
                        usuario.Rol.IdRol = ddlRol.SelectedValue != "0" ? Convert.ToInt32(ddlRol.SelectedValue) : usuarioDB.Rol.IdRol;

                        // MEDICO ASOCIADO:
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

                    // Ejecuta la modificación segura en la base de datos
                    negocio.Modificar(usuario);

                    // LOGICA DEL CARTEL: Si modificó la clave, avisa y redirige por script
                    if (!string.IsNullOrEmpty(txtPassword.Text))
                    {
                        string mensaje = "Contraseña actualizada correctamente.";
                        string script = $"alert('{mensaje}'); window.location='ListaUsuarios.aspx';";
                        ScriptManager.RegisterClientScriptBlock((Control)sender, sender.GetType(), "alertPassword", script, true);
                        return;
                    }
                }
                // CASO: ALTA (Usuario Nuevo)
                else
                {
                    usuario.Activo = true; // Todo usuario nuevo nace activo
                    usuario.Nombre = txtNombre.Text.Trim();
                    usuario.Apellido = txtApellido.Text.Trim();
                    usuario.NombreUsuario = txtNombreUsuario.Text.Trim();
                    usuario.Email = txtEmail.Text.Trim();
                    usuario.PasswordHash = txtPassword.Text; // Acá sí es obligatoria la primera vez

                    if (fileImagen.HasFile)
                        usuario.Imagen = fileImagen.FileBytes;

                    usuario.Rol = new Rol { IdRol = Convert.ToInt32(ddlRol.SelectedValue) };

                    if (ddlMedico.SelectedValue != "0")
                    {
                        usuario.Medico = new Medico { IdMedico = Convert.ToInt32(ddlMedico.SelectedValue) };
                    }

                    negocio.Agregar(usuario);
                }

                // Redirección normal para Altas o Modificaciones sin cambio de clave
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