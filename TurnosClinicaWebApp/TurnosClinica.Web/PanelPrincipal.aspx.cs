using System;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Web
{
    public partial class PanelPrincipal : Page
    {
        private const string RutaPacientesFormulario = "~/Pacientes/FormularioPaciente.aspx";
        private const string RutaPacientesLista = "~/Pacientes/ListaPacientes.aspx";
        private const string RutaMedicosFormulario = "~/Medicos/FormularioMedico.aspx";
        private const string RutaMedicosLista = "~/Medicos/ListaMedicos.aspx";
        private const string RutaEspecialidadesFormulario = "~/Especialidades/FormularioEspecialidad.aspx";
        private const string RutaEspecialidadesLista = "~/Especialidades/ListaEspecialidades.aspx";
        private const string RutaUsuariosFormulario = "~/Usuarios/FormularioUsuario.aspx";
        private const string RutaUsuariosLista = "~/Usuarios/ListaUsuarios.aspx";
        private const string RutaConfiguracionTurnos = "~/Turnos/ConfiguracionTurnos.aspx";
        private const string RutaTurnosFormulario = "~/Turnos/FormularioTurno.aspx";
        private const string RutaTurnosLista = "~/Turnos/ListaTurnos.aspx";
        private const string RutaMisTurnos = "~/Turnos/MisTurnos.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Session["UsuarioActual"] is Usuario usuario) || usuario.IdUsuario <= 0)
            {
                Response.Redirect("Ingresar.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                LblBienvenida.Text = "Hola, " + ObtenerNombre(usuario);
            }

            ConfigurarAccesos(usuario);
        }

        protected void LnkAltaPaciente_Click(object sender, EventArgs e)
        {
            AbrirPagina(RutaPacientesFormulario);
        }

        protected void LnkListaPacientes_Click(object sender, EventArgs e)
        {
            AbrirPagina(RutaPacientesLista);
        }

        protected void LnkAltaMedico_Click(object sender, EventArgs e)
        {
            AbrirPagina(RutaMedicosFormulario);
        }

        protected void LnkListaMedicos_Click(object sender, EventArgs e)
        {
            AbrirPagina(RutaMedicosLista);
        }

        protected void LnkAltaEspecialidad_Click(object sender, EventArgs e)
        {
            AbrirPagina(RutaEspecialidadesFormulario);
        }

        protected void LnkListaEspecialidades_Click(object sender, EventArgs e)
        {
            AbrirPagina(RutaEspecialidadesLista);
        }

        protected void LnkAltaUsuario_Click(object sender, EventArgs e)
        {
            AbrirPagina(RutaUsuariosFormulario);
        }

        protected void LnkListaUsuarios_Click(object sender, EventArgs e)
        {
            AbrirPagina(RutaUsuariosLista);
        }

        protected void LnkConfiguracionTurnos_Click(object sender, EventArgs e)
        {
            AbrirPagina(RutaConfiguracionTurnos);
        }

        protected void LnkAltaTurno_Click(object sender, EventArgs e)
        {
            AbrirPagina(RutaTurnosFormulario);
        }

        protected void LnkMisTurnos_Click(object sender, EventArgs e)
        {
            AbrirPagina(RutaMisTurnos);
        }

        protected void LnkListaTurnos_Click(object sender, EventArgs e)
        {
            AbrirPagina(RutaTurnosLista);
        }

        private void ConfigurarAccesos(Usuario usuario)
        {
            ConfigurarAcceso(LnkAltaPaciente, usuario, RutaPacientesFormulario);
            ConfigurarAcceso(LnkListaPacientes, usuario, RutaPacientesLista);
            CardPacientes.Visible = LnkAltaPaciente.Visible || LnkListaPacientes.Visible;

            ConfigurarAcceso(LnkAltaMedico, usuario, RutaMedicosFormulario);
            ConfigurarAcceso(LnkListaMedicos, usuario, RutaMedicosLista);
            CardMedicos.Visible = LnkAltaMedico.Visible || LnkListaMedicos.Visible;

            ConfigurarAcceso(LnkAltaTurno, usuario, RutaTurnosFormulario);
            ConfigurarAcceso(LnkListaTurnos, usuario, RutaTurnosLista);
            ConfigurarAcceso(LnkMisTurnos, usuario, RutaMisTurnos);
            CardTurnos.Visible = LnkAltaTurno.Visible || LnkListaTurnos.Visible || LnkMisTurnos.Visible;

            ConfigurarAcceso(LnkAltaEspecialidad, usuario, RutaEspecialidadesFormulario);
            ConfigurarAcceso(LnkListaEspecialidades, usuario, RutaEspecialidadesLista);
            CardEspecialidades.Visible = LnkAltaEspecialidad.Visible || LnkListaEspecialidades.Visible;

            ConfigurarAcceso(LnkAltaUsuario, usuario, RutaUsuariosFormulario);
            ConfigurarAcceso(LnkListaUsuarios, usuario, RutaUsuariosLista);
            CardUsuarios.Visible = LnkAltaUsuario.Visible || LnkListaUsuarios.Visible;

            ConfigurarAcceso(LnkConfiguracionTurnos, usuario, RutaConfiguracionTurnos);
            CardConfiguracion.Visible = LnkConfiguracionTurnos.Visible;
        }

        private string ObtenerNombre(Usuario usuario)
        {
            if (usuario != null && usuario.Persona != null)
            {
                string nombre = (usuario.Persona.Nombre + " " + usuario.Persona.Apellido).Trim();
                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    return nombre;
                }
            }

            return usuario.NombreUsuario;
        }

        private void ConfigurarAcceso(System.Web.UI.WebControls.WebControl control, Usuario usuario, string ruta)
        {
            control.Visible = AutorizacionRutasService.UsuarioPuedeAccederRuta(usuario, ruta);
        }

        private void AbrirPagina(string ruta)
        {
            Usuario usuario = Session["UsuarioActual"] as Usuario;
            if (!AutorizacionRutasService.UsuarioPuedeAccederRuta(usuario, ruta))
            {
                throw new Exception("No tiene permisos para ejecutar esta accion.");
            }

            Response.Redirect(ruta, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
