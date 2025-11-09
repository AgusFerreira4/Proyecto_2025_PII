using System;

namespace ClassLibrary
{
    public class Mensajes : Interaccion
    
    {
        public string contenido { get; set; }
        public Mensajes(Persona remitente, Persona destinatario, DateTime fecha, string tema) : base(remitente, destinatario,
            fecha, tema)
        {
        
        }

        public void Enviar()
        {
        
        }
    }
}