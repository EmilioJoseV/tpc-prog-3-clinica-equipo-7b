using System;
using System.Collections.Generic;

namespace TurnosClinica.Dominio.Entidades
{
    public class Paciente
    {
        public Paciente()
        {
            Turnos = new List<Turno>();
        }

        public int IdPaciente { get; set; }
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public bool Activo { get; set; } = true;
        public List<Turno> Turnos { get; set; }
    }
}
