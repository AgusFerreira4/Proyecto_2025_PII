using System;

namespace Library;

public class Llamadas : Interaccion
{
    public Llamadas(Persona remitente, Persona destinatario, DateTime fecha, string tema) : base(remitente, destinatario,
        fecha, tema)
    {
        
    }
}