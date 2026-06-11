using System;
using System.Collections.Generic;
using System.Web.UI;

namespace TurnosClinica.Web
{
    public partial class Inicio : Page
    {
        private static readonly IReadOnlyList<object> Resumen = new[]
        {
            new { Titulo = "Turnos", Descripcion = "Alta, reprogramacion y seguimiento de turnos de la clinica." },
            new { Titulo = "Pacientes", Descripcion = "Gestionar datos personales y contacto para gestionar la atencion." },
            new { Titulo = "Medicos y especialidades", Descripcion = "Gestionar y eliger difentes medicos y especialidades." }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RptResumen.DataSource = Resumen;
                RptResumen.DataBind();
            }
        }

    }
}
