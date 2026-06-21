using System;
using System.Collections.Generic;
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
                if (Request.QueryString["idBaja"] != null)
                {
                    int idEliminar = int.Parse(Request.QueryString["idBaja"]);

                    negocio.Eliminar(idEliminar);
                }


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
        public string ObtenerMedicosPorEspecialidad(int idEspecialidad)
        {
            MedicoNegocio medicoNegocio = new MedicoNegocio();
            List<Medico> todosLosMedicos = medicoNegocio.Listar();

            //  Filtro los médicos. 
            // Como un médico tiene una lista con Exists para buscar si dentro de sus especialidades está la que busco
            var medicosAsociados = todosLosMedicos.FindAll(m =>
                m.Especialidades != null &&
                m.Especialidades.Exists(esp => esp.IdEspecialidad == idEspecialidad)
            );

            if (medicosAsociados.Count > 0)
            {
                // uni los nombres
                List<string> nombres = new List<string>();
                foreach (var med in medicosAsociados)
                {
                    nombres.Add(med.Nombre + " " + med.Apellido);
                }
                return string.Join(", ", nombres);
            }
            else
            {
                return "Sin médicos asociados";
            }
        }

    }
}
