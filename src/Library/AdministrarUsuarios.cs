using System.Collections.Generic;
using ClassLibrary;

namespace Library
{
    public class AdministrarUsuarios
    {
        private static AdministrarUsuarios _instancia;
        private  List<Usuario> usuarios = new List<Usuario>();
        
        private AdministrarUsuarios() {}


        public static AdministrarUsuarios Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new AdministrarUsuarios();
                }
                return _instancia;
            }
        }

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
        public List<Usuario> VerTodos()
        {
            return usuarios;
        }
    } 
}
        