using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.AccesoDatos
{
    public class UsuarioDatos : IFiltrable<Usuario>, IMapeable<Usuario>
    {
        private readonly AccesoDatos accesoDatos;

        public UsuarioDatos()
        {
            accesoDatos = new AccesoDatos();
        }

        public List<Usuario> Listar(int? idRol, int? idMedico, bool? activo)
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                string consulta = "SELECT IdUsuario, NombreUsuario, Email, PasswordHash, Activo, IdRol, IdMedico FROM Usuarios WHERE 1=1";

                if (idRol.HasValue) consulta += " AND IdRol = @idRol";
                if (idMedico.HasValue) consulta += " AND IdMedico = @idMedico";
                if (activo.HasValue) consulta += " AND Activo = @activo";

                accesoDatos.setearConsulta(consulta);

                if (idRol.HasValue) accesoDatos.setearParametro("@idRol", idRol.Value);
                if (idMedico.HasValue) accesoDatos.setearParametro("@idMedico", idMedico.Value);
                if (activo.HasValue) accesoDatos.setearParametro("@activo", activo.Value);

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
        }

        public Usuario ObtenerPorId(int idUsuario)
        {
            Usuario usuario = new Usuario();
            try
            {
                accesoDatos.setearConsulta(
                    "SELECT IdUsuario, NombreUsuario, Email, PasswordHash, Activo, IdRol, IdMedico"
                    + " FROM Usuarios"
                    + " WHERE IdUsuario = @idUsuario");
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
        }

        public Usuario ValidarCredenciales(string nombreUsuario, string password)
        {
            Usuario usuario = new Usuario();
            try
            {
                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExisteNombreUsuario(string nombreUsuario)
        {
            try
            {
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExisteEmail(string email)
        {
            try
            {
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Agregar(Usuario usuario)
        {
            try
            {
                accesoDatos.setearConsulta(
                    "INSERT INTO Usuarios (NombreUsuario, Nombre, Apellido, Email, PasswordHash, Imagen, Activo, IdRol, IdMedico) " +
                    "VALUES (@nombreUsuario, @nombreDefecto, @apellidoDefecto, @email, @passwordHash, NULL, @activo, @idRol, @idMedico)");

                accesoDatos.setearParametro("@nombreUsuario", usuario.NombreUsuario);
                accesoDatos.setearParametro("@nombreDefecto", string.Empty);
                accesoDatos.setearParametro("@apellidoDefecto", string.Empty);
                accesoDatos.setearParametro("@email", usuario.Email);
                accesoDatos.setearParametro("@passwordHash", usuario.PasswordHash);
                accesoDatos.setearParametro("@activo", usuario.Activo);

                accesoDatos.setearParametro("@idRol", usuario.Rol != null ? (object)usuario.Rol.IdRol : DBNull.Value);
                accesoDatos.setearParametro("@idMedico", usuario.Medico != null ? (object)usuario.Medico.IdMedico : DBNull.Value);

                accesoDatos.ejecutarAccion();
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificar(Usuario usuario)
        {
            try
            {
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Usuario MapearFilaAEntidad(SqlDataReader fila)
        {
            Usuario usuario = new Usuario();
            usuario.IdUsuario = Convert.ToInt32(fila["IdUsuario"]);
            usuario.NombreUsuario = fila["NombreUsuario"].ToString();
            usuario.Email = fila["Email"].ToString();
            usuario.PasswordHash = fila["PasswordHash"] is DBNull ? string.Empty : fila["PasswordHash"].ToString();
            usuario.Activo = Convert.ToBoolean(fila["Activo"]);

            if (fila["IdRol"] != DBNull.Value)
            {
                usuario.Rol = new Rol
                {
                    IdRol = Convert.ToInt32(fila["IdRol"])
                };
            }

            if (fila["IdMedico"] != DBNull.Value)
            {
                usuario.Medico = new Medico
                {
                    IdMedico = Convert.ToInt32(fila["IdMedico"])
                };
            }

            return usuario;
        }

        public List<Usuario> ListarConFiltros(string campo, string criterio, string filtro, bool? activo)
        {
            throw new NotImplementedException();
        }
    }
}
