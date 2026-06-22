using System;
using System.Collections.Generic;
using TurnosClinica.Dominio.Entidades;
using System.Web.UI.WebControls;
using TurnosClinica.Negocio;
using TurnosClinica.Web;
using System.Web.UI;

namespace TurnosClinica.Web
{
    public partial class ListaEspecialidades : Page
    {
        
        public List<Especialidad> ListaEspecialidad { get; set; }
        public bool FiltroAvanzado { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            FiltroAvanzado = chkAvanzado.Checked;
            if (!IsPostBack)
            {
                
                EspecialidadNegocio negocio = new EspecialidadNegocio();
                if (Request.QueryString["idBaja"] != null)
                {
                    int idEliminar = int.Parse(Request.QueryString["idBaja"]);

                    negocio.Desactivar(idEliminar);
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
        protected void chkAvanzado_CheckedChanged(object sender, EventArgs e)
        {
            FiltroAvanzado = chkAvanzado.Checked;
            txtFiltro.Enabled = !FiltroAvanzado;

                        if (FiltroAvanzado)
            {
                List<Especialidad> lista = (List<Especialidad>)Session["listaEspecialidades"];
                ddlFiltroNombre.DataSource = lista;
                ddlFiltroNombre.DataTextField = "Nombre"; 
                ddlFiltroNombre.DataValueField = "IdEspecialidad"; 
                ddlFiltroNombre.DataBind();
                ddlFiltroNombre.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todas las especialidades", "0"));
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            List<Especialidad> listaOriginal = (List<Especialidad>)Session["listaEspecialidades"];
            List<Especialidad> filtrada = listaOriginal;

                        string nombreElegido = ddlFiltroNombre.SelectedItem.Text;
            if (nombreElegido != "Todas las especialidades")
            {
                filtrada = filtrada.FindAll(x => x.Nombre == nombreElegido);
            }

                        string estado = ddlEstado.SelectedItem.ToString();
            if (estado == "Activo")
            {
                filtrada = filtrada.FindAll(x => x.Activo == true);
            }
            else if (estado == "Inactivo")
            {
                filtrada = filtrada.FindAll(x => x.Activo == false);
            }

            ListaEspecialidad = filtrada;
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            ddlFiltroNombre.SelectedIndex = 0; 
            ddlEstado.SelectedIndex = 0; 

            ListaEspecialidad = (List<Especialidad>)Session["listaEspecialidades"];
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

