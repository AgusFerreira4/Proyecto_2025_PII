using System;
using Library;

namespace ClassLibrary
{

    public class Llamadas : Interaccion
    {
        //En esta versión aún no está considera la llamada como un IRespondible
        //debido a la implementación de la lógica en poco tiempo
        //Funcionalidad pendiente para la próxima entrega
        public string NumeroEmisor {get; set; }
        public string NumeroReceptor {get; set; }
        public bool Respondido { get; private set; } = true;


        public Llamadas(Persona emisor, Persona receptor, DateTime fecha, string tema) : base(emisor,
            receptor, fecha, tema)
        {
            this.NumeroEmisor = emisor.Telefono;
            this.NumeroReceptor = receptor.Telefono;

        }
        
    }
}