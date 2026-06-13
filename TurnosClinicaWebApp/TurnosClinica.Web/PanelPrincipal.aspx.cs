using System;
using System.Web.UI;

namespace TurnosClinica.Web
{
    public partial class PanelPrincipal : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void LnkAltaPaciente_Click(object sender, EventArgs e)
        {
            Response.Redirect("Pacientes/FormularioPaciente.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void LnkListaPacientes_Click(object sender, EventArgs e)
        {
            Response.Redirect("Pacientes/ListaPacientes.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void LnkAltaMedico_Click(object sender, EventArgs e)
        {
            Response.Redirect("Medicos/FormularioMedico.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void LnkAltaEspecialidad_Click(object sender, EventArgs e)
        {
            Response.Redirect("Especialidades/FormularioEspecialidad.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void LnkListaEspecialidades_Click(object sender, EventArgs e)
        {
            Response.Redirect("Especialidades/ListaEspecialidades.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void LnkAltaUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect("Usuarios/FormularioUsuario.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void LnkListaUsuarios_Click(object sender, EventArgs e)
        {
            Response.Redirect("Usuarios/ListaUsuarios.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void LnkConfiguracionTurnos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Turnos/ConfiguracionTurnos.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void LnkAltaTurno_Click(object sender, EventArgs e)
        {
            Response.Redirect("Turnos/FormularioTurno.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void LnkMisTurnos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Turnos/MisTurnos.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
