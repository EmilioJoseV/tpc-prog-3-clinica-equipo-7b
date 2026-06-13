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
                return;
            }

            Medico medico = medicoNegocio.ObtenerPorId(int.Parse(id));
            HfIdMedico.Value = medico.IdMedico.ToString();
            TxtMatricula.Text = medico.Matricula;
            TxtDni.Text = medico.DNI;
            TxtNombre.Text = medico.Nombre;
            TxtApellido.Text = medico.Apellido;
            TxtTelefono.Text = medico.Telefono;
            TxtEmail.Text = medico.Email;
            ChkMedicoActivo.Checked = medico.Activo;

            foreach (ListItem item in CblEspecialidades.Items)
            {
                item.Selected = medico.Especialidades.Any(especialidad => especialidad.IdEspecialidad.ToString() == item.Value);
            }

            Session[HorariosSessionKey] = medico.HorariosDisponibilidad ?? new List<HorarioDisponibilidadMedico>();
        }

        protected void BtnAgregarHorario_Click(object sender, EventArgs e)
        {
            try
            {
                List<HorarioDisponibilidadMedico> horarios = ObtenerHorariosTemporales();
                TimeSpan horaDesde = TimeSpan.ParseExact(TxtHoraDesde.Text.Trim(), "hh\\:mm", CultureInfo.InvariantCulture);
                TimeSpan horaHasta = TimeSpan.ParseExact(TxtHoraHasta.Text.Trim(), "hh\\:mm", CultureInfo.InvariantCulture);

                horarios.Add(new HorarioDisponibilidadMedico
                {
                    DiaSemana = (DiaSemanaEnum)int.Parse(DdlDiaSemana.SelectedValue),
                    HoraDesde = horaDesde,
                    HoraHasta = horaHasta,
                    Activo = ChkHorarioActivo.Checked
                });

                Session[HorariosSessionKey] = horarios;
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
                if (e.CommandName == "EliminarHorario")
                {
                    List<HorarioDisponibilidadMedico> horarios = ObtenerHorariosTemporales();
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
                    Matricula = TxtMatricula.Text,
                    DNI = TxtDni.Text,
                    Nombre = TxtNombre.Text,
                    Apellido = TxtApellido.Text,
                    Telefono = TxtTelefono.Text,
                    Email = TxtEmail.Text,
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
                SincronizarEstadosHorariosDesdeVista(horarios);
                return horarios;
            }

            List<HorarioDisponibilidadMedico> nuevaLista = new List<HorarioDisponibilidadMedico>();
            Session[HorariosSessionKey] = nuevaLista;
            return nuevaLista;
        }

        private void SincronizarEstadosHorariosDesdeVista(List<HorarioDisponibilidadMedico> horarios)
        {
            for (int i = 0; i < RptHorarios.Items.Count && i < horarios.Count; i++)
            {
                CheckBox chkHorarioActivo = RptHorarios.Items[i].FindControl("ChkHorarioActivo") as CheckBox;
                if (chkHorarioActivo != null)
                {
                    horarios[i].Activo = chkHorarioActivo.Checked;
                }
            }
        }

        private void BindHorarios()
        {
            RptHorarios.DataSource = ObtenerHorariosTemporales();
            RptHorarios.DataBind();
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
