namespace ClassLibrary
{
    public abstract class Persona
    {
        public string Nombre{ get; set; }
        public string Apellido{ get; set; }
        public string Email { get; set; }
        
        public int Telefono{ get; set; }

        public Persona(string nombre, string apellido, string email)
        {
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
        }
    }
}