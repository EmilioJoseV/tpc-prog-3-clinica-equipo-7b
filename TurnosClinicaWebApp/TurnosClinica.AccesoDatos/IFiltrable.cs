using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public interface IFiltrable<T> where T : class
    {
        List<T> ListarConFiltros(string campo, string criterio, string filtro, bool? activo);
    }
}
