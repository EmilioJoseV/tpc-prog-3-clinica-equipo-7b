using System.Collections.Generic;

namespace TurnosClinica.Dominio.Entidades
{
    public class Medico
    {
        public Medico()
        {
            Activo = true;
            Persona = new Persona();
            Especialidades = new List<Especialidad>();
            HorariosDisponibilidad = new List<HorarioDisponibilidadMedico>();
        }

        public int IdMedico { get; set; }
        public Persona Persona { get; set; }
        public string Matricula { get; set; }
        public bool Activo { get; set; }
        public List<Especialidad> Especialidades { get; set; }
        public List<HorarioDisponibilidadMedico> HorariosDisponibilidad { get; set; }
    }
}
