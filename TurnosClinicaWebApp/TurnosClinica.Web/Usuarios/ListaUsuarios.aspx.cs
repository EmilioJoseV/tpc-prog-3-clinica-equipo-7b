using System;
using System.Collections.Generic;
using System.Web.UI;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;
using TurnosClinica.Negocio;

namespace TurnosClinica.Web
{
    public partial class ListaUsuarios : Page
    {
        public List<Usuario> ListaUsuariosProp { get; set; } = new List<Usuario>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();

                ListaUsuariosProp = negocio.ListarTodos() ?? new List<Usuario>();
            }
            catch (Exception ex)
            {
                ListaUsuariosProp = new List<Usuario>();
                Session.Add("Error", ex.ToString());
            }
        }

        protected string ObtenerEstadoUsuarioTexto(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return "Sin estado";
            }

            if (!Enum.TryParse(value.ToString(), true, out EstadoUsuarioEnum estado))
            {
                return "Sin estado";
            }
            switch (estado)
            {
                case EstadoUsuarioEnum.Activo:
                    return "Activo";
                case EstadoUsuarioEnum.Pendiente:
                    return "Pendiente";
                case EstadoUsuarioEnum.Bloqueado:
                    return "Bloqueado";
                case EstadoUsuarioEnum.Inactivo:
                    return "Inactivo";
                case EstadoUsuarioEnum.CambioClavePendiente:
                    return "Cambio de clave pendiente";
                default:
                    return "Sin estado";
            }
        }

        protected string ObtenerEstadoUsuarioBadgeClass(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return "bg-secondary";
            }

            if (!Enum.TryParse(value.ToString(), true, out EstadoUsuarioEnum estado))
            {
                return "bg-secondary";
            }
            switch (estado)
            {
                case EstadoUsuarioEnum.Activo:
                    return "bg-success";
                case EstadoUsuarioEnum.Pendiente:
                    return "bg-warning text-dark";
                case EstadoUsuarioEnum.Bloqueado:
                    return "bg-danger";
                case EstadoUsuarioEnum.Inactivo:
                    return "bg-secondary";
                case EstadoUsuarioEnum.CambioClavePendiente:
                    return "bg-info text-dark";
                default:
                    return "bg-secondary";
            }
        }
    }
}
