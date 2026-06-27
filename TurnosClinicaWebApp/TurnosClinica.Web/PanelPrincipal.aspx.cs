using System;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.Web
{
    public partial class PanelPrincipal : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Session["UsuarioActual"] is Usuario usuario) || usuario.IdUsuario <= 0)
            {
                Response.Redirect("Ingresar.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            ConfigurarAccesos(usuario);
        }

        protected void LnkAltaPaciente_Click(object sender, EventArgs e)
        {
            AbrirPagina("Pacientes/FormularioPaciente.aspx", RolEnum.Administrador, RolEnum.Recepcionista);
        }

        protected void LnkListaPacientes_Click(object sender, EventArgs e)
        {
            AbrirPagina("Pacientes/ListaPacientes.aspx", RolEnum.Administrador, RolEnum.Recepcionista);
        }

        protected void LnkAltaMedico_Click(object sender, EventArgs e)
        {
            AbrirPagina("Medicos/FormularioMedico.aspx", RolEnum.Administrador, RolEnum.Recepcionista);
        }

        protected void LnkAltaEspecialidad_Click(object sender, EventArgs e)
        {
            AbrirPagina("Especialidades/FormularioEspecialidad.aspx", RolEnum.Administrador);
        }

        protected void LnkListaEspecialidades_Click(object sender, EventArgs e)
        {
            AbrirPagina("Especialidades/ListaEspecialidades.aspx", RolEnum.Administrador);
        }

        protected void LnkAltaUsuario_Click(object sender, EventArgs e)
        {
            AbrirPagina("Usuarios/FormularioUsuario.aspx", RolEnum.Administrador);
        }

        protected void LnkListaUsuarios_Click(object sender, EventArgs e)
        {
            AbrirPagina("Usuarios/ListaUsuarios.aspx", RolEnum.Administrador);
        }

        protected void LnkConfiguracionTurnos_Click(object sender, EventArgs e)
        {
            AbrirPagina("Turnos/ConfiguracionTurnos.aspx", RolEnum.Administrador);
        }

        protected void LnkAltaTurno_Click(object sender, EventArgs e)
        {
            AbrirPagina("Turnos/FormularioTurno.aspx", RolEnum.Administrador, RolEnum.Recepcionista);
        }

        protected void LnkMisTurnos_Click(object sender, EventArgs e)
        {
            AbrirPagina("Turnos/MisTurnos.aspx", RolEnum.Medico);
        }

        protected void LnkListaTurnos_Click(object sender, EventArgs e)
        {
            AbrirPagina("Turnos/ListaTurnos.aspx", RolEnum.Administrador, RolEnum.Recepcionista);
        }

        private void ConfigurarAccesos(Usuario usuario)
        {
            bool esAdministrador = AutorizacionRutasService.EsAdministrador(usuario);
            bool esRecepcionista = AutorizacionRutasService.EsRecepcionista(usuario);
            bool esMedico = AutorizacionRutasService.EsMedico(usuario);

            bool puedeVerPacientes = esAdministrador || esRecepcionista;
            bool puedeVerMedicos = esAdministrador || esRecepcionista;
            bool puedeVerEspecialidades = esAdministrador;
            bool puedeVerUsuarios = esAdministrador;
            bool puedeVerTurnos = esAdministrador || esRecepcionista || esMedico;

            MenuPacientes.Visible = puedeVerPacientes;
            MenuMedicos.Visible = puedeVerMedicos;
            MenuEspecialidades.Visible = puedeVerEspecialidades;
            MenuUsuarios.Visible = puedeVerUsuarios;
            MenuTurnos.Visible = puedeVerTurnos;

            BtnPacientes.Visible = puedeVerPacientes;
            BtnMedicos.Visible = puedeVerMedicos;
            BtnEspecialidades.Visible = puedeVerEspecialidades;
            BtnUsuarios.Visible = puedeVerUsuarios;
            BtnTurnos.Visible = puedeVerTurnos;

            LnkAltaPaciente.Visible = puedeVerPacientes;
            LnkListaPacientes.Visible = puedeVerPacientes;

            LnkAltaMedico.Visible = puedeVerMedicos;
            LnkListaMedicos.Visible = puedeVerMedicos;

            LnkAltaEspecialidad.Visible = esAdministrador;
            LnkListaEspecialidades.Visible = esAdministrador;

            LnkAltaUsuario.Visible = esAdministrador;
            LnkListaUsuarios.Visible = esAdministrador;

            LnkConfiguracionTurnos.Visible = esAdministrador;
            LnkAltaTurno.Visible = esAdministrador || esRecepcionista;
            LnkListaTurnos.Visible = esAdministrador || esRecepcionista;
            LnkMisTurnos.Visible = esMedico;
        }

        private void AbrirPagina(string ruta, params RolEnum[] roles)
        {
            ExigirRoles(roles);
            Response.Redirect(ruta, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void ExigirRoles(params RolEnum[] roles)
        {
            Usuario usuario = Session["UsuarioActual"] as Usuario;
            if (!AutorizacionRutasService.TieneAlgunRol(usuario, roles))
            {
                throw new Exception("No tiene permisos para ejecutar esta accion.");
            }
        }
    }
}
