using System;
using System.Collections.Generic;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class ListaUsuarios : System.Web.UI.Page
    {
        public List<Usuario> ListaUsuariosProp { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ListaUsuariosProp = new List<Usuario>();

            if (!IsPostBack)
            {
                try
                {
                    UsuarioNegocio negocio = new UsuarioNegocio();

                    ListaUsuariosProp = negocio.ListarTodos();
                }
                catch (Exception ex)
                {
                    Session.Add("Error", ex.ToString());
                }
            }
        }
    }
}