using System;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class DetalleTurno : Page
    {
        private readonly TurnoNegocio turnoNegocio = new TurnoNegocio();

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
                Turno turno = ConstruirTurnoModificado();
                turnoNegocio.Modificar(turno);

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
            Response.Redirect("FormularioTurno.aspx?id=" + ObtenerIdTurno(), false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnCancelarTurno_Click(object sender, EventArgs e)
        {
            try
            {
                turnoNegocio.Cancelar(ObtenerIdTurno(), ObtenerUsuarioActual().IdUsuario);
                Response.Redirect("ListaTurnos.aspx", false);
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
                turnoNegocio.MarcarNoAsistio(ObtenerIdTurno(), ObtenerUsuarioActual().IdUsuario);
                Response.Redirect("ListaTurnos.aspx", false);
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
                turnoNegocio.Cerrar(ObtenerIdTurno(), ObtenerUsuarioActual().IdUsuario);
                Response.Redirect("ListaTurnos.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListaTurnos.aspx", false);
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
            Turno turno = ObtenerTurnoActual();

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
            ConfigurarSegunEstado(turno);
            pnlContenido.Visible = true;
        }

        private void ConfigurarSegunEstado(Turno turno)
        {
            bool esFinal = turno.EstadoTurno != null && turno.EstadoTurno.EsFinal;

            txtObservaciones.ReadOnly = esFinal;
            txtDiagnosticoMedico.ReadOnly = esFinal;
            btnGuardar.Visible = !esFinal;
            btnReprogramar.Visible = !esFinal;
            btnCancelarTurno.Visible = !esFinal;
            btnNoAsistio.Visible = !esFinal;
            btnCerrarTurno.Visible = !esFinal;
        }

        private Turno ObtenerTurnoActual()
        {
            int idTurno = ObtenerIdTurno();
            Turno turno = turnoNegocio.ObtenerPorId(idTurno);

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
    }
}
