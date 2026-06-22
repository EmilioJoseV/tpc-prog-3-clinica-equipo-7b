using System.Collections.Generic;

namespace TurnosClinica.AccesoDatos
{
    public interface IEntidadGestionable<T> where T : class
    {
        List<T> Listar(bool? activo = null);

        List<T> ListarFiltroRapido(string palabra, bool? activo = null);

        List<T> ListarFiltroAvanzado(string campo, string criterio, string filtro, bool? activo = null);

        T ObtenerPorId(int id);

        int Agregar(T entidad);

        void Modificar(T entidad);

        void Desactivar(int id);

        void Activar(int id);
    }
}
