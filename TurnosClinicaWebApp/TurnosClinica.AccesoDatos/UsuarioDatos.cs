using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.AccesoDatos
{
    public class UsuarioDatos : IMapeable<Usuario>
    {
        private readonly AccesoDatos accesoDatos;
        private readonly PersonaDatos personaDatos;
        private readonly RolDatos rolDatos;
        private readonly EstadoUsuarioDatos estadoUsuarioDatos;

        public UsuarioDatos()
        {
            accesoDatos = new AccesoDatos();
            personaDatos = new PersonaDatos(accesoDatos);
            rolDatos = new RolDatos(accesoDatos);
            estadoUsuarioDatos = new EstadoUsuarioDatos(accesoDatos);
        }

        public UsuarioDatos(AccesoDatos accesoDatosCompartido)
        {
            accesoDatos = accesoDatosCompartido;
            personaDatos = new PersonaDatos(accesoDatosCompartido);
            rolDatos = new RolDatos(accesoDatosCompartido);
            estadoUsuarioDatos = new EstadoUsuarioDatos(accesoDatosCompartido);
        }

        public List<Usuario> Listar(string rolLiteral, string estadoUsuarioLiteral)
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                string consulta = "SELECT U.IdUsuario, U.IdPersona, U.NombreUsuario, U.PasswordHash, U.Imagen, "
                    + "       P.DNI, P.Nombre, P.Apellido, P.Telefono, P.Email, "
                    + "       R.Nombre AS NombreRol, "
                    + "       EU.Nombre AS NombreEstadoUsuario "
                    + "FROM Usuarios U "
                    + "INNER JOIN Personas P ON U.IdPersona = P.IdPersona "
                    + "INNER JOIN Roles R ON U.IdRol = R.IdRol "
                    + "INNER JOIN EstadosUsuario EU ON U.IdEstadoUsuario = EU.IdEstadoUsuario "
                    + "WHERE 1=1";

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
                    "SELECT U.IdUsuario, U.IdPersona, U.NombreUsuario, U.PasswordHash, U.Imagen,"
                    + " P.DNI, P.Nombre, P.Apellido, P.Telefono, P.Email,"
                    + " R.Nombre AS NombreRol,"
                    + " EU.Nombre AS NombreEstadoUsuario"
                    + " FROM Usuarios U"
                    + " INNER JOIN Personas P ON U.IdPersona = P.IdPersona"
                    + " INNER JOIN Roles R ON U.IdRol = R.IdRol"
                    + " INNER JOIN EstadosUsuario EU ON U.IdEstadoUsuario = EU.IdEstadoUsuario"
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
                    "SELECT U.IdUsuario, U.IdPersona, U.NombreUsuario, U.PasswordHash, U.Imagen,"
                    + " P.DNI, P.Nombre, P.Apellido, P.Telefono, P.Email,"
                    + " R.Nombre AS NombreRol,"
                    + " EU.Nombre AS NombreEstadoUsuario"
                    + " FROM Usuarios U"
                    + " INNER JOIN Personas P ON U.IdPersona = P.IdPersona"
                    + " INNER JOIN Roles R ON U.IdRol = R.IdRol"
                    + " INNER JOIN EstadosUsuario EU ON U.IdEstadoUsuario = EU.IdEstadoUsuario"
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
                    "SELECT U.IdUsuario, U.IdPersona, U.NombreUsuario, U.PasswordHash, U.Imagen,"
                    + " P.DNI, P.Nombre, P.Apellido, P.Telefono, P.Email,"
                    + " R.Nombre AS NombreRol,"
                    + " EU.Nombre AS NombreEstadoUsuario"
                    + " FROM Usuarios U"
                    + " INNER JOIN Personas P ON U.IdPersona = P.IdPersona"
                    + " INNER JOIN Roles R ON U.IdRol = R.IdRol"
                    + " INNER JOIN EstadosUsuario EU ON U.IdEstadoUsuario = EU.IdEstadoUsuario"
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

        public void Agregar(Usuario usuario)
        {
            try
            {
                if (usuario.IdPersona <= 0)
                {
                    usuario.IdPersona = personaDatos.Agregar(usuario);
                }
                else
                {
                    personaDatos.Modificar(usuario);
                }

                accesoDatos.setearConsulta(
                    "INSERT INTO Usuarios (IdPersona, NombreUsuario, PasswordHash, Imagen, IdRol, IdEstadoUsuario)"
                    + " VALUES (@idPersona, @nombreUsuario, @passwordHash, @imagen, @idRol, @idEstadoUsuario)");

                Rol rol = rolDatos.ObtenerPorNombre(usuario.Rol != null ? usuario.Rol.Nombre : null);
                EstadoUsuario estadoUsuario = estadoUsuarioDatos.ObtenerPorNombre(usuario.EstadoUsuario.ToString());
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

                accesoDatos.setearParametro("@idPersona", usuario.IdPersona);
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
                personaDatos.Modificar(usuario);

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
                accesoDatos.setearParametro("@idPersona", usuario.IdPersona);
                accesoDatos.setearParametro("@nombreUsuario", usuario.NombreUsuario);
                accesoDatos.setearParametro("@passwordHash", usuario.PasswordHash);
                accesoDatos.setearParametro("@imagen", usuario.Imagen != null ? (object)usuario.Imagen : DBNull.Value);
                Rol rol = rolDatos.ObtenerPorNombre(usuario.Rol != null ? usuario.Rol.Nombre : null);
                EstadoUsuario estadoUsuario = estadoUsuarioDatos.ObtenerPorNombre(usuario.EstadoUsuario.ToString());
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

        public void EliminarFisico(int idUsuario)
        {
            try
            {
                accesoDatos.setearConsulta("DELETE FROM Usuarios WHERE IdUsuario = @idUsuario");
                accesoDatos.setearParametro("@idUsuario", idUsuario);
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

        public void EliminarLogico(int idUsuario)
        {
            try
            {
                accesoDatos.setearConsulta("UPDATE Usuarios SET IdEstadoUsuario = @idEstadoUsuario WHERE IdUsuario = @idUsuario");
                accesoDatos.setearParametro("@idUsuario", idUsuario);
                EstadoUsuario estadoUsuario = estadoUsuarioDatos.ObtenerPorNombre(EstadoUsuarioEnum.Inactivo.ToString());
                accesoDatos.setearParametro("@idEstadoUsuario", estadoUsuario != null ? estadoUsuario.IdEstadoUsuario : 0);
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
                accesoDatos.setearConsulta("UPDATE Usuarios SET IdEstadoUsuario = @idEstadoUsuario WHERE IdUsuario = @idUsuario");
                accesoDatos.setearParametro("@idUsuario", idUsuario);
                EstadoUsuario estadoUsuario = estadoUsuarioDatos.ObtenerPorNombre(EstadoUsuarioEnum.Activo.ToString());
                accesoDatos.setearParametro("@idEstadoUsuario", estadoUsuario != null ? estadoUsuario.IdEstadoUsuario : 0);
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
                IdPersona = Convert.ToInt32(fila["IdPersona"]),
                NombreUsuario = fila["NombreUsuario"] is DBNull ? null : fila["NombreUsuario"].ToString(),
                DNI = fila["DNI"].ToString(),
                Nombre = fila["Nombre"].ToString(),
                Apellido = fila["Apellido"].ToString(),
                Telefono = fila["Telefono"] is DBNull ? string.Empty : fila["Telefono"].ToString(),
                Email = fila["Email"].ToString(),
                PasswordHash = fila["PasswordHash"] is DBNull ? null : fila["PasswordHash"].ToString(),
                Imagen = fila["Imagen"] is DBNull ? null : (byte[])fila["Imagen"],
                EstadoUsuario = Enum.TryParse(fila["NombreEstadoUsuario"]?.ToString(), true, out EstadoUsuarioEnum estado)
                    ? estado
                    : EstadoUsuarioEnum.Activo
            };

            if (fila["NombreRol"] != DBNull.Value)
            {
                usuario.Rol = new Rol
                {
                    Nombre = fila["NombreRol"] is DBNull ? string.Empty : fila["NombreRol"].ToString()
                };
            }

            return usuario;
        }

        public List<Usuario> ListarConFiltros(string campo, string criterio, string filtro, bool? activo)
        {
            return Listar(null, activo.HasValue
                ? (activo.Value ? EstadoUsuarioEnum.Activo.ToString() : EstadoUsuarioEnum.Inactivo.ToString())
                : null);
        }

    }
}
