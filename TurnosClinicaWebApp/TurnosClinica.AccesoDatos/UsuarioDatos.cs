using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;
using TurnosClinica.Dominio.Enums;

namespace TurnosClinica.AccesoDatos
{
    public class UsuarioDatos : IEntidadGestionable<Usuario>, IMapeable<Usuario>
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

        public List<Usuario> Listar(bool? activo = null)
        {
            return ListarFiltroAvanzado(null, null, null, activo);
        }

        public List<Usuario> ListarFiltroRapido(string palabra, bool? activo = null)
        {
            return ListarFiltroAvanzado(null, null, palabra, activo);
        }

        public List<Usuario> ListarFiltroAvanzado(string campo, string criterio, string filtro, bool? activo = null)
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                string consulta = ObtenerConsultaBase()
                    + " WHERE (UPPER(R.Nombre) = UPPER(@rolAdministrador)"
                    + " OR UPPER(R.Nombre) = UPPER(@rolRecepcionista))";

                if (activo.HasValue)
                {
                    consulta += activo.Value
                        ? " AND UPPER(EU.Nombre) <> UPPER(@estadoInactivo)"
                        : " AND UPPER(EU.Nombre) = UPPER(@estadoInactivo)";
                }

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    consulta += ConstruirFiltro(campo, criterio);
                }

                consulta += " ORDER BY P.Apellido, P.Nombre";

                accesoDatos.setearConsulta(consulta);
                accesoDatos.setearParametro("@rolAdministrador", RolEnum.Administrador.ToString());
                accesoDatos.setearParametro("@rolRecepcionista", RolEnum.Recepcionista.ToString());

                if (activo.HasValue)
                {
                    accesoDatos.setearParametro("@estadoInactivo", EstadoUsuarioEnum.Inactivo.ToString());
                }

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    accesoDatos.setearParametro("@filtro", filtro.Trim());
                }

                accesoDatos.ejecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    usuarios.Add(MapearFilaAEntidad(accesoDatos.Lector));
                }

                return usuarios;
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

        public Usuario ObtenerPorId(int id)
        {
            Usuario usuario = null;

            try
            {
                accesoDatos.setearConsulta(
                    ObtenerConsultaBase()
                    + " WHERE U.IdUsuario = @idUsuario");
                accesoDatos.setearParametro("@idUsuario", id);
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    usuario = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return usuario;
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
            catch
            {
                throw;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public Usuario ValidarCredenciales(string nombreUsuario, string password)
        {
            Usuario usuario = null;

            try
            {
                accesoDatos.setearConsulta(
                    ObtenerConsultaBase()
                    + " WHERE U.NombreUsuario = @nombreUsuario"
                    + " AND U.PasswordHash = @password"
                    + " AND UPPER(EU.Nombre) = UPPER(@estadoActivo)");
                accesoDatos.setearParametro("@nombreUsuario", nombreUsuario);
                accesoDatos.setearParametro("@password", password);
                accesoDatos.setearParametro("@estadoActivo", EstadoUsuarioEnum.Activo.ToString());
                accesoDatos.ejecutarLectura();

                if (accesoDatos.Lector.Read())
                {
                    usuario = MapearFilaAEntidad(accesoDatos.Lector);
                }

                return usuario;
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

        public bool ExisteNombreUsuario(string nombreUsuario, int excluirId = 0)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT COUNT(1) FROM Usuarios"
                    + " WHERE UPPER(NombreUsuario) = UPPER(@nombreUsuario)"
                    + " AND IdUsuario <> @excluirId");
                accesoDatos.setearParametro("@nombreUsuario", nombreUsuario);
                accesoDatos.setearParametro("@excluirId", excluirId);
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

        public int Agregar(Usuario usuario)
        {
            try
            {
                Rol rol = rolDatos.ObtenerPorNombre(usuario.Rol.Nombre);
                EstadoUsuario estado = estadoUsuarioDatos.ObtenerPorNombre(usuario.EstadoUsuario.Nombre);
                ValidarReferencias(rol, estado);

                accesoDatos.setearConsulta(
                    "INSERT INTO Usuarios"
                    + " (IdPersona, NombreUsuario, PasswordHash, Imagen, IdRol, IdEstadoUsuario)"
                    + " VALUES (@idPersona, @nombreUsuario, @passwordHash, @imagen, @idRol, @idEstadoUsuario)");
                accesoDatos.setearParametro("@idPersona", usuario.Persona.IdPersona);
                accesoDatos.setearParametro("@nombreUsuario", string.IsNullOrWhiteSpace(usuario.NombreUsuario)
                    ? (object)DBNull.Value
                    : usuario.NombreUsuario);
                accesoDatos.setearParametro("@passwordHash", string.IsNullOrWhiteSpace(usuario.PasswordHash)
                    ? (object)DBNull.Value
                    : usuario.PasswordHash);
                accesoDatos.setearParametro("@imagen", usuario.Imagen != null ? (object)usuario.Imagen : DBNull.Value);
                accesoDatos.setearParametro("@idRol", rol.IdRol);
                accesoDatos.setearParametro("@idEstadoUsuario", estado.IdEstadoUsuario);
                accesoDatos.ejecutarAccion();
                return 0;
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

        public void Modificar(Usuario usuario)
        {
            try
            {
                Rol rol = rolDatos.ObtenerPorNombre(usuario.Rol.Nombre);
                EstadoUsuario estado = estadoUsuarioDatos.ObtenerPorNombre(usuario.EstadoUsuario.Nombre);
                ValidarReferencias(rol, estado);

                accesoDatos.setearConsulta(
                    "UPDATE Usuarios SET"
                    + " NombreUsuario = @nombreUsuario,"
                    + " PasswordHash = @passwordHash,"
                    + " Imagen = @imagen,"
                    + " IdRol = @idRol,"
                    + " IdEstadoUsuario = @idEstadoUsuario"
                    + " WHERE IdUsuario = @idUsuario");
                accesoDatos.setearParametro("@idUsuario", usuario.IdUsuario);
                accesoDatos.setearParametro("@nombreUsuario", string.IsNullOrWhiteSpace(usuario.NombreUsuario)
                    ? (object)DBNull.Value
                    : usuario.NombreUsuario);
                accesoDatos.setearParametro("@passwordHash", string.IsNullOrWhiteSpace(usuario.PasswordHash)
                    ? (object)DBNull.Value
                    : usuario.PasswordHash);
                accesoDatos.setearParametro("@imagen", usuario.Imagen != null ? (object)usuario.Imagen : DBNull.Value);
                accesoDatos.setearParametro("@idRol", rol.IdRol);
                accesoDatos.setearParametro("@idEstadoUsuario", estado.IdEstadoUsuario);
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

        public void Desactivar(int id)
        {
            CambiarEstado(id, EstadoUsuarioEnum.Inactivo);
        }

        public void Activar(int id)
        {
            CambiarEstado(id, EstadoUsuarioEnum.Activo);
        }

        public Usuario MapearFilaAEntidad(SqlDataReader fila)
        {
            return new Usuario
            {
                IdUsuario = Convert.ToInt32(fila["IdUsuario"]),
                NombreUsuario = fila["NombreUsuario"] is DBNull ? null : fila["NombreUsuario"].ToString(),
                PasswordHash = fila["PasswordHash"] is DBNull ? null : fila["PasswordHash"].ToString(),
                Imagen = fila["Imagen"] is DBNull ? null : (byte[])fila["Imagen"],
                Persona = new Persona
                {
                    IdPersona = Convert.ToInt32(fila["IdPersona"]),
                    DNI = fila["DNI"].ToString(),
                    Nombre = fila["Nombre"].ToString(),
                    Apellido = fila["Apellido"].ToString(),
                    Telefono = fila["Telefono"] is DBNull ? string.Empty : fila["Telefono"].ToString(),
                    Email = fila["Email"].ToString()
                },
                Rol = new Rol
                {
                    IdRol = Convert.ToInt32(fila["IdRol"]),
                    Nombre = fila["NombreRol"].ToString(),
                    Descripcion = fila["DescripcionRol"] is DBNull ? string.Empty : fila["DescripcionRol"].ToString(),
                    Activo = Convert.ToBoolean(fila["ActivoRol"])
                },
                EstadoUsuario = new EstadoUsuario
                {
                    IdEstadoUsuario = Convert.ToInt32(fila["IdEstadoUsuario"]),
                    Nombre = fila["NombreEstadoUsuario"].ToString(),
                    Descripcion = fila["DescripcionEstadoUsuario"] is DBNull
                        ? string.Empty
                        : fila["DescripcionEstadoUsuario"].ToString(),
                    Activo = Convert.ToBoolean(fila["ActivoEstadoUsuario"])
                }
            };
        }

        private string ObtenerConsultaBase()
        {
            return "SELECT U.IdUsuario, U.IdPersona, U.NombreUsuario, U.PasswordHash, U.Imagen,"
                + " U.IdRol, U.IdEstadoUsuario,"
                + " P.DNI, P.Nombre, P.Apellido, P.Telefono, P.Email,"
                + " R.Nombre AS NombreRol, R.Descripcion AS DescripcionRol, R.Activo AS ActivoRol,"
                + " EU.Nombre AS NombreEstadoUsuario,"
                + " EU.Descripcion AS DescripcionEstadoUsuario,"
                + " EU.Activo AS ActivoEstadoUsuario"
                + " FROM Usuarios U"
                + " INNER JOIN Personas P ON P.IdPersona = U.IdPersona"
                + " INNER JOIN Roles R ON R.IdRol = U.IdRol"
                + " INNER JOIN EstadosUsuario EU ON EU.IdEstadoUsuario = U.IdEstadoUsuario";
        }

        private string ConstruirFiltro(string campo, string criterio)
        {
            string campoSql = ObtenerCampoSql(campo);

            if (string.IsNullOrWhiteSpace(campoSql))
            {
                return " AND (P.DNI LIKE '%' + @filtro + '%'"
                    + " OR P.Nombre LIKE '%' + @filtro + '%'"
                    + " OR P.Apellido LIKE '%' + @filtro + '%'"
                    + " OR P.Email LIKE '%' + @filtro + '%'"
                    + " OR R.Nombre LIKE '%' + @filtro + '%'"
                    + " OR EU.Nombre LIKE '%' + @filtro + '%'"
                    + " OR U.NombreUsuario LIKE '%' + @filtro + '%')";
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
                case "DNI":
                    return "P.DNI";
                case "Nombre":
                    return "P.Nombre";
                case "Apellido":
                    return "P.Apellido";
                case "Email":
                    return "P.Email";
                case "Rol":
                    return "R.Nombre";
                case "Estado":
                    return "EU.Nombre";
                case "NombreUsuario":
                    return "U.NombreUsuario";
                default:
                    return null;
            }
        }

        private void CambiarEstado(int id, EstadoUsuarioEnum estadoRequerido)
        {
            try
            {
                EstadoUsuario estado = estadoUsuarioDatos.ObtenerPorNombre(estadoRequerido.ToString());
                if (estado == null)
                {
                    throw new Exception("El estado del usuario no existe.");
                }

                accesoDatos.setearConsulta(
                    "UPDATE Usuarios SET IdEstadoUsuario = @idEstadoUsuario"
                    + " WHERE IdUsuario = @idUsuario");
                accesoDatos.setearParametro("@idUsuario", id);
                accesoDatos.setearParametro("@idEstadoUsuario", estado.IdEstadoUsuario);
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

        private void ValidarReferencias(Rol rol, EstadoUsuario estado)
        {
            if (rol == null)
            {
                throw new Exception("El rol del usuario no existe.");
            }

            if (estado == null)
            {
                throw new Exception("El estado del usuario no existe.");
            }
        }
    }
}
