using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class EstadoTurnoDatos
    {
        private readonly AccesoDatosBase accesoDatos;

        public EstadoTurnoDatos()
        {
            accesoDatos = new AccesoDatosBase();
        }

        public EstadoTurnoDatos(AccesoDatosBase accesoDatosCompartido)
        {
            accesoDatos = accesoDatosCompartido;
        }

        public List<EstadoTurno> Listar(bool? activo)
        {
            List<EstadoTurno> estados = new List<EstadoTurno>();
            try
            {
                string consulta = "SELECT IdEstadoTurno, Nombre, Descripcion, EsFinal, Activo FROM EstadosTurno WHERE 1=1";

                if (activo.HasValue)
                {
                    consulta += " AND Activo = @activo";
                }

                accesoDatos.setearConsulta(consulta);

                if (activo.HasValue)
                {
                    accesoDatos.setearParametro("@activo", activo.Value);
                }

                accesoDatos.ejecutarLectura();
                while (accesoDatos.Lector.Read())
                {
                    estados.Add(MapearFilaAEntidad(accesoDatos.Lector));
                }

                return estados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public EstadoTurno ObtenerPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return null;
            }

            EstadoTurno estadoTurno = null;
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT IdEstadoTurno, Nombre, Descripcion, EsFinal, Activo"
                    + " FROM EstadosTurno"
                    + " WHERE UPPER(Nombre) = UPPER(@nombre)");
                accesoDatos.setearParametro("@nombre", nombre.Trim());
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    estadoTurno = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return estadoTurno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        private EstadoTurno MapearFilaAEntidad(SqlDataReader fila)
        {
            return new EstadoTurno
            {
                IdEstadoTurno = Convert.ToInt32(fila["IdEstadoTurno"]),
                Nombre = fila["Nombre"].ToString(),
                Descripcion = fila["Descripcion"] is DBNull ? string.Empty : fila["Descripcion"].ToString(),
                EsFinal = Convert.ToBoolean(fila["EsFinal"]),
                Activo = Convert.ToBoolean(fila["Activo"])
            };
        }
    }
}
