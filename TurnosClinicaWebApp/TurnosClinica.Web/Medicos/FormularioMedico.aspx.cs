using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class FormularioMedico : Page
    {
        private readonly MedicoNegocio medicoNegocio = new MedicoNegocio();
        private readonly EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
        private const string HorariosSessionKey = "HorariosMedico";
        private const string HorarioEditIndexKey = "HorarioEditIndex";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CargarDiasSemana();
                    CargarEspecialidades();
                    CargarMedicoSiCorresponde();
                    BindHorarios();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        private void CargarDiasSemana()
        {
            DdlDiaSemana.Items.Clear();
            foreach (DiaSemanaEnum dia in Enum.GetValues(typeof(DiaSemanaEnum)))
            {
                DdlDiaSemana.Items.Add(new ListItem(ObtenerDiaSemanaTexto(dia), ((int)dia).ToString()));
            }
        }

        private void CargarEspecialidades()
        {
            List<Especialidad> especialidades = especialidadNegocio.Listar(true);
            CblEspecialidades.DataSource = especialidades;
            CblEspecialidades.DataTextField = "Nombre";
            CblEspecialidades.DataValueField = "IdEspecialidad";
            CblEspecialidades.DataBind();
        }

        private void CargarMedicoSiCorresponde()
        {
            string id = Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(id))
            {
                Session[HorariosSessionKey] = new List<HorarioDisponibilidadMedico>();
                CancelarEdicionHorario();
                return;
            }

            Medico medico = medicoNegocio.ObtenerPorId(int.Parse(id));
            HfIdMedico.Value = medico.IdMedico.ToString();
            HfIdPersona.Value = medico.Persona.IdPersona.ToString();
            TxtMatricula.Text = medico.Matricula;
            TxtDni.Text = medico.Persona.DNI;
            TxtNombre.Text = medico.Persona.Nombre;
            TxtApellido.Text = medico.Persona.Apellido;
            TxtTelefono.Text = medico.Persona.Telefono;
            TxtEmail.Text = medico.Persona.Email;
            ChkMedicoActivo.Checked = medico.Activo;

            foreach (ListItem item in CblEspecialidades.Items)
            {
                item.Selected = medico.Especialidades.Any(especialidad => especialidad.IdEspecialidad.ToString() == item.Value);
            }

            Session[HorariosSessionKey] = medico.HorariosDisponibilidad ?? new List<HorarioDisponibilidadMedico>();
            CancelarEdicionHorario();
        }

        protected void BtnAgregarHorario_Click(object sender, EventArgs e)
        {
            try
            {
                List<HorarioDisponibilidadMedico> horarios = ObtenerHorariosTemporales();
                TimeSpan horaDesde = TimeSpan.ParseExact(TxtHoraDesde.Text.Trim(), "hh\\:mm", CultureInfo.InvariantCulture);
                TimeSpan horaHasta = TimeSpan.ParseExact(TxtHoraHasta.Text.Trim(), "hh\\:mm", CultureInfo.InvariantCulture);
                int? indiceEdicion = ObtenerIndiceHorarioEnEdicion();
                DiaSemanaEnum diaSemana = (DiaSemanaEnum)int.Parse(DdlDiaSemana.SelectedValue);

                HorarioDisponibilidadMedico horario = new HorarioDisponibilidadMedico
                {
                    DiaSemana = diaSemana,
                    HoraDesde = horaDesde,
                    HoraHasta = horaHasta
                };

                if (indiceEdicion.HasValue && indiceEdicion.Value >= 0 && indiceEdicion.Value < horarios.Count)
                {
                    horarios[indiceEdicion.Value] = horario;
                }
                else
                {
                    horarios.Add(horario);
                }

                Session[HorariosSessionKey] = horarios;
                CancelarEdicionHorario();
                BindHorarios();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void RptHorarios_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                int? indiceEnEdicion = ObtenerIndiceHorarioEnEdicion();

                if (e.CommandName == "EliminarHorario")
                {
                    List<HorarioDisponibilidadMedico> horarios = ObtenerHorariosTemporales();
                    int index = int.Parse(e.CommandArgument.ToString());
                    if (index >= 0 && index < horarios.Count)
                    {
                        horarios.RemoveAt(index);
                        if (indiceEnEdicion.HasValue && indiceEnEdicion.Value == index)
                        {
                            CancelarEdicionHorario();
                        }
                        else if (indiceEnEdicion.HasValue && indiceEnEdicion.Value > index)
                        {
                            Session[HorarioEditIndexKey] = indiceEnEdicion.Value - 1;
                        }
                    }
                    else if (indiceEnEdicion.HasValue && indiceEnEdicion.Value == index)
                    {
                        CancelarEdicionHorario();
                    }

                    Session[HorariosSessionKey] = horarios;
                    BindHorarios();
                }
                else if (e.CommandName == "EditarHorario")
                {
                    List<HorarioDisponibilidadMedico> horarios = ObtenerHorariosTemporales();
                    int index = int.Parse(e.CommandArgument.ToString());
                    if (index >= 0 && index < horarios.Count)
                    {
                        CargarHorarioEnEdicion(index, horarios[index]);
                    }
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Medico medico = new Medico
                {
                    Persona = new Persona
                    {
                        IdPersona = string.IsNullOrWhiteSpace(HfIdPersona.Value) ? 0 : int.Parse(HfIdPersona.Value),
                        DNI = TxtDni.Text,
                        Nombre = TxtNombre.Text,
                        Apellido = TxtApellido.Text,
                        Telefono = TxtTelefono.Text,
                        Email = TxtEmail.Text
                    },
                    Matricula = TxtMatricula.Text,
                    Activo = ChkMedicoActivo.Checked,
                    Especialidades = ObtenerEspecialidadesSeleccionadas(),
                    HorariosDisponibilidad = ObtenerHorariosTemporales()
                };

                if (!string.IsNullOrWhiteSpace(HfIdMedico.Value))
                {
                    medico.IdMedico = int.Parse(HfIdMedico.Value);
                    medicoNegocio.Modificar(medico);
                }
                else
                {
                    medicoNegocio.Agregar(medico);
                }

                Session.Remove(HorariosSessionKey);
                CancelarEdicionHorario();
                Response.Redirect("ListaMedicos.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        private List<Especialidad> ObtenerEspecialidadesSeleccionadas()
        {
            List<Especialidad> especialidades = new List<Especialidad>();

            foreach (ListItem item in CblEspecialidades.Items)
            {
                if (item.Selected)
                {
                    especialidades.Add(new Especialidad
                    {
                        IdEspecialidad = int.Parse(item.Value),
                        Nombre = item.Text
                    });
                }
            }

            return especialidades;
        }

        private List<HorarioDisponibilidadMedico> ObtenerHorariosTemporales()
        {
            if (Session[HorariosSessionKey] is List<HorarioDisponibilidadMedico> horarios)
            {
                return horarios;
            }

            List<HorarioDisponibilidadMedico> nuevaLista = new List<HorarioDisponibilidadMedico>();
            Session[HorariosSessionKey] = nuevaLista;
            return nuevaLista;
        }

        private void BindHorarios()
        {
            RptHorarios.DataSource = ObtenerHorariosTemporales();
            RptHorarios.DataBind();
            BtnAgregarHorario.Text = ObtenerIndiceHorarioEnEdicion().HasValue ? "Actualizar" : "Agregar";
        }

        private int? ObtenerIndiceHorarioEnEdicion()
        {
            if (Session[HorarioEditIndexKey] == null)
            {
                return null;
            }

            int indice;
            if (int.TryParse(Session[HorarioEditIndexKey].ToString(), out indice))
            {
                return indice;
            }

            return null;
        }

        private void CargarHorarioEnEdicion(int index, HorarioDisponibilidadMedico horario)
        {
            DdlDiaSemana.SelectedValue = ((int)horario.DiaSemana).ToString();
            TxtHoraDesde.Text = horario.HoraDesde.ToString(@"hh\:mm");
            TxtHoraHasta.Text = horario.HoraHasta.ToString(@"hh\:mm");
            Session[HorarioEditIndexKey] = index;
            BtnAgregarHorario.Text = "Actualizar";
        }

        private void CancelarEdicionHorario()
        {
            Session.Remove(HorarioEditIndexKey);
            BtnAgregarHorario.Text = "Agregar";
        }

        protected string ObtenerDiaSemanaTexto(object value)
        {
            DiaSemanaEnum dia = (DiaSemanaEnum)Convert.ToInt32(value);
            switch (dia)
            {
                case DiaSemanaEnum.Lunes: return "Lunes";
                case DiaSemanaEnum.Martes: return "Martes";
                case DiaSemanaEnum.Miercoles: return "Miercoles";
                case DiaSemanaEnum.Jueves: return "Jueves";
                case DiaSemanaEnum.Viernes: return "Viernes";
                case DiaSemanaEnum.Sabado: return "Sabado";
                case DiaSemanaEnum.Domingo: return "Domingo";
                default: return "Desconocido";
            }
        }

        protected string FormatearHora(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return string.Empty;
            }

            TimeSpan hora = (TimeSpan)value;
            return hora.ToString(@"hh\:mm");
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove(HorariosSessionKey);
            Response.Redirect("ListaMedicos.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
