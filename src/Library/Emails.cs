using System;

namespace ClassLibrary
{
    public class Emails : Interaccion
    {
        public string contenido { get; set; }
        public Emails(Persona remitente, Persona destinatario, DateTime fecha, string tema) : base(remitente, destinatario,
            fecha, tema)
        {
        
        }

        public void Enviar()
        {
        
        }
    }
}

