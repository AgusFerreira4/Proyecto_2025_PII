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
                var cliente = new Cliente(nombres, apellidos, emails, telefonos, generos, fechanacimiento, usuarioasignados );
                ListaClientes.Add(cliente);
                usuarioasignados.AgregarCliente(cliente);
                
        }
        public void EliminarCliente(Cliente cliente)
        {
            ListaClientes.Remove(cliente);
        }

        public void ModificarCliente(Cliente cliente, string? unNombre, string? unApellido, string? unTelefono, string? unCorreo, DateTime unaFechaNacimiento, string? unGenero )
        {
            if (cliente == null) return;
            cliente.Nombre = unNombre;
            cliente.Apellido = unApellido;
            cliente.Telefono = unTelefono;
            cliente.Genero = unGenero;
            cliente.Email = unCorreo;
            cliente.FechaDeNacimiento = unaFechaNacimiento;
        }
        
        public void ModificarCliente(Cliente cliente, string? unNombre, string? unApellido, string? unTelefono, string? unCorreo, string? unGenero )
        {
            if (cliente == null) return;
            cliente.Nombre = unNombre;
            cliente.Apellido = unApellido;
            cliente.Telefono = unTelefono;
            cliente.Genero = unGenero;
            cliente.Email = unCorreo;
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
    
