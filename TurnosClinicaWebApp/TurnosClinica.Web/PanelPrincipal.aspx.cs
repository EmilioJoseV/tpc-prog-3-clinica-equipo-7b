using System;
using System.Web.UI;

namespace TurnosClinica.Web
{
    public partial class PanelPrincipal : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnInicio_Click(object sender, EventArgs e)
        {
            Response.Redirect("Inicio.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnPacientes_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionPacientes.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnMedicos_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionMedicos.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnEspecialidades_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionEspecialidades.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnHorariosMedicos_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionHorariosMedicos.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnUsuarios_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionUsuarios.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnRoles_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionRoles.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnConfiguracionTurnos_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConfiguracionTurnos.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnTurnos_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionTurnos.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnDetalleTurno_Click(object sender, EventArgs e)
        {
            Response.Redirect("DetalleTurno.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnMisTurnos_Click(object sender, EventArgs e)
        {
            Response.Redirect("MisTurnos.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnReportes_Click(object sender, EventArgs e)
        {
            Response.Redirect("Reportes.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
