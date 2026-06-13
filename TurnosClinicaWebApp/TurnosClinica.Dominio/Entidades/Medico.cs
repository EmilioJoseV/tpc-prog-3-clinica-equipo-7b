using System.Collections.Generic;

namespace TurnosClinica.Dominio.Entidades
{
    public class Medico
    {
        public Medico()
        {
            Especialidades = new List<Especialidad>();
            HorariosDisponibilidad = new List<HorarioDisponibilidadMedico>();
        }

        public int IdMedico { get; set; }
        public string Matricula { get; set; }
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; } = true;
        public List<Especialidad> Especialidades { get; set; }
        public List<HorarioDisponibilidadMedico> HorariosDisponibilidad { get; set; }
    }
}
