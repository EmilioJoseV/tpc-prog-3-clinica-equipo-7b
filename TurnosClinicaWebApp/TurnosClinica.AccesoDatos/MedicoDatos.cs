using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class MedicoDatos : IFiltrable<Medico>, IMapeable<Medico>
    {
        private readonly AccesoDatos accesoDatos;
        private readonly MedicoEspecialidadesDatos medicoEspecialidadesDatos;
        private readonly HorarioDisponibilidadMedicoDatos horarioDisponibilidadMedicoDatos;

        public MedicoDatos()
        {
            accesoDatos = new AccesoDatos();
            medicoEspecialidadesDatos = new MedicoEspecialidadesDatos();
            horarioDisponibilidadMedicoDatos = new HorarioDisponibilidadMedicoDatos();
        }

        public MedicoDatos(AccesoDatos accesoDatosCompartido)
        {
            accesoDatos = accesoDatosCompartido;
            medicoEspecialidadesDatos = new MedicoEspecialidadesDatos();
            horarioDisponibilidadMedicoDatos = new HorarioDisponibilidadMedicoDatos();
        }

        public List<Medico> Listar(bool? activo)
        {
            List<Medico> medicos = new List<Medico>();
            try
            {
                string consulta = "SELECT IdMedico, Matricula, DNI, Nombre, Apellido, Telefono, Email, Activo"
                    + " FROM Medicos";

                if (activo.HasValue)
                {
                    consulta += " WHERE Activo = @activo";
                }

                consulta += " ORDER BY Apellido ASC";

                accesoDatos.setearConsulta(consulta);

                if (activo.HasValue)
                {
                    accesoDatos.setearParametro("@activo", activo.Value);
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

        public List<Medico> ListarFiltroRapido(string filtro)
        {
            List<Medico> medicos = new List<Medico>();
            try
            {
                string consulta = "SELECT IdMedico, Matricula, DNI, Nombre, Apellido, Telefono, Email, Activo"
                    + " FROM Medicos";

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    consulta += " WHERE UPPER(Nombre) LIKE '%' + UPPER(@filtro) + '%'"
                        + " OR UPPER(Apellido) LIKE '%' + UPPER(@filtro) + '%'";
                }

                consulta += " ORDER BY Apellido, Nombre";

                accesoDatos.setearConsulta(consulta);

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

        public Medico ObtenerPorId(int idMedico)
        {
            Medico medico = new Medico();
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT IdMedico, Matricula, DNI, Nombre, Apellido, Telefono, Email, Activo"
                    + " FROM Medicos"
                    + " WHERE IdMedico = @idMedico");
                accesoDatos.setearParametro("@idMedico", idMedico);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    medico = MapearFilaAEntidad(accesoDatos.Lector);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }

            return medico;
        }

        public bool ExisteDni(string dni)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT COUNT(1)"
                    + " FROM Medicos"
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
                    + " FROM Medicos"
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
                accesoDatos.setearConsulta(
                    "INSERT INTO Medicos (Matricula, DNI, Nombre, Apellido, Telefono, Email, Activo)"
                    + " OUTPUT INSERTED.IdMedico"
                    + " VALUES (@matricula, @dni, @nombre, @apellido, @telefono, @email, @activo)");
                accesoDatos.setearParametro("@matricula", medico.Matricula);
                accesoDatos.setearParametro("@dni", medico.DNI);
                accesoDatos.setearParametro("@nombre", medico.Nombre);
                accesoDatos.setearParametro("@apellido", medico.Apellido);
                accesoDatos.setearParametro("@telefono", string.IsNullOrWhiteSpace(medico.Telefono) ? (object)DBNull.Value : medico.Telefono);
                accesoDatos.setearParametro("@email", medico.Email);
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
                    + " SET Matricula = @matricula, DNI = @dni, Nombre = @nombre, Apellido = @apellido, Telefono = @telefono, Email = @email, Activo = @activo"
                    + " WHERE IdMedico = @idMedico");
                accesoDatos.setearParametro("@matricula", medico.Matricula);
                accesoDatos.setearParametro("@dni", medico.DNI);
                accesoDatos.setearParametro("@nombre", medico.Nombre);
                accesoDatos.setearParametro("@apellido", medico.Apellido);
                accesoDatos.setearParametro("@telefono", string.IsNullOrWhiteSpace(medico.Telefono) ? (object)DBNull.Value : medico.Telefono);
                accesoDatos.setearParametro("@email", medico.Email);
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
                string consulta = "SELECT IdMedico, Matricula, DNI, Nombre, Apellido, Telefono, Email, Activo"
                    + " FROM Medicos";
                bool tieneCondicion = false;

                if (activo.HasValue)
                {
                    consulta += tieneCondicion ? " AND Activo = @activo" : " WHERE Activo = @activo";
                    tieneCondicion = true;
                }

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    switch (campo)
                    {
                        case "Matricula":
                            consulta += criterio == "Comienza con"
                                ? (tieneCondicion ? " AND Matricula LIKE @filtro + '%'" : " WHERE Matricula LIKE @filtro + '%'")
                                : criterio == "Termina con"
                                    ? (tieneCondicion ? " AND Matricula LIKE '%' + @filtro" : " WHERE Matricula LIKE '%' + @filtro")
                                    : (tieneCondicion ? " AND Matricula LIKE '%' + @filtro + '%'" : " WHERE Matricula LIKE '%' + @filtro + '%'");
                            tieneCondicion = true;
                            break;
                        case "DNI":
                            consulta += criterio == "Igual a"
                                ? (tieneCondicion ? " AND DNI = @filtro" : " WHERE DNI = @filtro")
                                : criterio == "Mayor a"
                                    ? (tieneCondicion ? " AND DNI > @filtro" : " WHERE DNI > @filtro")
                                    : (tieneCondicion ? " AND DNI < @filtro" : " WHERE DNI < @filtro");
                            tieneCondicion = true;
                            break;
                        case "Nombre":
                            consulta += criterio == "Comienza con"
                                ? (tieneCondicion ? " AND Nombre LIKE @filtro + '%'" : " WHERE Nombre LIKE @filtro + '%'")
                                : criterio == "Termina con"
                                    ? (tieneCondicion ? " AND Nombre LIKE '%' + @filtro" : " WHERE Nombre LIKE '%' + @filtro")
                                    : (tieneCondicion ? " AND Nombre LIKE '%' + @filtro + '%'" : " WHERE Nombre LIKE '%' + @filtro + '%'");
                            tieneCondicion = true;
                            break;
                        case "Apellido":
                            consulta += criterio == "Comienza con"
                                ? (tieneCondicion ? " AND Apellido LIKE @filtro + '%'" : " WHERE Apellido LIKE @filtro + '%'")
                                : criterio == "Termina con"
                                    ? (tieneCondicion ? " AND Apellido LIKE '%' + @filtro" : " WHERE Apellido LIKE '%' + @filtro")
                                    : (tieneCondicion ? " AND Apellido LIKE '%' + @filtro + '%'" : " WHERE Apellido LIKE '%' + @filtro + '%'");
                            tieneCondicion = true;
                            break;
                        default:
                            consulta += tieneCondicion
                                ? " AND (Nombre LIKE '%' + @filtro + '%' OR Apellido LIKE '%' + @filtro + '%' OR Matricula LIKE '%' + @filtro + '%')"
                                : " WHERE (Nombre LIKE '%' + @filtro + '%' OR Apellido LIKE '%' + @filtro + '%' OR Matricula LIKE '%' + @filtro + '%')";
                            tieneCondicion = true;
                            break;
                    }
                }

                consulta += " ORDER BY Apellido, Nombre";

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
            Medico medico = MapearBaseAEntidad(fila);
            
            if (medico.IdMedico > 0)
            {
                medico.Especialidades = medicoEspecialidadesDatos.ListarPorMedico(medico.IdMedico);
                medico.HorariosDisponibilidad = horarioDisponibilidadMedicoDatos.ListarPorMedico(medico.IdMedico);
            }

            return medico;
        }

        private Medico MapearBaseAEntidad(SqlDataReader fila)
        {
            Medico medico = new Medico();
            medico.IdMedico = Convert.ToInt32(fila["IdMedico"]);
            medico.Matricula = fila["Matricula"].ToString();
            medico.DNI = fila["DNI"].ToString();
            medico.Nombre = fila["Nombre"].ToString();
            medico.Apellido = fila["Apellido"].ToString();
            medico.Telefono = fila["Telefono"] is DBNull ? string.Empty : fila["Telefono"].ToString();
            medico.Email = fila["Email"].ToString();
            medico.Activo = Convert.ToBoolean(fila["Activo"]);
            return medico;
        }
    }
}
