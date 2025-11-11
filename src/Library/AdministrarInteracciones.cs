using System.Collections.Generic;
using ClassLibrary;

namespace Library

{
    public class AdministrarInteracciones
    {
        private List<Interaccion> ListaInteracciones = new List<Interaccion>();
        private static AdministrarInteracciones _instancia;
        
        private AdministrarInteracciones() {}

        public static AdministrarInteracciones Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new AdministrarInteracciones();
                }
                return _instancia;
            }
        }
        public void AgregarInteraccion(Cliente unCliente, Interaccion unaInteraccion)
        {
            ListaInteracciones.Add(unaInteraccion);
        }
        public List<Interaccion> VerInteraccionesCliente()
        {
            return ListaInteracciones;
        }

        public void EliminarInteraccion(Interaccion interaccion)
        {
            ListaInteracciones.Remove(interaccion);
        }
        
        public void AgregarNota(Interaccion unaInteraccion, string nota)
        {
            unaInteraccion.AddNota(nota);
        }

        public void AgregarLlamada() { }
        public void AgregarReunion() { }
        public void AgregarEmails() { }
        public void AgregarMensajes() { }
    }
}