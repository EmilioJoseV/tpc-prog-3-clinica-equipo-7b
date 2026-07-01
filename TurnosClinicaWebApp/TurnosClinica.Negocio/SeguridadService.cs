using System;
using System.Security.Cryptography;
using System.Text;

namespace TurnosClinica.Negocio
{
    public class SeguridadService
    {
        public void ValidarNuevaContrasena(string nuevaContrasena, string confirmacionContrasena)
        {
            if (string.IsNullOrWhiteSpace(nuevaContrasena))
            {
                throw new Exception("Debe ingresar la nueva contrasena.");
            }

            if (nuevaContrasena.Trim().Length < 8)
            {
                throw new Exception("La nueva contrasena debe tener al menos 8 caracteres.");
            }

            if (!string.Equals(nuevaContrasena, confirmacionContrasena, StringComparison.Ordinal))
            {
                throw new Exception("La confirmacion de contrasena no coincide.");
            }
        }

        public string CalcularHash(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return null;
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(texto);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder resultado = new StringBuilder();

                foreach (byte valor in hash)
                {
                    resultado.Append(valor.ToString("x2"));
                }

                return resultado.ToString();
            }
        }
    }
}
