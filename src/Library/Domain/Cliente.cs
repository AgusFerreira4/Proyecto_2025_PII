using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace ClassLibrary
{
    public class Cliente : Persona
    {
    /*+ Telefono : string
    + Genero : string
    + FechaDeNacimienta : string
    + Etiquetas : List<string>
    + UsuarioAsignado : Usuario
    - ListaInteracciones : GenericContainer<Interaccion>
    */
        public Guid Id { get; } = Guid.NewGuid();
        public string Telefono { get; set; }
        public string Genero { get; set; }
        public DateTime FechaDeNacimiento { get; set; }
        public List<string> Etiquetas { get; set; }
        public Usuario UsuarioAsignado { get; set; }
        public List<Venta> ListaVentas { get; set; }
        

        public Cliente(string nombre, string apellido, string email, string telefono, string genero,
            DateTime fechaDeNacimiento, Usuario usuarioAsignado)
            : base(nombre, apellido, email, telefono)
        {
            this.Telefono = telefono;
            this.Genero = genero;
            this.FechaDeNacimiento = fechaDeNacimiento;
            this.UsuarioAsignado = usuarioAsignado;
            this.Etiquetas = new List<string>();
            this.ListaVentas = new List<Venta>();

        }

    }
}

   
