using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class MisTurnos : Page
    {
        private readonly TurnoNegocio turnoNegocio = new TurnoNegocio();
        private readonly EstadoTurnoNegocio estadoTurnoNegocio = new EstadoTurnoNegocio();
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
                    Medico medico = ObtenerMedicoActual();
                    litMedico.Text = "Medico: " + medico.Persona.Apellido + ", " + medico.Persona.Nombre;
                    txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    CargarEstados();
                    CargarLista();
                }
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CargarLista();
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                CargarLista();
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                txtFiltro.Text = string.Empty;
                txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
                ddlEstado.SelectedIndex = 0;
                CargarLista();
            }
            catch (Exception ex)
            {
                ((MasterLayout)Master).MostrarError(ex.Message);
            }
        }

        protected void btnHoy_Click(object sender, EventArgs e)
        {
            txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
            CargarLista();
        }

        protected void btnManana_Click(object sender, EventArgs e)
        {
            txtFecha.Text = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            CargarLista();
        }

        protected void btnTodos_Click(object sender, EventArgs e)
        {
            txtFecha.Text = string.Empty;
            CargarLista();
        }

        protected void dgvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Ver")
            {
                return;
            }

            Response.Redirect("DetalleTurno.aspx?id=" + e.CommandArgument, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void dgvTurnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            Turno turno = e.Row.DataItem as Turno;
            if (turno != null && turno.FechaTurno.Date == DateTime.Today)
            {
                e.Row.CssClass += " table-warning";
            }
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

        protected string ObtenerClaseEstado(string estado)
        {
            switch (estado)
            {
                case "Nuevo":
                    return "badge bg-primary";
                case "Reprogramado":
                    return "badge bg-warning text-dark";
                case "Cancelado":
                    return "badge bg-danger";
                case "NoAsistio":
                    return "badge bg-secondary";
                case "Cerrado":
                    return "badge bg-success";
                default:
                    return "badge bg-dark";
            }
        }

        protected string ObtenerTextoAccion(object esFinal)
        {
            bool turnoFinal = esFinal is bool valor && valor;
            return turnoFinal ? "Ver" : "Atender";
        }

        private void CargarEstados()
        {
            List<EstadoTurno> estados = estadoTurnoNegocio.Listar(true);
            estados.Sort(CompararEstadosTurno);
            ddlEstado.DataSource = estados;
            ddlEstado.DataValueField = "IdEstadoTurno";
            ddlEstado.DataTextField = "Nombre";
            ddlEstado.DataBind();
            ddlEstado.Items.Insert(0, new ListItem("Todos", string.Empty));
        }

        private void CargarLista()
        {
            Medico medico = ObtenerMedicoActual();
            litMedico.Text = "Medico: " + medico.Persona.Apellido + ", " + medico.Persona.Nombre;

            int? idEstado = null;
            if (int.TryParse(ddlEstado.SelectedValue, out int valorEstado) && valorEstado > 0)
            {
                idEstado = valorEstado;
            }

            DateTime? fecha = null;
            if (DateTime.TryParse(txtFecha.Text, out DateTime fechaTurno))
            {
                fecha = fechaTurno.Date;
            }

            dgvTurnos.DataSource = turnoNegocio.ListarPorMedicoConFiltros(
                medico.IdMedico,
                txtFiltro.Text,
                idEstado,
                fecha);
            dgvTurnos.DataBind();
        }

        private Medico ObtenerMedicoActual()
        {
            Usuario usuario = ObtenerUsuarioActual();
            if (usuario.Persona == null || usuario.Persona.IdPersona <= 0)
            {
                throw new Exception("No se pudo identificar la persona asociada al usuario autenticado.");
            }

            return medicoNegocio.ObtenerPorIdPersona(usuario.Persona.IdPersona);
        }

        private Usuario ObtenerUsuarioActual()
        {
            if (Session["UsuarioActual"] is Usuario usuario && usuario.IdUsuario > 0)
            {
                return usuario;
            }

            throw new Exception("Debe iniciar sesion para consultar sus turnos.");
        }

        private int CompararEstadosTurno(EstadoTurno estado1, EstadoTurno estado2)
        {
            string nombre1 = estado1 == null ? string.Empty : estado1.Nombre;
            string nombre2 = estado2 == null ? string.Empty : estado2.Nombre;
            return string.Compare(nombre1, nombre2, StringComparison.OrdinalIgnoreCase);
        }
    }
}
