using System;
using System.Collections.Generic;
// Tus usings normales:
using TurnosClinica.Negocio;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Web
{
    public partial class ListaEspecialidades : System.Web.UI.Page
    {
        
        public List<Especialidad> ListaEspecialidad { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                EspecialidadNegocio negocio = new EspecialidadNegocio();

                
                Session.Add("listaEspecialidades", negocio.Listar(true));
            }

            
            ListaEspecialidad = (List<Especialidad>)Session["listaEspecialidades"];
        }

        
        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            // Recupero la lista de la Session
            List<Especialidad> listaOriginal = (List<Especialidad>)Session["listaEspecialidades"];

            // lista con findall 
            List<Especialidad> listaFiltrada = listaOriginal.FindAll(x =>
                x.Nombre.ToUpper().Contains(txtFiltro.Text.ToUpper()));

            ListaEspecialidad = listaFiltrada;
        }
    }
}
