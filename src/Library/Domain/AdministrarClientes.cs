using System;
using System.Collections.Generic;
using ClassLibrary;
using Library.Excepciones;

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
        public void CrearCliente(string nombres, string apellidos, string telefonos, string emails, string generos, DateTime fechanacimiento, Usuario usuarioasignados)
        {
            try
            {
                var cliente = new Cliente(nombres, apellidos, emails, telefonos, generos, fechanacimiento,
                    usuarioasignados);
                ListaClientes.Add(cliente);
                if (usuarioasignados.Suspendido)
                {
                    throw new SuspendedUserException("El usuario esta suspendido");
                }

                usuarioasignados.AgregarCliente(cliente);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error al crear usuario: {ex.Message}");
                throw;
            }
        }
        public void EliminarCliente(Cliente cliente)
        {
            try
            {
                ListaClientes.Remove(cliente);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void ModificarCliente(Cliente cliente, string? unNombre, string? unApellido, string? unTelefono, string? unCorreo, DateTime? unaFechaNacimiento, string? unGenero )
        {
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
            if (cliente.Equals(null))
            {
                throw new NullReferenceException("No mando un cliente");
            }
            cliente.Etiquetas.Add(etiqueta);
                
        }
        public Cliente BuscarCliente(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                throw new ArgumentException("El criterio de búsqueda no puede ser nulo ni vacío.");

            if (ListaClientes == null || ListaClientes.Count == 0)
                throw new InvalidOperationException("No hay clientes cargados en el sistema.");

            foreach (Cliente cliente in ListaClientes)
            {
                if (cliente.Nombre.Contains(criterio, StringComparison.OrdinalIgnoreCase) ||
                    cliente.Apellido.Contains(criterio, StringComparison.OrdinalIgnoreCase) ||
                    cliente.Telefono.Contains(criterio, StringComparison.OrdinalIgnoreCase) ||
                    cliente.Email.Contains(criterio, StringComparison.OrdinalIgnoreCase))
                {
                    return cliente;
                }
            }

            throw new KeyNotFoundException("No se encontró ningún cliente con ese criterio.");
        }
        }
 
}
    
