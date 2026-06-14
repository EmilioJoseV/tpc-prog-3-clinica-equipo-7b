using System;
using System.Web.UI.WebControls;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Web
{
    public partial class FormularioUsuario : System.Web.UI.Page
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
                            txtNombreUsuario.Text = usuario.NombreUsuario;
                            txtEmail.Text = usuario.Email;
                            txtPassword.Text = usuario.PasswordHash;
                            chkActivo.Checked = usuario.Activo;

                            if (usuario.Rol != null)
                                ddlRol.SelectedValue = usuario.Rol.IdRol.ToString();

                            if (usuario.Medico != null)
                                ddlMedico.SelectedValue = usuario.Medico.IdMedico.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Session.Add("Error", ex.ToString());
            }
        }

        private void CargarDesplegables()
        {
            try
            {
                ddlRol.Items.Clear();

                foreach (string nombreRol in Enum.GetNames(typeof(RolEnum)))
                {
                    int valorRol = (int)Enum.Parse(typeof(RolEnum), nombreRol);

                    ListItem item = new ListItem(nombreRol, (valorRol + 1).ToString());
                    ddlRol.Items.Add(item);
                }

                ddlRol.Items.Insert(0, new ListItem("-- Seleccione un Rol --", "0"));

                ddlMedico.Items.Clear();

                ddlMedico.Items.Insert(0, new ListItem("-- No asignado (No es médico) --", "0"));
            }
            catch (Exception ex)
            {
                throw ex;
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
                Usuario nuevoUsuario = new Usuario();

                nuevoUsuario.NombreUsuario = txtNombreUsuario.Text;
                nuevoUsuario.Email = txtEmail.Text;
                nuevoUsuario.PasswordHash = txtPassword.Text;
                nuevoUsuario.Activo = chkActivo.Checked;

                nuevoUsuario.Rol = new Rol { IdRol = int.Parse(ddlRol.SelectedValue) };

                if (ddlMedico.SelectedValue != "0")
                {
                    nuevoUsuario.Medico = new Medico { IdMedico = int.Parse(ddlMedico.SelectedValue) };
                }
                else
                {
                    nuevoUsuario.Medico = null;
                }
                negocio.Agregar(nuevoUsuario);

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
    }
}