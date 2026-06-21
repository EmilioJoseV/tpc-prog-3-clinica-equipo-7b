using System;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class ConfiguracionTurnos : Page
    {
        private readonly ConfiguracionTurnoNegocio configuracionTurnoNegocio = new ConfiguracionTurnoNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CargarConfiguracion();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        private void CargarConfiguracion()
        {
            ConfiguracionTurno configuracionTurno = configuracionTurnoNegocio.Obtener();

            HfIdConfiguracionTurno.Value = configuracionTurno.IdConfiguracionTurno.ToString();
            TxtDuracionMinutos.Text = configuracionTurno.DuracionMinutos > 0
                ? configuracionTurno.DuracionMinutos.ToString()
                : string.Empty;
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ConfiguracionTurno configuracionTurno = new ConfiguracionTurno
                {
                    IdConfiguracionTurno = string.IsNullOrWhiteSpace(HfIdConfiguracionTurno.Value) ? 0 : int.Parse(HfIdConfiguracionTurno.Value),
                    DuracionMinutos = int.Parse(TxtDuracionMinutos.Text.Trim()),
                    Activo = true
                };

                configuracionTurnoNegocio.Modificar(configuracionTurno);
                Response.Redirect("../PanelPrincipal.aspx", false);
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
            Response.Redirect("../PanelPrincipal.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
