using System;

namespace Library;

public abstract class Interaccion
{
    public Persona Remitente { get; set; }
    public Persona Destinatario { get; set; }
    public DateTime Fecha { get; set; }
    public string Tema { get; set; }
    public string Nota { get; set; }

    public Interaccion(Persona remitente, Persona destinatario, DateTime fecha, string tema)
    {
        Remitente = remitente;
        Destinatario = destinatario;
        Fecha = fecha;
        Tema = tema;
    }

    public void AddNota(string nota)
    {
        Nota = nota;
    }
}