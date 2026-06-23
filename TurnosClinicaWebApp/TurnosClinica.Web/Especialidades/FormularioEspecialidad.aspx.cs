using System;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class FormularioEspecialidad : Page
    {
        private readonly EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
        private readonly MedicoNegocio medicoNegocio = new MedicoNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    return;
                }

                chkActivo.Checked = true;

                int idEspecialidad;
                if (!int.TryParse(Request.QueryString["id"], out idEspecialidad))
                {
                    CargarMedicosAsociados(null);
                    return;
                }

                Especialidad especialidad = especialidadNegocio.ObtenerPorId(idEspecialidad);
                if (especialidad == null)
                {
                    throw new Exception("La especialidad no existe.");
                }

                hfIdEspecialidad.Value = especialidad.IdEspecialidad.ToString();
                lblTitulo.Text = "Detalle de Especialidad";
                txtNombre.Text = especialidad.Nombre;
                txtDescripcion.Text = especialidad.Descripcion;
                chkActivo.Checked = especialidad.Activo;
                CargarMedicosAsociados(idEspecialidad);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Especialidad especialidad = new Especialidad
                {
                    Nombre = txtNombre.Text,
                    Descripcion = txtDescripcion.Text,
                    Activo = chkActivo.Checked
                };

                int idEspecialidad;
                if (int.TryParse(hfIdEspecialidad.Value, out idEspecialidad))
                {
                    especialidad.IdEspecialidad = idEspecialidad;
                    especialidadNegocio.Modificar(especialidad);
                }
                else
                {
                    especialidadNegocio.Agregar(especialidad);
                }

                Response.Redirect("ListaEspecialidades.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListaEspecialidades.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void CargarMedicosAsociados(int? idEspecialidad)
        {
            dgvMedicosAsociados.DataSource = idEspecialidad.HasValue
                ? medicoNegocio.ListarPorEspecialidad(idEspecialidad.Value)
                : null;
            dgvMedicosAsociados.DataBind();
        }

        private void MostrarError(Exception ex)
        {
            ((MasterLayout)Master).MostrarError(ex.Message);
        }
    }
}
