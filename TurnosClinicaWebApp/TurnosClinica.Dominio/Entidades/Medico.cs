using System.Collections.Generic;

namespace TurnosClinica.Dominio.Entidades
{
    public class Medico
    {
        public Medico()
        {
            MedicoEspecialidades = new List<MedicoEspecialidad>();
            DiasAtencion = new List<DiaAtencionMedico>();
            Turnos = new List<Turno>();
            Usuarios = new List<Usuario>();
        }

        public int IdMedico { get; set; }
        public string Matricula { get; set; }
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int IdTurnoTrabajo { get; set; }
        public bool Activo { get; set; } = true;

        public TurnoTrabajo TurnoTrabajo { get; set; }
        public List<MedicoEspecialidad> MedicoEspecialidades { get; set; }
        public List<DiaAtencionMedico> DiasAtencion { get; set; }
        public List<Turno> Turnos { get; set; }
        public List<Usuario> Usuarios { get; set; }
    }
}
