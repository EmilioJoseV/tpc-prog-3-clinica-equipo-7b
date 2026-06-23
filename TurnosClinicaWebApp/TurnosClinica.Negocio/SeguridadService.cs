using System.Security.Cryptography;
using System.Text;

namespace TurnosClinica.Negocio
{
    public class SeguridadService
    {
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
