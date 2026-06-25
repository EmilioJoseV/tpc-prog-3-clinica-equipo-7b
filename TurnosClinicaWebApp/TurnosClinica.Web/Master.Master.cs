using System;
using System.Web.UI;
using System.IO;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Web
{
    public partial class MasterLayout : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool usuarioAutenticado = Session["UsuarioActual"] != null;
            LnkPanelPrincipal.Visible = usuarioAutenticado;
            LnkIngresar.Visible = !usuarioAutenticado;
            LnkSalir.Visible = usuarioAutenticado;
            imgAvatar.Visible = usuarioAutenticado;

            if (usuarioAutenticado)
            {
                Usuario user = (Usuario)Session["UsuarioActual"];

                //para tenr la ruta fisica
                string rutaImagen = Server.MapPath("~/Images/Perfiles/perfil-" + user.IdUsuario + ".jpg");

               
                if (File.Exists(rutaImagen))
                {
                    // Si existe le voy a  mostrar la foto
                    imgAvatar.ImageUrl = "~/Images/Perfiles/perfil-" + user.IdUsuario + ".jpg";
                }
                else
                {
                    imgAvatar.ImageUrl = "https://simg.nicepng.com/png/small/202-2022264_usuario-annimo-usuario-anpnimo-user-icon-png-transparent.png";
                }
            }
        }

        protected void LnkSalir_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Ingresar.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        public void MostrarError(string mensaje)
        {
            string mensajeVisible = string.IsNullOrWhiteSpace(mensaje)
                ? "Ocurrio un error inesperado."
                : mensaje;
            LblMensajeError.Text = Server.HtmlEncode(mensajeVisible);
            PnlMensajeError.Visible = true;
        }
    }
}
