using System.Collections.Generic;

namespace ClassLibrary
{
    public abstract class Persona
    {
        public string Nombre{ get; set; }
        public string Apellido{ get; set; }
        public string Email { get; set; }
        
        public string Telefono{ get; set; }
        public List<Interaccion> ListaInteracciones { get; set; }

        public Persona(string nombre, string apellido, string email, string telefono)
        {
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Telefono = telefono;
            ListaInteracciones = new List<Interaccion>();
        }
    }
}