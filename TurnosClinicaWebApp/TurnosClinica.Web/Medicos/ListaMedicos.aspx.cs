using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class ListaMedicos : Page
    {
        public List<Medico> MedicosLista { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarLista();
            }
        }

        private void CargarLista()
        {
            try
            {
                MedicoNegocio negocio = new MedicoNegocio();
                MedicosLista = negocio.Listar();

                RptMedicos.DataSource = MedicosLista;
                RptMedicos.DataBind();
                PnlVacio.Visible = MedicosLista == null || MedicosLista.Count == 0;
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void BtnNuevoMedico_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioMedico.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void RptMedicos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                int idMedico = int.Parse(e.CommandArgument.ToString());

                if (e.CommandName == "Editar")
                {
                    Response.Redirect("FormularioMedico.aspx?id=" + idMedico, false);
                    Context.ApplicationInstance.CompleteRequest();
                }

                if (e.CommandName == "Desactivar")
                {
                    MedicoNegocio negocio = new MedicoNegocio();
                    negocio.Desactivar(idMedico);
                    CargarLista();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("../Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
    }
}
