using System;
using System.Collections.Generic;
using System.Web.UI;

namespace TurnosClinica.Web
{
    public partial class Inicio : Page
    {
        private static readonly IReadOnlyList<InicioResumenItem> Resumen = new[]
        {
            new InicioResumenItem
            {
                Titulo = "Turnos",
                Descripcion = "Alta, reprogramacion y seguimiento de turnos de la clinica."
            },
            new InicioResumenItem
            {
                Titulo = "Pacientes",
                Descripcion = "Gestionar datos personales y contacto para gestionar la atencion."
            },
            new InicioResumenItem
            {
                Titulo = "Medicos y especialidades",
                Descripcion = "Gestionar y elegir diferentes medicos y especialidades."
            }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RptResumen.DataSource = Resumen;
                RptResumen.DataBind();
            }
        }

        private class InicioResumenItem
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
        }
    }
}
