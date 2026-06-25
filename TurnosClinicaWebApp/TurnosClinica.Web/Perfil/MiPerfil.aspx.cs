using System;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades; 

namespace TurnosClinica.Web
{
    public partial class MiPerfil : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                
                Usuario user = (Usuario)Session["UsuarioActual"];
                
                if (txtImagen.PostedFile.FileName != "")
                {
                    
                    string ruta = Server.MapPath("~/Images/Perfiles/");                  
                    txtImagen.PostedFile.SaveAs(ruta + "perfil-" + user.IdUsuario + ".jpg");
                    imgNuevoPerfil.ImageUrl = "~/Images/Perfiles/perfil-" + user.IdUsuario + ".jpg";
                }
               
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
            }


        }
    }
}
