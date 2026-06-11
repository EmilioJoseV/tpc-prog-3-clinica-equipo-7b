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

        protected void BtnPacientes_Click(object sender, EventArgs e)
        {
            Response.Redirect("Pacientes/ListaPacientes.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnMedicos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Medicos/ListaMedicos.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnEspecialidades_Click(object sender, EventArgs e)
        {
            Response.Redirect("Especialidades/ListaEspecialidades.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnUsuarios_Click(object sender, EventArgs e)
        {
            Response.Redirect("Usuarios/ListaUsuarios.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnTurnos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Turnos/ListaTurnos.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnDetalleTurno_Click(object sender, EventArgs e)
        {
            Response.Redirect("Turnos/DetalleTurno.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnMisTurnos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Turnos/MisTurnos.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
