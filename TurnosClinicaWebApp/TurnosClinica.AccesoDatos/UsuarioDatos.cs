using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.AccesoDatos
{
    public class UsuarioDatos : IMapeable<Usuario>
    {
        private readonly AccesoDatosBase accesoDatos;
        private readonly RolDatos rolDatos;
        private readonly EstadoUsuarioDatos estadoUsuarioDatos;

        public UsuarioDatos(AccesoDatosBase accesoDatos)
        {
            this.accesoDatos = accesoDatos;
            rolDatos = new RolDatos(accesoDatos.CrearContextoCompartido());
            estadoUsuarioDatos = new EstadoUsuarioDatos(accesoDatos.CrearContextoCompartido());
        }

        public List<Usuario> Listar(string rolLiteral, string estadoUsuarioLiteral)
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                string consulta = ObtenerConsultaBase() + " WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(rolLiteral))
                {
                    consulta += " AND UPPER(R.Nombre) = UPPER(@rolLiteral)";
                }

                if (!string.IsNullOrWhiteSpace(estadoUsuarioLiteral))
                {
                    consulta += " AND UPPER(EU.Nombre) = UPPER(@estadoLiteral)";
                }

                accesoDatos.setearConsulta(consulta);

                if (!string.IsNullOrWhiteSpace(rolLiteral))
                {
                    accesoDatos.setearParametro("@rolLiteral", rolLiteral);
                }

                if (!string.IsNullOrWhiteSpace(estadoUsuarioLiteral))
                {
                    accesoDatos.setearParametro("@estadoLiteral", estadoUsuarioLiteral);
                }

                accesoDatos.ejecutarLectura();
                while (accesoDatos.Lector.Read())
                {
                    usuarios.Add(MapearFilaAEntidad(accesoDatos.Lector));
                }

                return usuarios;
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

        public Usuario ObtenerPorId(int idUsuario)
        {
            Usuario usuario = null;
            try
            {
                accesoDatos.setearConsulta(
                    ObtenerConsultaBase()
                    + " WHERE U.IdUsuario = @idUsuario");
                accesoDatos.setearParametro("@idUsuario", idUsuario);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    usuario = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return usuario;
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

        public Usuario ObtenerPorIdPersona(int idPersona)
        {
            Usuario usuario = null;
            try
            {
                accesoDatos.setearConsulta(
                    ObtenerConsultaBase()
                    + " WHERE U.IdPersona = @idPersona");
                accesoDatos.setearParametro("@idPersona", idPersona);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    usuario = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return usuario;
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

        public Usuario ValidarCredenciales(string nombreUsuario, string password)
        {
            try
            {
                accesoDatos.setearConsulta(
                    ObtenerConsultaBase()
                    + " WHERE U.NombreUsuario = @nombreUsuario"
                    + " AND U.PasswordHash = @password"
                    + " AND UPPER(EU.Nombre) = UPPER(@estadoUsuarioActivo)");
                accesoDatos.setearParametro("@nombreUsuario", nombreUsuario);
                accesoDatos.setearParametro("@password", password);
                accesoDatos.setearParametro("@estadoUsuarioActivo", EstadoUsuarioEnum.Activo.ToString());
                accesoDatos.ejecutarLectura();

                Usuario usuario = null;
                if (accesoDatos.Lector.Read())
                {
                    usuario = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return usuario;
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

        public bool ExisteNombreUsuario(string nombreUsuario)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT COUNT(1)"
                    + " FROM Usuarios"
                    + " WHERE NombreUsuario = @nombreUsuario");
                accesoDatos.setearParametro("@nombreUsuario", nombreUsuario);
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

        public void Agregar(Usuario usuario)
        {
            try
            {
                Rol rol = rolDatos.ObtenerPorNombre(usuario.Rol != null ? usuario.Rol.Nombre : null);
                EstadoUsuario estadoUsuario = estadoUsuarioDatos.ObtenerPorNombre(usuario.EstadoUsuario != null ? usuario.EstadoUsuario.Nombre : null);
                int? idRol = rol != null ? rol.IdRol : (int?)null;
                int? idEstadoUsuario = estadoUsuario != null ? estadoUsuario.IdEstadoUsuario : (int?)null;
                if (!idRol.HasValue)
                {
                    throw new Exception("El rol del usuario no existe en la tabla de roles.");
                }

                if (!idEstadoUsuario.HasValue)
                {
                    throw new Exception("El estado del usuario no existe en la tabla de estados de usuario.");
                }

                accesoDatos.setearConsulta(
                    "INSERT INTO Usuarios (IdPersona, NombreUsuario, PasswordHash, Imagen, IdRol, IdEstadoUsuario)"
                    + " VALUES (@idPersona, @nombreUsuario, @passwordHash, @imagen, @idRol, @idEstadoUsuario)");
                accesoDatos.setearParametro("@idPersona", usuario.Persona.IdPersona);
                accesoDatos.setearParametro("@nombreUsuario", usuario.NombreUsuario);
                accesoDatos.setearParametro("@passwordHash", usuario.PasswordHash);
                accesoDatos.setearParametro("@imagen", usuario.Imagen != null ? (object)usuario.Imagen : DBNull.Value);
                accesoDatos.setearParametro("@idRol", idRol.Value);
                accesoDatos.setearParametro("@idEstadoUsuario", idEstadoUsuario.Value);

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

        public void Modificar(Usuario usuario)
        {
            try
            {
                Rol rol = rolDatos.ObtenerPorNombre(usuario.Rol != null ? usuario.Rol.Nombre : null);
                EstadoUsuario estadoUsuario = estadoUsuarioDatos.ObtenerPorNombre(usuario.EstadoUsuario != null ? usuario.EstadoUsuario.Nombre : null);
                int? idRol = rol != null ? rol.IdRol : (int?)null;
                int? idEstadoUsuario = estadoUsuario != null ? estadoUsuario.IdEstadoUsuario : (int?)null;
                if (!idRol.HasValue)
                {
                    throw new Exception("El rol del usuario no existe en la tabla de roles.");
                }

                if (!idEstadoUsuario.HasValue)
                {
                    throw new Exception("El estado del usuario no existe en la tabla de estados de usuario.");
                }

                accesoDatos.setearConsulta(
                    "UPDATE Usuarios SET"
                    + " IdPersona = @idPersona,"
                    + " NombreUsuario = @nombreUsuario,"
                    + " PasswordHash = @passwordHash,"
                    + " Imagen = @imagen,"
                    + " IdEstadoUsuario = @idEstadoUsuario,"
                    + " IdRol = @idRol"
                    + " WHERE IdUsuario = @idUsuario");
                accesoDatos.setearParametro("@idUsuario", usuario.IdUsuario);
                accesoDatos.setearParametro("@idPersona", usuario.Persona.IdPersona);
                accesoDatos.setearParametro("@nombreUsuario", usuario.NombreUsuario);
                accesoDatos.setearParametro("@passwordHash", usuario.PasswordHash);
                accesoDatos.setearParametro("@imagen", usuario.Imagen != null ? (object)usuario.Imagen : DBNull.Value);
                accesoDatos.setearParametro("@idRol", idRol.Value);
                accesoDatos.setearParametro("@idEstadoUsuario", idEstadoUsuario.Value);

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

        public void Desactivar(int idUsuario)
        {
            try
            {
                EstadoUsuario estadoUsuario = estadoUsuarioDatos.ObtenerPorNombre(EstadoUsuarioEnum.Inactivo.ToString());

                accesoDatos.setearConsulta("UPDATE Usuarios SET IdEstadoUsuario = @idEstadoUsuario WHERE IdUsuario = @idUsuario");
                accesoDatos.setearParametro("@idUsuario", idUsuario);
                accesoDatos.setearParametro("@idEstadoUsuario", estadoUsuario.IdEstadoUsuario);
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

        public void AltaLogica(int idUsuario)
        {
            try
            {
                EstadoUsuario estadoUsuario = estadoUsuarioDatos.ObtenerPorNombre(EstadoUsuarioEnum.Activo.ToString());
                if (estadoUsuario == null)
                {
                    throw new Exception("El estado activo del usuario no existe en la tabla de estados de usuario.");
                }

                accesoDatos.setearConsulta("UPDATE Usuarios SET IdEstadoUsuario = @idEstadoUsuario WHERE IdUsuario = @idUsuario");
                accesoDatos.setearParametro("@idUsuario", idUsuario);
                accesoDatos.setearParametro("@idEstadoUsuario", estadoUsuario.IdEstadoUsuario);
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

        public Usuario MapearFilaAEntidad(SqlDataReader fila)
        {
            Usuario usuario = new Usuario
            {
                IdUsuario = Convert.ToInt32(fila["IdUsuario"]),
                NombreUsuario = fila["NombreUsuario"] is DBNull ? null : fila["NombreUsuario"].ToString(),
                Persona = new Persona
                {
                    IdPersona = Convert.ToInt32(fila["IdPersona"]),
                    DNI = fila["DNI"].ToString(),
                    Nombre = fila["Nombre"].ToString(),
                    Apellido = fila["Apellido"].ToString(),
                    Telefono = fila["Telefono"] is DBNull ? string.Empty : fila["Telefono"].ToString(),
                    Email = fila["Email"].ToString()
                },
                PasswordHash = fila["PasswordHash"] is DBNull ? null : fila["PasswordHash"].ToString(),
                Imagen = fila["Imagen"] is DBNull ? null : (byte[])fila["Imagen"],
                Rol = MapearRol(fila),
                EstadoUsuario = MapearEstadoUsuario(fila)
            };

            return usuario;
        }

        public List<Usuario> ListarConFiltros(string campo, string criterio, string filtro, bool? activo)
        {
            return Listar(null, activo.HasValue
                ? (activo.Value ? EstadoUsuarioEnum.Activo.ToString() : EstadoUsuarioEnum.Inactivo.ToString())
                : null);
        }

        private string ObtenerConsultaBase()
        {
            return "SELECT U.IdUsuario, U.IdPersona, U.NombreUsuario, U.PasswordHash, U.Imagen, "
                + "       U.IdRol, U.IdEstadoUsuario, "
                + "       P.DNI, P.Nombre, P.Apellido, P.Telefono, P.Email, "
                + "       R.Nombre AS NombreRol, R.Descripcion AS DescripcionRol, R.Activo AS ActivoRol, "
                + "       EU.Nombre AS NombreEstadoUsuario, EU.Descripcion AS DescripcionEstadoUsuario, EU.Activo AS ActivoEstadoUsuario "
                + "FROM Usuarios U "
                + "INNER JOIN Personas P ON U.IdPersona = P.IdPersona "
                + "INNER JOIN Roles R ON U.IdRol = R.IdRol "
                + "INNER JOIN EstadosUsuario EU ON U.IdEstadoUsuario = EU.IdEstadoUsuario";
        }

        private Rol MapearRol(SqlDataReader fila)
        {
            if (fila["IdRol"] == DBNull.Value)
            {
                return null;
            }

            return new Rol
            {
                IdRol = Convert.ToInt32(fila["IdRol"]),
                Nombre = fila["NombreRol"] is DBNull ? null : fila["NombreRol"].ToString(),
                Descripcion = fila["DescripcionRol"] is DBNull ? string.Empty : fila["DescripcionRol"].ToString(),
                Activo = fila["ActivoRol"] is DBNull ? false : Convert.ToBoolean(fila["ActivoRol"])
            };
        }

        private EstadoUsuario MapearEstadoUsuario(SqlDataReader fila)
        {
            if (fila["IdEstadoUsuario"] == DBNull.Value)
            {
                return null;
            }

            return new EstadoUsuario
            {
                IdEstadoUsuario = Convert.ToInt32(fila["IdEstadoUsuario"]),
                Nombre = fila["NombreEstadoUsuario"] is DBNull ? null : fila["NombreEstadoUsuario"].ToString(),
                Descripcion = fila["DescripcionEstadoUsuario"] is DBNull ? string.Empty : fila["DescripcionEstadoUsuario"].ToString(),
                Activo = fila["ActivoEstadoUsuario"] is DBNull ? false : Convert.ToBoolean(fila["ActivoEstadoUsuario"])
            };
        }
    }
}
