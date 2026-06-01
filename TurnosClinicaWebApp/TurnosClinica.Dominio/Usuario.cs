using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnosClinica.Dominio
{
    public class Usuario
    {
        public int IdUsuario{ get; set; }
        public string NombreUsuario { get; set; }

        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int IdRol { get; set; }
        public int ? IdMedico { get; set; }
        public bool Activo { get; set; }
        
    }
}
