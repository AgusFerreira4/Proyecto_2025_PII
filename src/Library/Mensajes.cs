using System;
using Library;

namespace ClassLibrary
{
    public class Mensajes : Interaccion, IRespondible
    
    {
        public string NumeroEmisor { get; set; }
        public string NumeroReceptor { get; set; }
        public string Contenido { get; set; }
        public bool Respondido { get; private set; }

        public Mensajes(Persona emisor, Persona receptor, DateTime fecha, string tema, string contenido) : base(emisor, receptor,
            fecha, tema)
        {
            this.NumeroEmisor = emisor.Telefono;
            this.NumeroReceptor = receptor.Telefono;
            this.Contenido = contenido;
        }
        public void MarcarComoRespondido()
        {
            Respondido = true;
        }

        public bool EsRespuestaDe(IRespondible otra)
        {
            if (otra is Mensajes otroMensaje)
            {
                return otroMensaje.Emisor == this.Receptor &&
                       otroMensaje.Receptor == this.Emisor &&
                       otroMensaje.Fecha > this.Fecha;
            }

            return false;
        }
    }
}