using System.Collections.Generic;
using ClassLibrary;

namespace Library
{
    public class AdministrarUsuarios
    {
        private static List<Usuario> usuarios = new List<Usuario>();

        public void Crear(string nombre, string apellido, string email)
        {
            Usuario nuevo = new Usuario(nombre, email, apellido, false);
            usuarios.Add(nuevo);
        }

        public void EliminarUsuario(Usuario usuario)
        {
            usuarios.Remove(usuario);
        }

        public void SuspenderUsuario(Usuario usuario)
        {
            usuario.Suspendido = true;
        }

        public void RehabilitarUsuario(Usuario usuario)
        {
            usuario.Suspendido = false;
        } 
    } 
}
        