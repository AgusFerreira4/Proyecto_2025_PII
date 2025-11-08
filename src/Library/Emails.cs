using System;

namespace Library;

public class Emails : Interaccion
{
    public Emails(Persona remitente, Persona destinatario, DateTime fecha, string tema) : base(remitente, destinatario,
        fecha, tema)
    {
        
    }

    public void Enviar()
    {
        
    }
}