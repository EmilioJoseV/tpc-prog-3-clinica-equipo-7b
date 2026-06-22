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
                    CargarPaciente();
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
                Paciente paciente = new Paciente
                {
                    IdPaciente = string.IsNullOrWhiteSpace(HfIdPaciente.Value) ? 0 : int.Parse(HfIdPaciente.Value),
                    Persona = new Persona
                    {
                        IdPersona = string.IsNullOrWhiteSpace(HfIdPersona.Value) ? 0 : int.Parse(HfIdPersona.Value),
                        DNI = TxtDni.Text,
                        Nombre = TxtNombre.Text,
                        Apellido = TxtApellido.Text,
                        Telefono = TxtTelefono.Text,
                        Email = TxtEmail.Text
                    },
                    FechaNacimiento = DateTime.Parse(TxtFechaNacimiento.Text.Trim(), CultureInfo.InvariantCulture),
                    Direccion = TxtDireccion.Text,
                    Activo = ChkActivo.Checked
                };

                if (paciente.IdPaciente > 0)
                {
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

        private void CargarPaciente()
        {
            string id = Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(id))
            {
                return;
            }

            Paciente paciente = pacienteNegocio.ObtenerPorId(int.Parse(id));
            if (paciente == null)
            {
                throw new Exception("El paciente no existe.");
            }

            HfIdPaciente.Value = paciente.IdPaciente.ToString();
            HfIdPersona.Value = paciente.Persona.IdPersona.ToString();
            TxtDni.Text = paciente.Persona.DNI;
            TxtNombre.Text = paciente.Persona.Nombre;
            TxtApellido.Text = paciente.Persona.Apellido;
            TxtFechaNacimiento.Text = paciente.FechaNacimiento.ToString("yyyy-MM-dd");
            TxtTelefono.Text = paciente.Persona.Telefono;
            TxtEmail.Text = paciente.Persona.Email;
            TxtDireccion.Text = paciente.Direccion;
            ChkActivo.Checked = paciente.Activo;
        }
    }
}
