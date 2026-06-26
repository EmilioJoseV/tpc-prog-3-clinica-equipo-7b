using System;
using System.Web.UI;
using System.IO;
using TurnosClinica.Dominio.Entidades; 

namespace TurnosClinica.Web
{
    public partial class MiPerfil : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                // ver si la sesion no caduco
                if (Session["UsuarioActual"] != null)
                {
                    Usuario user = (Usuario)Session["UsuarioActual"];
                    
                    if (user.Persona != null)
                    {
                        txtEmail.Text = user.Persona.Email;
                        txtNombre.Text = user.Persona.Nombre;
                        txtApellido.Text = user.Persona.Apellido;//nota a ver
                        
                    }
                    
                    string rutaImagen = Server.MapPath("~/Images/Perfiles/perfil-" + user.IdUsuario + ".jpg");
                    if (File.Exists(rutaImagen))
                    {
                        imgNuevoPerfil.ImageUrl = "~/Images/Perfiles/perfil-" + user.IdUsuario + ".jpg";
                    }
                    else
                    {
                        
                        imgNuevoPerfil.ImageUrl = "https://www.palomacornejo.com/wp-content/uploads/2021/08/no-image.jpg";
                    }
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario user = (Usuario)Session["UsuarioActual"];

                if (user != null)
                { 
                    if (txtImagen.PostedFile != null && txtImagen.PostedFile.FileName != "")
                    {
                        string ruta = Server.MapPath("~/Images/Perfiles/");
                        txtImagen.PostedFile.SaveAs(ruta + "perfil-" + user.IdUsuario + ".jpg");
                        imgNuevoPerfil.ImageUrl = "~/Images/Perfiles/perfil-" + user.IdUsuario + ".jpg";
                    }

                
                    if (user.Persona != null)
                    {
                        user.Persona.Email = txtEmail.Text;
                        user.Persona.Nombre = txtNombre.Text;
                        user.Persona.Apellido = txtApellido.Text;

                        TurnosClinica.Negocio.PersonaNegocio personaNegocio = new TurnosClinica.Negocio.PersonaNegocio();
                        personaNegocio.Modificar(user.Persona);

                        Session["UsuarioActual"] = user;
                    }
                }
                else
                {
                    Response.Redirect("~/Ingresar.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
            }
        }


    }
}
