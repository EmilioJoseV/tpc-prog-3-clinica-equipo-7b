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
                if (!IsPostBack)
                {
                    CargarDesplegables();

                    if (Request.QueryString["id"] != null)
                    {
                        int idUsuario = int.Parse(Request.QueryString["id"]);
                        UsuarioNegocio negocio = new UsuarioNegocio();
                        Usuario usuario = negocio.ObtenerPorId(idUsuario);

                        if (usuario != null)
                        {
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

                            if (usuario.Activo)
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
                        btnInactivar.Text = "Inactivar";
                        btnInactivar.CssClass = "btn btn-warning";
                    }
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
                    usuario.IdUsuario = int.Parse(Request.QueryString["id"]);

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

                usuario.Rol = new Rol { IdRol = int.Parse(ddlRol.SelectedValue) };

                if (ddlMedico.SelectedValue != "0")
                    usuario.Medico = new Medico { IdMedico = int.Parse(ddlMedico.SelectedValue) };
                else
                    usuario.Medico = null;

                if (usuario.IdUsuario > 0)
                {
                    negocio.Modificar(usuario);
                }
                else
                {
                    negocio.Agregar(usuario);
                }

                Response.Redirect("ListaUsuarios.aspx", false);
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
                            usuario.Activo = false;
                        }
                        else
                        {
                            usuario.Activo = true;
                        }

                        negocio.Modificar(usuario);

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