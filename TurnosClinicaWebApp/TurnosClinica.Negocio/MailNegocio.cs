using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using TurnosClinica.Dominio.Entidades;

namespace TurnosClinica.Negocio
{
    public class MailNegocio
    {
        private MailMessage email;
        private SmtpClient server;
        
        public MailNegocio()
        {
            server = new SmtpClient();
            server.Credentials = new NetworkCredential("programationiii@gmail.com", "programacion3");
            server.EnableSsl = true;
            server.Port = 587;
            server.Host = "smtp.gmail.com";
        }
        // est es el metodo el cual usaremos
        public void EnviarConfirmacionTurnoNuevo(Turno turno)
        {
            // verifico si el paciente exist y el mail
            if (turno.Paciente == null || turno.Paciente.Persona == null || string.IsNullOrEmpty(turno.Paciente.Persona.Email))
            {
                return; 
            }
            // esto seria el asunto
            string asunto = $"Confirmación de Turno Médico - Clínica";

            // cuerpo del correo
            string cuerpoHtml = $"<h2>¡Hola {turno.Paciente.Persona.Nombre} {turno.Paciente.Persona.Apellido}!</h2>" +
                                $"<p>Tu turno en la clínica ha sido confirmado con éxito.</p>" +
                                $"<ul>" +
                                $"<li><strong>Nro de Turno:</strong> {turno.NumeroTurno}</li>" +
                                $"<li><strong>Especialidad:</strong> {turno.Especialidad.Nombre}</li>" +
                                $"<li><strong>Médico:</strong> {turno.Medico.Persona.Nombre} {turno.Medico.Persona.Apellido}</li>" +
                                $"<li><strong>Fecha:</strong> {turno.FechaTurno.ToString("dd/MM/yyyy")}</li>" +
                                $"<li><strong>Hora:</strong> {turno.HoraInicio.ToString(@"hh\:mm")} hs</li>" +
                                $"</ul>" +
                                $"<p>Por favor, recuerda llegar con 10 minutos de anticipación.</p>";

            // el strong lo estoy usando para poner el texto en negritA
            
            email = new MailMessage();
            email.From = new MailAddress("noresponder@clinicamedica.com", "Turnos Clínica");
            email.To.Add(turno.Paciente.Persona.Email);
            email.Subject = asunto;
            email.IsBodyHtml = true;
            email.Body = cuerpoHtml;
            
            try
            {
                server.Send(email);
            }
            catch (Exception ex)
            {
                // por si hay algun error
                throw new Exception("Hubo un error al enviar el mail: " + ex.Message);
            }
        }

    }
}
