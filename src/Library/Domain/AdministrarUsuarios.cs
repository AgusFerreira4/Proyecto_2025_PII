using System;
using System.Collections.Generic;
using System.Threading;
using ClassLibrary;

namespace Library
{
    public class AdministrarUsuarios
    {
        private static AdministrarUsuarios _instancia;
        private List<Usuario> usuarios = new List<Usuario>();

        private AdministrarUsuarios()
        {
        }

        public static AdministrarUsuarios Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new AdministrarUsuarios();
                return _instancia;
            }
        }

        public void Crear(string nombre, string apellido, string email, string telefono)
        {
            Usuario nuevo = new Usuario(nombre, email, apellido, telefono);
            usuarios.Add(nuevo);
        }

        public void Add(Usuario usuario)
        {
            usuarios.Add(usuario);
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

       
        public Usuario? BuscarPorId(Guid id)
        {
            return usuarios.Find(u => u.Id == id);
        }

        public Usuario? BuscarPorEmail(string email)
        {
            return usuarios.Find(u => u.Email == email);
        }


        public List<Cliente> BuscarClientePorVentaEntre(int valor1, int valor2)
        {
                List<Cliente> clientesMonto = new List<Cliente>();
                
               
                foreach (Usuario u in usuarios)
                {
                    foreach (Venta v in  u.ObtenerVentas())
                    {
                        if ((valor1<v.Total)&&(v.Total<valor2))
                        {
                            clientesMonto.Add(v.ClienteComprador);
                        }
                    }
                }

                return clientesMonto;
            }

        public List<Cliente> BuscarClientesPorProducto(Producto producto)
        {
            List<Cliente> clientesProducto = new List<Cliente>();
            
            ArgumentNullException.ThrowIfNull(producto);
          
            foreach (Usuario u in usuarios)
            {
                foreach (Venta v in  u.ObtenerVentas())
                {
                    if (v.ProductosCantidad.ContainsKey(producto))
                    {
                        clientesProducto.Add(v.ClienteComprador);
                    }
                }
            }

            return clientesProducto;
        }
    }
}
