using System;
using System.Collections.Generic;
using System.Globalization;
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
                ((MasterLayout)Master).MostrarError(ex.Message);
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
                return;
            }

            Medico medico = medicoNegocio.ObtenerPorId(int.Parse(id));
            if (medico == null)
            {
                throw new Exception("El medico no existe.");
            }

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
                item.Selected = MedicoTieneEspecialidadSeleccionada(medico, item.Value);
            }

            Session[HorariosSessionKey] = medico.HorariosDisponibilidad ?? new List<HorarioDisponibilidadMedico>();
        }

        protected void BtnAgregarHorario_Click(object sender, EventArgs e)
        {
            try
            {
                List<HorarioDisponibilidadMedico> horarios = ObtenerHorariosDesdeListado();
                TimeSpan horaDesde = TimeSpan.ParseExact(TxtHoraDesde.Text.Trim(), "hh\\:mm", CultureInfo.InvariantCulture);
                TimeSpan horaHasta = TimeSpan.ParseExact(TxtHoraHasta.Text.Trim(), "hh\\:mm", CultureInfo.InvariantCulture);
                DiaSemanaEnum diaSemana = (DiaSemanaEnum)int.Parse(DdlDiaSemana.SelectedValue);

                horarios.Add(new HorarioDisponibilidadMedico
                {
                    DiaSemana = diaSemana,
                    HoraDesde = horaDesde,
                    HoraHasta = horaHasta
                });

                Session[HorariosSessionKey] = horarios;
                TxtHoraDesde.Text = string.Empty;
                TxtHoraHasta.Text = string.Empty;
                BindHorarios();
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void RptHorarios_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EliminarHorario")
                {
                    List<HorarioDisponibilidadMedico> horarios = ObtenerHorariosDesdeListado();
                    int index = int.Parse(e.CommandArgument.ToString());
                    if (index >= 0 && index < horarios.Count)
                    {
                        horarios.RemoveAt(index);
                    }

                    Session[HorariosSessionKey] = horarios;
                    BindHorarios();
                }
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
                    HorariosDisponibilidad = ObtenerHorariosDesdeListado()
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
                Response.Redirect("ListaMedicos.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
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

        private bool MedicoTieneEspecialidadSeleccionada(Medico medico, string idEspecialidad)
        {
            if (medico == null || medico.Especialidades == null || string.IsNullOrWhiteSpace(idEspecialidad))
            {
                return false;
            }

            foreach (Especialidad especialidad in medico.Especialidades)
            {
                if (especialidad != null && especialidad.IdEspecialidad.ToString() == idEspecialidad)
                {
                    return true;
                }
            }

            return false;
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
        }

        private List<HorarioDisponibilidadMedico> ObtenerHorariosDesdeListado()
        {
            List<HorarioDisponibilidadMedico> horarios = new List<HorarioDisponibilidadMedico>();

            foreach (RepeaterItem item in RptHorarios.Items)
            {
                HiddenField idHorario = (HiddenField)item.FindControl("HfIdHorario");
                DropDownList diaSemana = (DropDownList)item.FindControl("DdlDiaSemanaFila");
                TextBox horaDesde = (TextBox)item.FindControl("TxtHoraDesdeFila");
                TextBox horaHasta = (TextBox)item.FindControl("TxtHoraHastaFila");

                horarios.Add(new HorarioDisponibilidadMedico
                {
                    IdHorarioDisponibilidadMedico = string.IsNullOrWhiteSpace(idHorario.Value)
                        ? 0
                        : int.Parse(idHorario.Value),
                    DiaSemana = (DiaSemanaEnum)int.Parse(diaSemana.SelectedValue),
                    HoraDesde = TimeSpan.ParseExact(horaDesde.Text.Trim(), "hh\\:mm", CultureInfo.InvariantCulture),
                    HoraHasta = TimeSpan.ParseExact(horaHasta.Text.Trim(), "hh\\:mm", CultureInfo.InvariantCulture)
                });
            }

            return horarios;
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
