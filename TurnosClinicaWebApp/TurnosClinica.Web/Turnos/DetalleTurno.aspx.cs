using System;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class DetalleTurno : Page
    {
        private readonly TurnoNegocio turnoNegocio = new TurnoNegocio();
        private readonly MedicoNegocio medicoNegocio = new MedicoNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Session["UsuarioActual"] is Usuario usuario) || usuario.IdUsuario <= 0)
            {
                Response.Redirect("../Ingresar.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            try
            {
                if (!IsPostBack)
                {
                    ValidarAccesoPantalla(usuario);
                    CargarTurno();
                }
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = ObtenerUsuarioActual();
                ValidarAccesoPantalla(usuario);
                Medico medico = ObtenerMedicoActualSiCorresponde(usuario);

                if (EsModoMedico(medico))
                {
                    turnoNegocio.ModificarDiagnosticoMedico(
                        ObtenerIdTurno(),
                        txtDiagnosticoMedico.Text,
                        medico.IdMedico,
                        usuario.IdUsuario);
                }
                else
                {
                    ExigirAdministrador(usuario);
                    Turno turno = ConstruirTurnoModificado();
                    turnoNegocio.Modificar(turno);
                }

                Response.Redirect("DetalleTurno.aspx?id=" + ObtenerIdTurno(), false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void btnReprogramar_Click(object sender, EventArgs e)
        {
            ExigirRolAdministrativo(ObtenerUsuarioActual());
            Response.Redirect("FormularioTurno.aspx?id=" + ObtenerIdTurno(), false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnCancelarTurno_Click(object sender, EventArgs e)
        {
            try
            {
                ExigirAdministrador(ObtenerUsuarioActual());
                turnoNegocio.Cancelar(ObtenerIdTurno(), ObtenerUsuarioActual().IdUsuario);
                Response.Redirect(ObtenerUrlVolver(), false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void btnNoAsistio_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = ObtenerUsuarioActual();
                ValidarAccesoPantalla(usuario);
                Medico medico = ObtenerMedicoActualSiCorresponde(usuario);

                if (EsModoMedico(medico))
                {
                    turnoNegocio.MarcarNoAsistioComoMedico(ObtenerIdTurno(), usuario.IdUsuario, medico.IdMedico);
                }
                else
                {
                    ExigirAdministrador(usuario);
                    turnoNegocio.MarcarNoAsistio(ObtenerIdTurno(), usuario.IdUsuario);
                }

                Response.Redirect(ObtenerUrlVolver(), false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void btnCerrarTurno_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = ObtenerUsuarioActual();
                ValidarAccesoPantalla(usuario);
                Medico medico = ObtenerMedicoActualSiCorresponde(usuario);

                if (EsModoMedico(medico))
                {
                    turnoNegocio.CerrarComoMedico(
                        ObtenerIdTurno(),
                        usuario.IdUsuario,
                        medico.IdMedico,
                        txtDiagnosticoMedico.Text);
                }
                else
                {
                    ExigirAdministrador(usuario);
                    turnoNegocio.Cerrar(ObtenerIdTurno(), usuario.IdUsuario, txtDiagnosticoMedico.Text);
                }

                Response.Redirect(ObtenerUrlVolver(), false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect(ObtenerUrlVolver(), false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected string ObtenerFechaConDia(DateTime fecha)
        {
            string[] dias =
            {
                "Domingo",
                "Lunes",
                "Martes",
                "Miercoles",
                "Jueves",
                "Viernes",
                "Sabado"
            };

            return dias[(int)fecha.DayOfWeek] + " - " + fecha.ToString("dd/MM/yyyy");
        }

        private void CargarTurno()
        {
            Usuario usuario = ObtenerUsuarioActual();
            Medico medico = ObtenerMedicoActualSiCorresponde(usuario);
            Turno turno = ObtenerTurnoActual(medico);
            bool esModoMedico = EsModoMedico(turno, medico);

            txtNumeroTurno.Text = turno.NumeroTurno;
            txtEstado.Text = turno.EstadoTurno.Nombre;
            txtFechaActual.Text = ObtenerFechaConDia(turno.FechaTurno);
            txtHorarioActual.Text = turno.HoraInicio.ToString(@"hh\:mm") + " - " + turno.HoraFin.ToString(@"hh\:mm");
            txtPaciente.Text = turno.Paciente.Persona.Apellido + ", " + turno.Paciente.Persona.Nombre + " - DNI " + turno.Paciente.Persona.DNI;
            txtMedico.Text = turno.Medico.Persona.Apellido + ", " + turno.Medico.Persona.Nombre + " - Matricula " + turno.Medico.Matricula;
            txtEspecialidad.Text = turno.Especialidad.Nombre;
            txtEmailPaciente.Text = turno.Paciente.Persona.Email;
            txtObservaciones.Text = turno.Observaciones;
            txtDiagnosticoMedico.Text = turno.DiagnosticoMedico;
            ConfigurarSegunEstado(turno, esModoMedico);
            pnlContenido.Visible = true;
        }

        private void ConfigurarSegunEstado(Turno turno, bool esModoMedico)
        {
            Usuario usuario = ObtenerUsuarioActual();
            bool puedeGestionarTurnos = AutorizacionRutasService.TieneRol(usuario, RolEnum.Administrador, RolEnum.Recepcionista);
            bool puedeGestionAdministrativaTurno = AutorizacionRutasService.TieneRol(usuario, RolEnum.Administrador);
            bool esFinal = turno.EstadoTurno != null && turno.EstadoTurno.EsFinal;

            txtObservaciones.ReadOnly = esFinal || esModoMedico || !puedeGestionAdministrativaTurno;
            txtDiagnosticoMedico.ReadOnly = esFinal;
            btnGuardar.Visible = !esFinal && (esModoMedico || puedeGestionAdministrativaTurno);
            btnReprogramar.Visible = !esFinal && !esModoMedico && puedeGestionarTurnos;
            btnCancelarTurno.Visible = !esFinal && puedeGestionAdministrativaTurno;
            btnNoAsistio.Visible = !esFinal && (esModoMedico || puedeGestionAdministrativaTurno);
            btnCerrarTurno.Visible = !esFinal && (esModoMedico || puedeGestionAdministrativaTurno);
            btnVolver.Text = esModoMedico ? "Volver" : "Cancelar";
        }

        private Turno ObtenerTurnoActual(Medico medicoActual = null)
        {
            int idTurno = ObtenerIdTurno();
            Turno turno = medicoActual != null
                ? turnoNegocio.ObtenerPorIdParaMedico(idTurno, medicoActual.IdMedico)
                : turnoNegocio.ObtenerPorId(idTurno);

            if (turno == null)
            {
                throw new Exception("El turno no existe.");
            }

            return turno;
        }

        private int ObtenerIdTurno()
        {
            if (!int.TryParse(Request.QueryString["id"], out int idTurno) || idTurno <= 0)
            {
                throw new Exception("El id del turno no es valido.");
            }

            return idTurno;
        }

        private Turno ConstruirTurnoModificado()
        {
            return new Turno
            {
                IdTurno = ObtenerIdTurno(),
                Observaciones = txtObservaciones.Text,
                DiagnosticoMedico = txtDiagnosticoMedico.Text,
                UsuarioModificacion = ObtenerUsuarioActual()
            };
        }

        private Usuario ObtenerUsuarioActual()
        {
            if (Session["UsuarioActual"] is Usuario usuario && usuario.IdUsuario > 0)
            {
                return usuario;
            }

            throw new Exception("Debe iniciar sesion para modificar el turno.");
        }

        private Medico ObtenerMedicoActual(Usuario usuario)
        {
            if (usuario == null || usuario.Persona == null || usuario.Persona.IdPersona <= 0)
            {
                throw new Exception("No se pudo identificar la persona asociada al usuario autenticado.");
            }

            return medicoNegocio.ObtenerPorIdPersona(usuario.Persona.IdPersona);
        }

        private Medico ObtenerMedicoActualSiCorresponde(Usuario usuario)
        {
            if (usuario == null)
            {
                return null;
            }

            try
            {
                return ObtenerMedicoActual(usuario);
            }
            catch
            {
                if (usuario.Rol != null
                    && string.Equals(
                        usuario.Rol.Nombre,
                        RolEnum.Medico.ToString(),
                        StringComparison.OrdinalIgnoreCase))
                {
                    throw;
                }

                return null;
            }
        }

        private bool EsModoMedico(Medico medicoActual)
        {
            if (!AutorizacionRutasService.TieneRol(ObtenerUsuarioActual(), RolEnum.Medico))
            {
                return false;
            }

            if (medicoActual == null)
            {
                return false;
            }

            Turno turno = ObtenerTurnoActual(medicoActual);
            return EsModoMedico(turno, medicoActual);
        }

        private bool EsModoMedico(Turno turno, Medico medicoActual)
        {
            return AutorizacionRutasService.TieneRol(ObtenerUsuarioActual(), RolEnum.Medico)
                && medicoActual != null
                && turno != null
                && turno.Medico != null
                && turno.Medico.IdMedico == medicoActual.IdMedico;
        }

        private void ValidarAccesoPantalla(Usuario usuario)
        {
            if (AutorizacionRutasService.TieneRol(usuario, RolEnum.Administrador, RolEnum.Recepcionista, RolEnum.Medico))
            {
                return;
            }

            throw new Exception("No tiene permisos para acceder a este turno.");
        }

        private void ExigirAdministrador(Usuario usuario)
        {
            if (!AutorizacionRutasService.TieneRol(usuario, RolEnum.Administrador))
            {
                throw new Exception("No tiene permisos para ejecutar esta accion.");
            }
        }

        private void ExigirRolAdministrativo(Usuario usuario)
        {
            if (!AutorizacionRutasService.TieneRol(usuario, RolEnum.Administrador, RolEnum.Recepcionista))
            {
                throw new Exception("No tiene permisos para ejecutar esta accion.");
            }
        }

        private string ObtenerUrlVolver()
        {
            Usuario usuario = ObtenerUsuarioActual();
            Medico medico = ObtenerMedicoActualSiCorresponde(usuario);
            return EsModoMedico(medico) ? "MisTurnos.aspx" : "ListaTurnos.aspx";
        }
    }
}
