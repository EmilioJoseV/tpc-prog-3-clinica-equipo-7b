using System;
using System.Linq;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
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
                            CargarDesplegables();

                            lblTitulo.InnerText = "Modificar Usuario";
                            txtNombre.Text = usuario.Nombre;
                            txtApellido.Text = usuario.Apellido;
                            txtNombreUsuario.Text = usuario.NombreUsuario;
                            txtEmail.Text = usuario.Email;
                            txtPassword.Text = usuario.PasswordHash;

                            if (usuario.Rol != null)
                            {
                                ddlRol.SelectedValue = usuario.Rol.IdRol.ToString();
                            }

                            if (usuario.Medico != null)
                            {
                                ddlMedico.SelectedValue = usuario.Medico.IdMedico.ToString();
                            }
                        }

                        // ESTO SE EJECUTA SIEMPRE (En la primera carga y en cada PostBack)
                        btnInactivar.Visible = true;
                        btnEliminar.Visible = true;

                        if (usuario.Activo == true)
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
                        CargarDesplegables();
                        lblTitulo.InnerText = "Nuevo Usuario";
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
                if (ddlRol.SelectedValue == "0")
                {
                    return;
                }

                UsuarioNegocio negocio = new UsuarioNegocio();
                Usuario usuario = new Usuario();

                if (Request.QueryString["id"] != null)
                {
                    usuario.IdUsuario = Convert.ToInt32(Request.QueryString["id"]);

                    // Si modificamos, mantenemos el estado Activo que ya tenía en la DB
                    Usuario usuarioExistente = negocio.ObtenerPorId(usuario.IdUsuario);
                    if (usuarioExistente != null)
                    {
                        usuario.Activo = usuarioExistente.Activo;
                    }
                }
                else
                {
                    // Si es un usuario nuevo, forzamos que nazca ACTIVO (true)
                    usuario.Activo = true;
                }

                usuario.Nombre = txtNombre.Text.Trim();
                usuario.Apellido = txtApellido.Text.Trim();
                usuario.NombreUsuario = txtNombreUsuario.Text.Trim();
                usuario.Email = txtEmail.Text.Trim();
                usuario.PasswordHash = txtPassword.Text;

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
                usuario.Rol.IdRol = Convert.ToInt32(ddlRol.SelectedValue);

                if (ddlMedico.SelectedValue != "0")
                {
                    usuario.Medico = new Medico();
                    usuario.Medico.IdMedico = Convert.ToInt32(ddlMedico.SelectedValue);
                }
                else
                {
                    usuario.Medico = null;
                }

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
            ddlMedico.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccione un Médico", "0"));
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