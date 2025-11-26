using System.Reflection;
using ClassLibrary;

namespace Library
{
    public class Administrador : Usuario
    {

        public Administrador(
            string nombre,
            string email,
            string apellido,
            string telefono
        ) : base(nombre, email, apellido, telefono)
        {
        
        }


        public void CrearUsuario
        (
            string nombreUsuario,
            string emailUsuario,
            string apellidoUsuario,
            string unTelefono
                )
        {
            AdministrarUsuarios.Instancia.Crear(nombreUsuario, emailUsuario, apellidoUsuario, unTelefono);
        }

        public void EliminarUsuario(Usuario unUsuario)
        {
            AdministrarUsuarios.Instancia.EliminarUsuario(unUsuario);
        }

        public void SuspenderUsuario(Usuario unUsuario)
        {
            AdministrarUsuarios.Instancia.SuspenderUsuario(unUsuario);
        }

        public void ReahnilitarUsuario(Usuario unUsuario)
        {
            AdministrarUsuarios.Instancia.RehabilitarUsuario(unUsuario);
        }
        
    }
}