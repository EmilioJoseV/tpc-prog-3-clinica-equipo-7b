using System;
using System.Collections.Generic;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class ListaUsuarios : Page
    {
        public List<Usuario> ListaUsuariosProp { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();

                ListaUsuariosProp = negocio.ListarTodos();

                if (ListaUsuariosProp == null)
                {
                    ListaUsuariosProp = new List<Usuario>();
                }
            }
            catch (Exception ex)
            {
                Session.Add("Error", ex.ToString());
            }
        }
    }
}