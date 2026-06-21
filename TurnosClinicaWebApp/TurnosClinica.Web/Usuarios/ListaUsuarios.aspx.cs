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

                ListaUsuariosProp = negocio.Listar() ?? new List<Usuario>();
            }
            catch (Exception ex)
            {
                ListaUsuariosProp = new List<Usuario>();
                Session.Add("Error", ex.ToString());
            }
        }

        protected string ObtenerEstadoUsuarioTexto(object value)
        {
            EstadoUsuario estadoUsuario = value as EstadoUsuario;
            if (estadoUsuario != null && !string.IsNullOrWhiteSpace(estadoUsuario.Nombre))
            {
                return estadoUsuario.Nombre;
            }

            if (value == null || value == DBNull.Value)
            {
                return "Sin estado";
            }

            return value.ToString();
        }

        protected string ObtenerEstadoUsuarioBadgeClass(object value)
        {
            EstadoUsuario estadoUsuario = value as EstadoUsuario;
            if (estadoUsuario != null && !string.IsNullOrWhiteSpace(estadoUsuario.Nombre))
            {
                return ObtenerEstadoUsuarioBadgeClassPorNombre(estadoUsuario.Nombre);
            }

            if (value == null || value == DBNull.Value)
            {
                return "bg-secondary";
            }

            return ObtenerEstadoUsuarioBadgeClassPorNombre(value.ToString());
        }

        private string ObtenerEstadoUsuarioBadgeClassPorNombre(string nombreEstado)
        {
            if (string.IsNullOrWhiteSpace(nombreEstado))
            {
                return "bg-secondary";
            }

            if (string.Equals(nombreEstado, EstadoUsuarioEnum.Activo.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "bg-success";
            }

            if (string.Equals(nombreEstado, EstadoUsuarioEnum.Pendiente.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "bg-warning text-dark";
            }

            if (string.Equals(nombreEstado, EstadoUsuarioEnum.Bloqueado.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "bg-danger";
            }

            if (string.Equals(nombreEstado, EstadoUsuarioEnum.Inactivo.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "bg-secondary";
            }

            if (string.Equals(nombreEstado, EstadoUsuarioEnum.CambioClavePendiente.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return "bg-info text-dark";
            }

            return "bg-secondary";
        }
    }
}
