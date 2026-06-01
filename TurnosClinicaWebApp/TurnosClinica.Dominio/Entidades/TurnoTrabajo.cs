using System;
using System.Collections.Generic;

namespace TurnosClinica.Dominio.Entidades
{
    public class TurnoTrabajo
    {
        public TurnoTrabajo()
        {
            Medicos = new List<Medico>();
        }

        public int IdTurnoTrabajo { get; set; }
        public string Nombre { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public bool Activo { get; set; } = true;

        public List<Medico> Medicos { get; set; }
    }
}
