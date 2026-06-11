using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public interface IMapeable<T> where T : class
    {
        T MapearFilaAEntidad(SqlDataReader fila);
    }
}
