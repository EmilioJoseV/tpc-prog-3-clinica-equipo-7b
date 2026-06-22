using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class MedicoDatos : IFiltrable<Medico>, IMapeable<Medico>
    {
        private readonly AccesoDatosBase accesoDatos;

        public MedicoDatos(AccesoDatosBase accesoDatos)
        {
            this.accesoDatos = accesoDatos;
        }

        public List<Medico> Listar(bool? activo)
        {
            return ListarConFiltros(null, null, null, activo);
        }

        public List<Medico> ListarFiltroRapido(string filtro)
        {
            return ListarConFiltros("Nombre", "Contiene", filtro, null);
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
                    CargarDetalleMedico(medico);
                }

                return medico;
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public void Agregar(Medico medico)
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public bool Desactivar(int idMedico)
        {
            return CambiarEstado(idMedico, false);
        }

        public void Activar(int idMedico)
        {
            CambiarEstado(idMedico, true);
        }

        private bool CambiarEstado(int idMedico, bool activo)
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
                return true;
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

        public List<Medico> ListarConFiltros(string campo, string criterio, string filtro, bool? activo)
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
                    switch (campo)
                    {
                        case "Matricula":
                            consulta += criterio == "Comienza con"
                                ? " AND M.Matricula LIKE @filtro + '%'"
                                : criterio == "Termina con"
                                    ? " AND M.Matricula LIKE '%' + @filtro"
                                    : " AND M.Matricula LIKE '%' + @filtro + '%'";
                            break;
                        case "DNI":
                            consulta += criterio == "Igual a"
                                ? " AND P.DNI = @filtro"
                                : criterio == "Mayor a"
                                    ? " AND P.DNI > @filtro"
                                    : " AND P.DNI < @filtro";
                            break;
                        case "Nombre":
                            consulta += criterio == "Comienza con"
                                ? " AND P.Nombre LIKE @filtro + '%'"
                                : criterio == "Termina con"
                                    ? " AND P.Nombre LIKE '%' + @filtro"
                                    : " AND P.Nombre LIKE '%' + @filtro + '%'";
                            break;
                        case "Apellido":
                            consulta += criterio == "Comienza con"
                                ? " AND P.Apellido LIKE @filtro + '%'"
                                : criterio == "Termina con"
                                    ? " AND P.Apellido LIKE '%' + @filtro"
                                    : " AND P.Apellido LIKE '%' + @filtro + '%'";
                            break;
                        default:
                            consulta += " AND (P.Nombre LIKE '%' + @filtro + '%' OR P.Apellido LIKE '%' + @filtro + '%' OR P.DNI LIKE '%' + @filtro + '%' OR M.Matricula LIKE '%' + @filtro + '%')";
                            break;
                    }
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
                    Medico medico = MapearFilaAEntidad(accesoDatos.Lector);
                    CargarDetalleMedico(medico);
                    medicos.Add(medico);
                }

                return medicos;
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

            MedicoEspecialidadesDatos especialidadesDatos = new MedicoEspecialidadesDatos();
            HorarioDisponibilidadMedicoDatos horariosDatos = new HorarioDisponibilidadMedicoDatos();

            medico.Especialidades = especialidadesDatos.ListarPorMedico(medico.IdMedico);
            medico.HorariosDisponibilidad = horariosDatos.ListarPorMedico(medico.IdMedico);
        }
    }
}
