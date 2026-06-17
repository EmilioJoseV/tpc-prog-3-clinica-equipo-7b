using System;
using System.Globalization;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class FormularioPaciente : Page
    {
        private readonly PacienteNegocio pacienteNegocio = new PacienteNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CargarPacienteSiCorresponde();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        private void CargarPacienteSiCorresponde()
        {
            string id = Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(id))
            {
                return;
            }

            Paciente paciente = pacienteNegocio.ObtenerPorId(int.Parse(id));
            HfIdPaciente.Value = paciente.IdPaciente.ToString();
            TxtDni.Text = paciente.DNI;
            TxtNombre.Text = paciente.Nombre;
            TxtApellido.Text = paciente.Apellido;
            TxtFechaNacimiento.Text = paciente.FechaNacimiento.ToString("yyyy-MM-dd");
            TxtTelefono.Text = paciente.Telefono;
            TxtEmail.Text = paciente.Email;
            TxtDireccion.Text = paciente.Direccion;
            ChkActivo.Checked = paciente.Activo;
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Paciente paciente = new Paciente
                {
                    DNI = TxtDni.Text,
                    Nombre = TxtNombre.Text,
                    Apellido = TxtApellido.Text,
                    FechaNacimiento = DateTime.Parse(TxtFechaNacimiento.Text.Trim(), CultureInfo.InvariantCulture),
                    Telefono = TxtTelefono.Text,
                    Email = TxtEmail.Text,
                    Direccion = TxtDireccion.Text,
                    Activo = ChkActivo.Checked
                };

                if (!string.IsNullOrWhiteSpace(HfIdPaciente.Value))
                {
                    paciente.IdPaciente = int.Parse(HfIdPaciente.Value);
                    pacienteNegocio.Modificar(paciente);
                }
                else
                {
                    pacienteNegocio.Agregar(paciente);
                }

                Response.Redirect("ListaPacientes.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListaPacientes.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
