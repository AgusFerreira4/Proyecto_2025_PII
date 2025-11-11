using System;
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

        public void Crear(string nombre, string apellido, string email, string telefono)
        {
            try
            {
            Usuario nuevo = new Usuario(nombre, email, apellido, telefono);
            usuarios.Add(nuevo);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error al crear usuario: {ex.Message}");
                throw;
            }
        }

        public void EliminarUsuario(Usuario usuario)
        {
            try
            {
                usuarios.Remove(usuario);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void SuspenderUsuario(Usuario usuario)
        {
            try
            {
                usuario.Suspendido = true;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void RehabilitarUsuario(Usuario usuario)
        {
            try
            {
                usuario.Suspendido = false;

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e);
                throw;
            }
        } 
        public List<Usuario> VerTodos()
        {
            return usuarios;
        }
    } 
}
        