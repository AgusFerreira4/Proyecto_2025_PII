using System;
using System.Security.AccessControl;

namespace Library;

public class Cliente : Persona
{
/*+ Telefono : string
+ Genero : string
    + FechaDeNacimienta : string
    + Etiquetas : List<string>
    + UsuarioAsignado : Usuario
    - ListaInteracciones : GenericContainer<Interaccion>
    */
    public string Telefono { get; set; }
    public string Genero { get; set; }
    public DateTime FechaDeNacimiento { get; set; }
    public List<string> Etiquetas { get; set; }
    public Usuario UsuarioAsignado { get; set; }
    private GenericContainer<Interaccion> ListaInteraccion { get; set; }

}