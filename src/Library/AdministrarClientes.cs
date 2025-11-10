using System;
using System.Collections.Generic;
using ClassLibrary;

namespace Library
{
    public class AdministrarClientes
    {
        private List<Cliente> ListaClientes = new List<Cliente>();
        private static AdministrarClientes _instancia;
        private AdministrarClientes() { }
        public static AdministrarClientes Instancia
        {
            get { return _instancia ??= new AdministrarClientes(); }
        }
        public void CrearCliente(string nombres, string apellidos, string telefonos, string emails, string generos, DateTime fechanacimiento, Usuario usuarioasignados )
        {
            //Añadir excepción cuando el usuario está suspendido    
            var cliente = new Cliente(nombres, apellidos, emails, telefonos, generos, fechanacimiento, usuarioasignados );
            ListaClientes.Add(cliente);
            usuarioasignados.AgregarCliente(cliente);
                
        }
        public void EliminarCliente(Cliente cliente)
        {
            ListaClientes.Remove(cliente);
        }

        public void ModificarCliente(Cliente cliente, string? unNombre, string? unApellido, string? unTelefono, string? unCorreo, DateTime? unaFechaNacimiento, string? unGenero )
        {
            //Añadir excepciones cuando el cliente es nulo o el usuario está suspendido
            if (unNombre != null)
                cliente.Nombre = unNombre;

            if (unApellido != null)
                cliente.Apellido = unApellido;

            if (unTelefono != null)
                cliente.Telefono = unTelefono;

            if (unCorreo != null)
                cliente.Email = unCorreo;

            if (unaFechaNacimiento.HasValue)
                cliente.FechaDeNacimiento = unaFechaNacimiento.Value;
            if (unGenero != null)
            {
                cliente.Genero = unGenero;
            }
        }
        
        
            public void AgregarEtiquetaCliente(Cliente cliente, string etiqueta)
            {
                if (cliente?.Etiquetas == null)
                    cliente.Etiquetas = new List<string>();
                cliente.Etiquetas.Add(etiqueta);
                
            }
            
            public Cliente BuscarCliente(string criterio)
            {
                if (string.IsNullOrEmpty(criterio)) return null;

                foreach (var cliente in ListaClientes)
                {
                    string nombre = cliente.Nombre ?? "";
                    string apellido = cliente.Apellido ?? "";
                    string email = cliente.Email ?? "";
                    string telefono = cliente.Telefono ?? "";

                    if (nombre.ToLower().Contains(criterio.ToLower()) ||
                        apellido.ToLower().Contains(criterio.ToLower()) ||
                        email.ToLower().Contains(criterio.ToLower()) ||
                        telefono.Contains(criterio))
                    {
                        return cliente;
                    }
                }
                return null;
            }
            
        }
}
    
