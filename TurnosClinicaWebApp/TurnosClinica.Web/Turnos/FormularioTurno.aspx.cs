using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;
using TurnosClinica.Negocio.DTO;

namespace TurnosClinica.Web
{
    public partial class FormularioTurno : Page
    {
        private readonly PacienteNegocio pacienteNegocio = new PacienteNegocio();
        private readonly EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
        private readonly MedicoNegocio medicoNegocio = new MedicoNegocio();
        private readonly TurnoCalculoService turnoCalculoService = new TurnoCalculoService();
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
                TxtFecha.Attributes["min"] = DateTime.Today.ToString("yyyy-MM-dd");
                TxtFecha.Attributes["max"] = DateTime.Today.AddDays(90).ToString("yyyy-MM-dd");

                if (!IsPostBack)
                {
                    CargarPacientes();
                    CargarEspecialidades();
                    TxtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void ChkBuscarPorFecha_CheckedChanged(object sender, EventArgs e)
        {
            ChkBuscarPorFecha.Checked = true;
            ChkBuscarProximo.Checked = false;
            PnlDisponibilidad.Visible = false;
            LimpiarHorarioSeleccionado();
        }

        protected void ChkBuscarProximo_CheckedChanged(object sender, EventArgs e)
        {
            ChkBuscarPorFecha.Checked = false;
            ChkBuscarProximo.Checked = true;
            PnlDisponibilidad.Visible = false;
            LimpiarHorarioSeleccionado();
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                int idEspecialidad = ObtenerIdSeleccionado(
                    DdlEspecialidad,
                    "Debe seleccionar una especialidad.");

                List<TurnoDisponibleDTO> disponibles;
                if (ChkBuscarProximo.Checked)
                {
                    disponibles = turnoCalculoService.ListarTurnosDisponiblesMasProximos(
                        idEspecialidad,
                        DateTime.Now);
                }
                else
                {
                    ChkBuscarPorFecha.Checked = true;
                    ChkBuscarProximo.Checked = false;
                    disponibles = turnoCalculoService.ListarTurnosDisponibles(
                        idEspecialidad,
                        ObtenerFecha());
                }

                if (disponibles.Count > 0)
                {
                    TxtFecha.Text = disponibles[0].FechaTurno.ToString("yyyy-MM-dd");
                }

                MostrarDisponibilidad(disponibles);
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void DgvDisponibilidad_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Seleccionar")
            {
                return;
            }

            try
            {
                string[] valores = e.CommandArgument.ToString().Split('|');
                int idMedico = int.Parse(valores[0]);
                TimeSpan horaInicio = TimeSpan.Parse(valores[1]);
                TimeSpan horaFin = TimeSpan.Parse(valores[2]);
                Medico medico = medicoNegocio.ObtenerPorId(idMedico);

                HfIdMedicoSeleccionado.Value = idMedico.ToString();
                HfHoraInicio.Value = horaInicio.ToString();
                HfHoraFin.Value = horaFin.ToString();
                LblSeleccion.Text = Server.HtmlEncode(
                    medico.Persona.Apellido + ", " + medico.Persona.Nombre
                    + " - " + ObtenerFecha().ToString("dd/MM/yyyy")
                    + " de " + horaInicio.ToString(@"hh\:mm")
                    + " a " + horaFin.ToString(@"hh\:mm"));
                PnlSeleccion.Visible = true;
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Turno turno = new Turno
                {
                    Paciente = new Paciente
                    {
                        IdPaciente = ObtenerIdSeleccionado(DdlPaciente, "Debe seleccionar un paciente.")
                    },
                    Especialidad = new Especialidad
                    {
                        IdEspecialidad = ObtenerIdSeleccionado(DdlEspecialidad, "Debe seleccionar una especialidad.")
                    },
                    Medico = new Medico
                    {
                        IdMedico = ObtenerIdHorarioSeleccionado()
                    },
                    FechaTurno = ObtenerFecha(),
                    HoraInicio = TimeSpan.Parse(HfHoraInicio.Value),
                    HoraFin = TimeSpan.Parse(HfHoraFin.Value),
                    Observaciones = TxtObservaciones.Text,
                    UsuarioAlta = ObtenerUsuarioActual()
                };

                turnoNegocio.Agregar(turno);
                Response.Redirect("ListaTurnos.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListaTurnos.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void CargarPacientes()
        {
            DdlPaciente.DataSource = pacienteNegocio.Listar(true)
                .OrderBy(paciente => paciente.Persona.Apellido)
                .ThenBy(paciente => paciente.Persona.Nombre)
                .Select(paciente => new
                {
                    paciente.IdPaciente,
                    NombreCompleto = paciente.Persona.Apellido + ", "
                        + paciente.Persona.Nombre + " - DNI " + paciente.Persona.DNI
                })
                .ToList();
            DdlPaciente.DataValueField = "IdPaciente";
            DdlPaciente.DataTextField = "NombreCompleto";
            DdlPaciente.DataBind();
            DdlPaciente.Items.Insert(0, new ListItem("Seleccionar", string.Empty));
        }

        private void CargarEspecialidades()
        {
            DdlEspecialidad.DataSource = especialidadNegocio.Listar(true)
                .OrderBy(especialidad => especialidad.Nombre)
                .ToList();
            DdlEspecialidad.DataValueField = "IdEspecialidad";
            DdlEspecialidad.DataTextField = "Nombre";
            DdlEspecialidad.DataBind();
            DdlEspecialidad.Items.Insert(0, new ListItem("Seleccionar", string.Empty));
        }

        private int ObtenerIdSeleccionado(DropDownList lista, string mensaje)
        {
            if (!int.TryParse(lista.SelectedValue, out int id) || id <= 0)
            {
                throw new Exception(mensaje);
            }

            return id;
        }

        private int ObtenerIdHorarioSeleccionado()
        {
            if (!int.TryParse(HfIdMedicoSeleccionado.Value, out int idMedico)
                || idMedico <= 0
                || string.IsNullOrWhiteSpace(HfHoraInicio.Value)
                || string.IsNullOrWhiteSpace(HfHoraFin.Value))
            {
                throw new Exception("Debe seleccionar un horario disponible.");
            }

            return idMedico;
        }

        private DateTime ObtenerFecha()
        {
            if (!DateTime.TryParse(TxtFecha.Text, out DateTime fecha))
            {
                throw new Exception("Debe ingresar una fecha valida.");
            }

            return fecha.Date;
        }

        private Usuario ObtenerUsuarioActual()
        {
            if (Session["UsuarioActual"] is Usuario usuario && usuario.IdUsuario > 0)
            {
                return usuario;
            }

            throw new Exception("Debe iniciar sesion para registrar un turno.");
        }

        private void MostrarDisponibilidad(List<TurnoDisponibleDTO> disponibles)
        {
            DgvDisponibilidad.DataSource = disponibles
                .OrderBy(turno => turno.FechaTurno)
                .ThenBy(turno => turno.HoraInicio)
                .ThenBy(turno => turno.Medico.Persona.Apellido)
                .ToList();
            DgvDisponibilidad.DataBind();
            PnlDisponibilidad.Visible = true;
            LimpiarHorarioSeleccionado();
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

        private void LimpiarHorarioSeleccionado()
        {
            HfIdMedicoSeleccionado.Value = string.Empty;
            HfHoraInicio.Value = string.Empty;
            HfHoraFin.Value = string.Empty;
            LblSeleccion.Text = string.Empty;
            PnlSeleccion.Visible = false;
        }
    }
}
