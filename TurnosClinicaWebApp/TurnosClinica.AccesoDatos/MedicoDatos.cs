using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class MedicoDatos : IFiltrable<Medico>, IMapeable<Medico>
    {
        private readonly AccesoDatos accesoDatos;
        private readonly PersonaDatos personaDatos;
        private readonly MedicoEspecialidadesDatos medicoEspecialidadesDatos;
        private readonly HorarioDisponibilidadMedicoDatos horarioDisponibilidadMedicoDatos;

        public MedicoDatos()
        {
            accesoDatos = new AccesoDatos();
            personaDatos = new PersonaDatos(accesoDatos);
            medicoEspecialidadesDatos = new MedicoEspecialidadesDatos(accesoDatos);
            horarioDisponibilidadMedicoDatos = new HorarioDisponibilidadMedicoDatos(accesoDatos);
        }

        public MedicoDatos(AccesoDatos accesoDatosCompartido)
        {
            accesoDatos = accesoDatosCompartido;
            personaDatos = new PersonaDatos(accesoDatosCompartido);
            medicoEspecialidadesDatos = new MedicoEspecialidadesDatos(accesoDatosCompartido);
            horarioDisponibilidadMedicoDatos = new HorarioDisponibilidadMedicoDatos(accesoDatosCompartido);
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
            Medico medico = new Medico();
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

        public Medico ObtenerPorIdPersona(int idPersona)
        {
            Medico medico = null;
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT M.IdMedico, M.IdPersona, M.Matricula, M.Activo,"
                    + " P.DNI, P.Nombre, P.Apellido, P.Telefono, P.Email"
                    + " FROM Medicos M"
                    + " INNER JOIN Personas P ON P.IdPersona = M.IdPersona"
                    + " WHERE M.IdPersona = @idPersona");
                accesoDatos.setearParametro("@idPersona", idPersona);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    medico = MapearFilaAEntidad(accesoDatos.Lector);
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

        public bool ExisteDni(string dni)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT COUNT(1)"
                    + " FROM Personas"
                    + " WHERE DNI = @dni");
                accesoDatos.setearParametro("@dni", dni);
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

        public bool ExisteEmail(string email)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT COUNT(1)"
                    + " FROM Personas"
                    + " WHERE Email = @email");
                accesoDatos.setearParametro("@email", email);
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
                if (medico.IdPersona <= 0)
                {
                    medico.IdPersona = personaDatos.Agregar(medico);
                }
                else
                {
                    personaDatos.Modificar(medico);
                }

                accesoDatos.setearConsulta(
                    "INSERT INTO Medicos (IdPersona, Matricula, Activo)"
                    + " OUTPUT INSERTED.IdMedico"
                    + " VALUES (@idPersona, @matricula, @activo)");
                accesoDatos.setearParametro("@idPersona", medico.IdPersona);
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
                personaDatos.Modificar(medico);

                accesoDatos.setearConsulta(
                    "UPDATE Medicos"
                    + " SET IdPersona = @idPersona, Matricula = @matricula, Activo = @activo"
                    + " WHERE IdMedico = @idMedico");
                accesoDatos.setearParametro("@idPersona", medico.IdPersona);
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
            try
            {
                accesoDatos.setearConsulta(
                    "UPDATE Medicos"
                    + " SET Activo = 0"
                    + " WHERE IdMedico = @idMedico");
                accesoDatos.setearParametro("@idMedico", idMedico);
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
                    + "INNER JOIN Personas P ON P.IdPersona = M.IdPersona";
                bool tieneCondicion = false;

                if (activo.HasValue)
                {
                    consulta += tieneCondicion ? " AND M.Activo = @activo" : " WHERE M.Activo = @activo";
                    tieneCondicion = true;
                }

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    switch (campo)
                    {
                        case "Matricula":
                            consulta += criterio == "Comienza con"
                                ? (tieneCondicion ? " AND M.Matricula LIKE @filtro + '%'" : " WHERE M.Matricula LIKE @filtro + '%'")
                                : criterio == "Termina con"
                                    ? (tieneCondicion ? " AND M.Matricula LIKE '%' + @filtro" : " WHERE M.Matricula LIKE '%' + @filtro")
                                    : (tieneCondicion ? " AND M.Matricula LIKE '%' + @filtro + '%'" : " WHERE M.Matricula LIKE '%' + @filtro + '%'");
                            tieneCondicion = true;
                            break;
                        case "DNI":
                            consulta += criterio == "Igual a"
                                ? (tieneCondicion ? " AND P.DNI = @filtro" : " WHERE P.DNI = @filtro")
                                : criterio == "Mayor a"
                                    ? (tieneCondicion ? " AND P.DNI > @filtro" : " WHERE P.DNI > @filtro")
                                    : (tieneCondicion ? " AND P.DNI < @filtro" : " WHERE P.DNI < @filtro");
                            tieneCondicion = true;
                            break;
                        case "Nombre":
                            consulta += criterio == "Comienza con"
                                ? (tieneCondicion ? " AND P.Nombre LIKE @filtro + '%'" : " WHERE P.Nombre LIKE @filtro + '%'")
                                : criterio == "Termina con"
                                    ? (tieneCondicion ? " AND P.Nombre LIKE '%' + @filtro" : " WHERE P.Nombre LIKE '%' + @filtro")
                                    : (tieneCondicion ? " AND P.Nombre LIKE '%' + @filtro + '%'" : " WHERE P.Nombre LIKE '%' + @filtro + '%'");
                            tieneCondicion = true;
                            break;
                        case "Apellido":
                            consulta += criterio == "Comienza con"
                                ? (tieneCondicion ? " AND P.Apellido LIKE @filtro + '%'" : " WHERE P.Apellido LIKE @filtro + '%'")
                                : criterio == "Termina con"
                                    ? (tieneCondicion ? " AND P.Apellido LIKE '%' + @filtro" : " WHERE P.Apellido LIKE '%' + @filtro")
                                    : (tieneCondicion ? " AND P.Apellido LIKE '%' + @filtro + '%'" : " WHERE P.Apellido LIKE '%' + @filtro + '%'");
                            tieneCondicion = true;
                            break;
                        default:
                            consulta += tieneCondicion
                                ? " AND (P.Nombre LIKE '%' + @filtro + '%' OR P.Apellido LIKE '%' + @filtro + '%' OR P.DNI LIKE '%' + @filtro + '%' OR M.Matricula LIKE '%' + @filtro + '%')"
                                : " WHERE (P.Nombre LIKE '%' + @filtro + '%' OR P.Apellido LIKE '%' + @filtro + '%' OR P.DNI LIKE '%' + @filtro + '%' OR M.Matricula LIKE '%' + @filtro + '%')";
                            tieneCondicion = true;
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
                    medicos.Add(MapearFilaAEntidad(accesoDatos.Lector));
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
            Medico medico = new Medico
            {
                IdMedico = Convert.ToInt32(fila["IdMedico"]),
                IdPersona = Convert.ToInt32(fila["IdPersona"]),
                Matricula = fila["Matricula"].ToString(),
                DNI = fila["DNI"].ToString(),
                Nombre = fila["Nombre"].ToString(),
                Apellido = fila["Apellido"].ToString(),
                Telefono = fila["Telefono"] is DBNull ? string.Empty : fila["Telefono"].ToString(),
                Email = fila["Email"].ToString(),
                Activo = Convert.ToBoolean(fila["Activo"])
            };

            if (medico.IdMedico > 0)
            {
                MedicoEspecialidadesDatos especialidadesDatos = new MedicoEspecialidadesDatos();
                HorarioDisponibilidadMedicoDatos horariosDatos = new HorarioDisponibilidadMedicoDatos();

                medico.Especialidades = especialidadesDatos.ListarPorMedico(medico.IdMedico);
                medico.HorariosDisponibilidad = horariosDatos.ListarPorMedico(medico.IdMedico);
            }

            return medico;
        }
    }
}
