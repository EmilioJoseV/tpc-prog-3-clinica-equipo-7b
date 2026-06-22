using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class MedicoDatos : IEntidadGestionable<Medico>, IMapeable<Medico>
    {
        private readonly AccesoDatosBase accesoDatos;

        public MedicoDatos(AccesoDatosBase accesoDatos)
        {
            this.accesoDatos = accesoDatos;
        }

        public List<Medico> Listar(bool? activo = null)
        {
            return ListarFiltroAvanzado(null, null, null, activo);
        }

        public List<Medico> ListarFiltroRapido(string palabra, bool? activo = null)
        {
            return ListarFiltroAvanzado(null, null, palabra, activo);
        }

        public Medico ObtenerPorId(int idMedico)
        {
            Medico medico = null;
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT M.IdMedico, M.IdPersona, M.Matricula, M.Activo,"
                    + " P.DNI, P.Nombre, P.Apellido, P.Telefono, P.Email"
                    + " FROM Medicos M"
                    + " INNER JOIN Personas P ON P.IdPersona = M.IdPersona"
                    + " WHERE M.IdMedico = @idMedico");
                accesoDatos.setearParametro("@idMedico", idMedico);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    medico = MapearFilaAEntidad(accesoDatos.Lector);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }

            CargarDetalleMedico(medico);
            return medico;
        }

        public bool ExisteMatricula(string matricula)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT COUNT(1)"
                    + " FROM Medicos"
                    + " WHERE Matricula = @matricula");
                accesoDatos.setearParametro("@matricula", matricula);
                return accesoDatos.ejecutarAccionScalar() > 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public int Agregar(Medico medico)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "INSERT INTO Medicos (IdPersona, Matricula, Activo)"
                    + " OUTPUT INSERTED.IdMedico"
                    + " VALUES (@idPersona, @matricula, @activo)");
                accesoDatos.setearParametro("@idPersona", medico.Persona.IdPersona);
                accesoDatos.setearParametro("@matricula", medico.Matricula);
                accesoDatos.setearParametro("@activo", medico.Activo);
                medico.IdMedico = accesoDatos.ejecutarAccionScalar();
                return medico.IdMedico;
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public void Modificar(Medico medico)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "UPDATE Medicos"
                    + " SET IdPersona = @idPersona, Matricula = @matricula, Activo = @activo"
                    + " WHERE IdMedico = @idMedico");
                accesoDatos.setearParametro("@idPersona", medico.Persona.IdPersona);
                accesoDatos.setearParametro("@matricula", medico.Matricula);
                accesoDatos.setearParametro("@activo", medico.Activo);
                accesoDatos.setearParametro("@idMedico", medico.IdMedico);
                accesoDatos.ejecutarAccion();
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public void Desactivar(int idMedico)
        {
            CambiarEstado(idMedico, false);
        }

        public void Activar(int idMedico)
        {
            CambiarEstado(idMedico, true);
        }

        private void CambiarEstado(int idMedico, bool activo)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "UPDATE Medicos"
                    + " SET Activo = @activo"
                    + " WHERE IdMedico = @idMedico");
                accesoDatos.setearParametro("@idMedico", idMedico);
                accesoDatos.setearParametro("@activo", activo);
                accesoDatos.ejecutarAccion();
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public List<Medico> ListarFiltroAvanzado(string campo, string criterio, string filtro, bool? activo = null)
        {
            List<Medico> medicos = new List<Medico>();
            try
            {
                string consulta = "SELECT M.IdMedico, M.IdPersona, M.Matricula, M.Activo, "
                    + "       P.DNI, P.Nombre, P.Apellido, P.Telefono, P.Email "
                    + "FROM Medicos M "
                    + "INNER JOIN Personas P ON P.IdPersona = M.IdPersona WHERE 1=1";

                if (activo.HasValue)
                {
                    consulta += " AND M.Activo = @activo";
                }

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    consulta += ConstruirFiltro(campo, criterio);
                }

                consulta += " ORDER BY P.Apellido, P.Nombre";

                accesoDatos.setearConsulta(consulta);
                if (activo.HasValue)
                {
                    accesoDatos.setearParametro("@activo", activo.Value);
                }
                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    accesoDatos.setearParametro("@filtro", filtro);
                }

                accesoDatos.ejecutarLectura();
                while (accesoDatos.Lector.Read())
                {
                    medicos.Add(MapearFilaAEntidad(accesoDatos.Lector));
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }

            foreach (Medico medico in medicos)
            {
                CargarDetalleMedico(medico);
            }

            return medicos;
        }

        private string ConstruirFiltro(string campo, string criterio)
        {
            string campoSql = ObtenerCampoSql(campo);
            if (string.IsNullOrWhiteSpace(campoSql))
            {
                return " AND (P.Nombre LIKE '%' + @filtro + '%'"
                    + " OR P.Apellido LIKE '%' + @filtro + '%'"
                    + " OR P.DNI LIKE '%' + @filtro + '%'"
                    + " OR P.Email LIKE '%' + @filtro + '%'"
                    + " OR M.Matricula LIKE '%' + @filtro + '%')";
            }

            if (string.Equals(criterio, "Igual a", StringComparison.OrdinalIgnoreCase))
            {
                return " AND " + campoSql + " = @filtro";
            }

            if (string.Equals(criterio, "Comienza con", StringComparison.OrdinalIgnoreCase))
            {
                return " AND " + campoSql + " LIKE @filtro + '%'";
            }

            if (string.Equals(criterio, "Termina con", StringComparison.OrdinalIgnoreCase))
            {
                return " AND " + campoSql + " LIKE '%' + @filtro";
            }

            return " AND " + campoSql + " LIKE '%' + @filtro + '%'";
        }

        private string ObtenerCampoSql(string campo)
        {
            if (string.IsNullOrWhiteSpace(campo))
            {
                return null;
            }

            switch (campo.Trim())
            {
                case "Matricula":
                    return "M.Matricula";
                case "DNI":
                    return "P.DNI";
                case "Nombre":
                    return "P.Nombre";
                case "Apellido":
                    return "P.Apellido";
                case "Email":
                    return "P.Email";
                default:
                    return null;
            }
        }

        public Medico MapearFilaAEntidad(SqlDataReader fila)
        {
            return new Medico
            {
                IdMedico = Convert.ToInt32(fila["IdMedico"]),
                Matricula = fila["Matricula"].ToString(),
                Persona = new Persona
                {
                    IdPersona = Convert.ToInt32(fila["IdPersona"]),
                    DNI = fila["DNI"].ToString(),
                    Nombre = fila["Nombre"].ToString(),
                    Apellido = fila["Apellido"].ToString(),
                    Telefono = fila["Telefono"] is DBNull ? string.Empty : fila["Telefono"].ToString(),
                    Email = fila["Email"].ToString()
                },
                Activo = Convert.ToBoolean(fila["Activo"])
            };
        }

        private void CargarDetalleMedico(Medico medico)
        {
            if (medico == null || medico.IdMedico <= 0)
            {
                return;
            }

            MedicoEspecialidadesDatos especialidadesDatos = new MedicoEspecialidadesDatos(new AccesoDatosBase());
            HorarioDisponibilidadMedicoDatos horariosDatos = new HorarioDisponibilidadMedicoDatos(new AccesoDatosBase());

            medico.Especialidades = especialidadesDatos.ObtenerEspecialidadesAsociadasAMedico(medico.IdMedico);
            medico.HorariosDisponibilidad = horariosDatos.ObtenerHorariosAsociadosAMedico(medico.IdMedico);
        }
    }
}
