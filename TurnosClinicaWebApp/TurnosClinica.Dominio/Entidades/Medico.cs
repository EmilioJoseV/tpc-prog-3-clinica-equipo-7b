using System.Collections.Generic;

namespace TurnosClinica.Dominio.Entidades
{
    public class Medico : Persona
    {
        public Medico()
        {
            Activo = true;
            Especialidades = new List<Especialidad>();
            HorariosDisponibilidad = new List<HorarioDisponibilidadMedico>();
        }

        public int IdMedico { get; set; }
        public string Matricula { get; set; }
        public bool Activo { get; set; }
        public List<Especialidad> Especialidades { get; set; }
        public List<HorarioDisponibilidadMedico> HorariosDisponibilidad { get; set; }
    }
}
