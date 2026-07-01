using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;
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

        private bool EsReprogramacion
        {
            get { return ObtenerIdTurno() > 0; }
        }

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
                    ValidarAcceso();
                    CargarPacientes();
                    CargarEspecialidades();
                    TxtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    ConfigurarModo();
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
                int idEspecialidad = ObtenerIdEspecialidad();

                List<TurnoDisponibleDTO> disponibles;
                if (ChkBuscarProximo.Checked)
                {
                    disponibles = turnoCalculoService.ListarTurnosDisponiblesMasProximos(
                        idEspecialidad,
                        DateTime.Now,
                        ObtenerIdPaciente(),
                        ObtenerIdTurno());
                }
                else
                {
                    ChkBuscarPorFecha.Checked = true;
                    ChkBuscarProximo.Checked = false;
                    disponibles = turnoCalculoService.ListarTurnosDisponibles(
                        idEspecialidad,
                        ObtenerFecha(),
                        ObtenerIdPaciente(),
                        ObtenerIdTurno());
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
                if (EsReprogramacion)
                {
                    ExigirRolAdministrativo();

                    Turno turno = new Turno
                    {
                        IdTurno = ObtenerIdTurno(),
                        Paciente = new Paciente
                        {
                            IdPaciente = ObtenerIdPaciente()
                        },
                        Especialidad = new Especialidad
                        {
                            IdEspecialidad = ObtenerIdEspecialidad()
                        },
                        Medico = new Medico
                        {
                            IdMedico = ObtenerIdHorarioSeleccionado()
                        },
                        FechaTurno = ObtenerFecha(),
                        HoraInicio = TimeSpan.Parse(HfHoraInicio.Value),
                        HoraFin = TimeSpan.Parse(HfHoraFin.Value),
                        Observaciones = TxtObservaciones.Text,
                        UsuarioModificacion = ObtenerUsuarioActual()
                    };

                    turnoNegocio.Reprogramar(turno);
                    Response.Redirect("DetalleTurno.aspx?id=" + turno.IdTurno, false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                Turno turnoAlta = new Turno
                {
                    Paciente = new Paciente
                    {
                        IdPaciente = ObtenerIdPaciente()
                    },
                    Especialidad = new Especialidad
                    {
                        IdEspecialidad = ObtenerIdEspecialidad()
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

                turnoNegocio.Agregar(turnoAlta);
                Response.Redirect(ObtenerUrlDespuesDeGuardarAlta(), false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            if (EsReprogramacion)
            {
                Response.Redirect("DetalleTurno.aspx?id=" + ObtenerIdTurno(), false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            Response.Redirect("ListaTurnos.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void CargarPacientes()
        {
            List<Paciente> pacientes = pacienteNegocio.Listar(true);
            pacientes.Sort(CompararPacientes);

            List<ListItem> items = new List<ListItem>();
            foreach (Paciente paciente in pacientes)
            {
                items.Add(new ListItem(
                    paciente.Persona.Apellido + ", "
                    + paciente.Persona.Nombre + " - DNI " + paciente.Persona.DNI,
                    paciente.IdPaciente.ToString()));
            }

            DdlPaciente.DataSource = items;
            DdlPaciente.DataValueField = "Value";
            DdlPaciente.DataTextField = "Text";
            DdlPaciente.DataBind();
            DdlPaciente.Items.Insert(0, new ListItem("Seleccionar", string.Empty));
        }

        private void CargarEspecialidades()
        {
            List<Especialidad> especialidades = especialidadNegocio.Listar(true);
            especialidades.Sort(CompararEspecialidades);
            DdlEspecialidad.DataSource = especialidades;
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

        private int ObtenerIdPaciente()
        {
            if (EsReprogramacion)
            {
                if (!int.TryParse(HfIdPaciente.Value, out int idPaciente) || idPaciente <= 0)
                {
                    throw new Exception("El paciente del turno no es valido.");
                }

                return idPaciente;
            }

            return ObtenerIdSeleccionado(DdlPaciente, "Debe seleccionar un paciente.");
        }

        private int ObtenerIdEspecialidad()
        {
            if (EsReprogramacion)
            {
                if (!int.TryParse(HfIdEspecialidad.Value, out int idEspecialidad) || idEspecialidad <= 0)
                {
                    throw new Exception("La especialidad del turno no es valida.");
                }

                return idEspecialidad;
            }

            return ObtenerIdSeleccionado(DdlEspecialidad, "Debe seleccionar una especialidad.");
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
            disponibles.Sort(CompararDisponibilidad);
            DgvDisponibilidad.DataSource = disponibles;
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

        private void ConfigurarModo()
        {
            if (!EsReprogramacion)
            {
                return;
            }

            ExigirRolAdministrativo();

            Turno turno = turnoNegocio.ObtenerPorId(ObtenerIdTurno());
            if (turno == null)
            {
                throw new Exception("El turno no existe.");
            }

            if (turno.EstadoTurno != null && turno.EstadoTurno.EsFinal)
            {
                throw new Exception("El turno esta en un estado final y no admite reprogramacion.");
            }

            LblTitulo.Text = "Reprogramar Turno";
            LblSubtitulo.Text = "Busca un nuevo horario disponible para el turno seleccionado.";
            LblTurnoActual.Text = Server.HtmlEncode(
                turno.NumeroTurno
                + " - "
                + ObtenerFechaConDia(turno.FechaTurno)
                + " de "
                + turno.HoraInicio.ToString(@"hh\:mm")
                + " a "
                + turno.HoraFin.ToString(@"hh\:mm"));
            PnlTurnoActual.Visible = true;
            ColPacienteEditable.Visible = false;
            ColEspecialidadEditable.Visible = false;
            ColPacienteSoloLectura.Visible = true;
            ColEspecialidadSoloLectura.Visible = true;
            TxtPaciente.Text = turno.Paciente.Persona.Apellido + ", " + turno.Paciente.Persona.Nombre + " - DNI " + turno.Paciente.Persona.DNI;
            TxtEspecialidad.Text = turno.Especialidad.Nombre;
            TxtFecha.Text = turno.FechaTurno.ToString("yyyy-MM-dd");
            TxtObservaciones.Text = turno.Observaciones;
            HfIdPaciente.Value = turno.Paciente.IdPaciente.ToString();
            HfIdEspecialidad.Value = turno.Especialidad.IdEspecialidad.ToString();
            DdlPaciente.SelectedValue = turno.Paciente.IdPaciente.ToString();
            DdlEspecialidad.SelectedValue = turno.Especialidad.IdEspecialidad.ToString();
            BtnGuardar.Text = "Reprogramar turno";
        }

        private int ObtenerIdTurno()
        {
            if (!int.TryParse(Request.QueryString["id"], out int idTurno) || idTurno <= 0)
            {
                return 0;
            }

            return idTurno;
        }

        private void LimpiarHorarioSeleccionado()
        {
            HfIdMedicoSeleccionado.Value = string.Empty;
            HfHoraInicio.Value = string.Empty;
            HfHoraFin.Value = string.Empty;
            LblSeleccion.Text = string.Empty;
            PnlSeleccion.Visible = false;
        }

        private void ValidarAcceso()
        {
            if (!AutorizacionRutasService.TieneRol(ObtenerUsuarioActual(), RolEnum.Administrador, RolEnum.Recepcionista))
            {
                throw new Exception("No tiene permisos para gestionar turnos.");
            }
        }

        private void ExigirRolAdministrativo()
        {
            Usuario usuario = ObtenerUsuarioActual();
            if (!AutorizacionRutasService.TieneRol(usuario, RolEnum.Administrador, RolEnum.Recepcionista))
            {
                throw new Exception("No tiene permisos para reprogramar turnos.");
            }
        }

        private string ObtenerUrlDespuesDeGuardarAlta()
        {
            return "ListaTurnos.aspx";
        }

        private int CompararPacientes(Paciente paciente1, Paciente paciente2)
        {
            string apellido1 = paciente1 == null || paciente1.Persona == null ? string.Empty : paciente1.Persona.Apellido;
            string apellido2 = paciente2 == null || paciente2.Persona == null ? string.Empty : paciente2.Persona.Apellido;
            int comparacion = string.Compare(apellido1, apellido2, StringComparison.OrdinalIgnoreCase);

            if (comparacion != 0)
            {
                return comparacion;
            }

            string nombre1 = paciente1 == null || paciente1.Persona == null ? string.Empty : paciente1.Persona.Nombre;
            string nombre2 = paciente2 == null || paciente2.Persona == null ? string.Empty : paciente2.Persona.Nombre;
            return string.Compare(nombre1, nombre2, StringComparison.OrdinalIgnoreCase);
        }

        private int CompararEspecialidades(Especialidad especialidad1, Especialidad especialidad2)
        {
            string nombre1 = especialidad1 == null ? string.Empty : especialidad1.Nombre;
            string nombre2 = especialidad2 == null ? string.Empty : especialidad2.Nombre;
            return string.Compare(nombre1, nombre2, StringComparison.OrdinalIgnoreCase);
        }

        private int CompararDisponibilidad(TurnoDisponibleDTO turno1, TurnoDisponibleDTO turno2)
        {
            int comparacion = DateTime.Compare(turno1.FechaTurno, turno2.FechaTurno);
            if (comparacion != 0)
            {
                return comparacion;
            }

            comparacion = TimeSpan.Compare(turno1.HoraInicio, turno2.HoraInicio);
            if (comparacion != 0)
            {
                return comparacion;
            }

            string apellido1 = turno1.Medico == null || turno1.Medico.Persona == null ? string.Empty : turno1.Medico.Persona.Apellido;
            string apellido2 = turno2.Medico == null || turno2.Medico.Persona == null ? string.Empty : turno2.Medico.Persona.Apellido;
            return string.Compare(apellido1, apellido2, StringComparison.OrdinalIgnoreCase);
        }
    }
}
