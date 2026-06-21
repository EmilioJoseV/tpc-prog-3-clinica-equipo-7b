using System;
using System.Collections.Generic;

namespace TurnosClinica.Dominio.Entidades
{
    public class Paciente : Persona
    {
        public Paciente()
        {
            Activo = true;
            Turnos = new List<Turno>();
        }

        public int IdPaciente { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; }
        public bool Activo { get; set; }
        public List<Turno> Turnos { get; set; }
    }
}
