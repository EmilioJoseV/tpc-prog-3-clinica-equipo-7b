using System;
using System.Collections.Generic;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class ListaEspecialidades : System.Web.UI.Page
    {
        
        public List<Especialidad> ListaEspecialidad { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();

            try
            {
                if (!IsPostBack)
                {
                    
                    ListaEspecialidad = negocio.Listar(true);
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
            }
        }
    }
}
