using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnosClinica.AccesoDatos;
using TurnosClinica.Negocio.DTO;

namespace TurnosClinica.Negocio
{
    public class TurnoCalculoService
    {
        private readonly ConfiguracionTurnoDatos configuracionTurnoDatos;
        private readonly HorarioDisponibilidadMedicoDatos horarioDisponibilidadMedicoDatos;
        private readonly EspecialidadDatos especialidadDatos;
        private readonly TurnoDatos turnoDatos;

        //Constantes para operaciones de turno
        private const int DiasMaximosAnticipacion = 90;

        public TurnoCalculoService()
        {
            configuracionTurnoDatos = new ConfiguracionTurnoDatos();
            horarioDisponibilidadMedicoDatos = new HorarioDisponibilidadMedicoDatos(new AccesoDatosBase());
            especialidadDatos = new EspecialidadDatos(new AccesoDatosBase());
            turnoDatos = new TurnoDatos();
        }

        public List<TurnoDisponibleDTO> ListarTurnosDisponibles(int? idEspecialidad, int? idMedico, DateTime fechaConsulta)
        {

            if (idEspecialidad == null && idEspecialidad <= 0)
            {
                throw new ArgumentException("Debe especificar una especialidad.");
            }

            if (idMedico != null && idMedico <= 0)
            {
                throw new ArgumentException("Debe especificar un medico valido.");
            }

            if (fechaConsulta != null && fechaConsulta < DateTime.Today || fechaConsulta > DateTime.Today.AddDays(DiasMaximosAnticipacion))
            {
                throw new ArgumentException("La fecha de consulta debe estar entre hoy y " + DiasMaximosAnticipacion + " dias en el futuro.");
            }



            return new List<TurnoDisponibleDTO>();
        }

        public bool ValidarDisponibilidadTurno(int idMedico, DateTime fecha, TimeSpan HoraInicio, TimeSpan HoraFin)
        {
            return true;
        }
    }
}
