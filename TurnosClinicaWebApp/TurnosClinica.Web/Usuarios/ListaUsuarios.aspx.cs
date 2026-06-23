using System;
using System.Collections.Generic;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class ListaUsuarios : Page
    {
        // 1. Usamos una variable privada de respaldo para asegurar que jamás devuelva null
        private List<Usuario> _listaUsuarios;

        public List<Usuario> ListaUsuariosProp
        {
            get
            {
                if (_listaUsuarios == null)
                    _listaUsuarios = new List<Usuario>();
                return _listaUsuarios;
            }
            set { _listaUsuarios = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListaUsuariosProp = new List<Usuario>();

                try
                {
                    UsuarioNegocio negocio = new UsuarioNegocio();
                    List<Usuario> listaAux = negocio.ListarTodos();

                    if (listaAux != null)
                    {
                        ListaUsuariosProp = listaAux;
                    }
                }
                catch (Exception ex)
                {
                    Session.Add("Error", ex.Message);

                    System.Diagnostics.Debug.WriteLine("ERROR EN BASE DE DATOS: " + ex.ToString());
                }
            }
        }
    }
}