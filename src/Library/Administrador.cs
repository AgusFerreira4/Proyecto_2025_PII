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
            bool suspendido
        ) : base(nombre, email, apellido, suspendido)
        {
        
        }


        public void CrearUsuario
        (
            string nombreUsuario,
            string emailUsuario,
            string apellidoUsuario
                )
        {
            AdministrarUsuarios.Instancia.Crear(nombreUsuario, emailUsuario, apellidoUsuario);
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