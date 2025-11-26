using System;
using Library;

namespace ClassLibrary
{
    public class Emails : Interaccion, IRespondible

    {
    public string DireccionEmisor { get; set; }
    public string DireccionReceptor { get; set; }
    public string Contenido { get; set; }
    public bool Respondido { get; set; } = false;


    public Emails(Persona emisor, Persona receptor, DateTime fecha, string tema, string contenido) : base(
        emisor, receptor, fecha, tema)
    {
        this.DireccionEmisor = emisor.Email;
        this.DireccionReceptor = receptor.Email;
        this.Contenido = contenido;
    }

    public void MarcarComoRespondido()
    {
        Respondido = true;
    }

    public bool EsRespuestaDe(IRespondible otra)
    {
        if (otra is Emails otroEmail)
        {
            return otroEmail.Emisor == this.Receptor &&
                   otroEmail.Receptor == this.Emisor &&
                   otroEmail.Fecha > this.Fecha;
        }

        return false;
    }
    }
}

