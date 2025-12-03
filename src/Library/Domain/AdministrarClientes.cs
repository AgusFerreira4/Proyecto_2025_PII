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

        public static AdministrarClientes Instancia => _instancia ??= new AdministrarClientes();

       
        public Cliente CrearCliente(string nombres, string apellidos, string telefonos, string emails, string generos, DateTime fechanacimiento, Usuario usuarioasignados )
        {
            try
            {
                var cliente = new Cliente(nombres, apellidos, emails, telefonos, generos, fechanacimiento, usuarioasignados);
                ListaClientes.Add(cliente);

                if (usuarioasignados.Suspendido)
                    throw new SuspendedUserException("El usuario está suspendido");

                usuarioasignados.AgregarCliente(cliente);
                return cliente;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error al crear cliente: {ex.Message}");
                throw;
            }
            
        }

       
        public Cliente? BuscarClientePorId(Guid id)
        {
            return ListaClientes.Find(c => c.Id == id);
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

      
        public void EliminarCliente(Cliente cliente)
        {
            if (!ListaClientes.Remove(cliente))
                throw new ArgumentException("No se pudo eliminar el cliente, no existe en la lista.");
        }

        public void EliminarCliente(Guid id)
        {
            Cliente? cliente = BuscarClientePorId(id);
            if (cliente == null)
                throw new ArgumentException("No existe un cliente con ese ID.");
            EliminarCliente(cliente);
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
                cliente.Genero = unGenero;
        }

        public void ModificarCliente(Guid id, string? unNombre = null, string? unApellido = null, string? unTelefono = null, string? unCorreo = null, DateTime? unaFechaNacimiento = null, string? unGenero = null)
        {
            Cliente? cliente = BuscarClientePorId(id);
            if (cliente == null)
                throw new ArgumentException("No existe un cliente con ese ID.");
            ModificarCliente(cliente, unNombre, unApellido, unTelefono, unCorreo, unaFechaNacimiento, unGenero);
        }

      
        public void AgregarEtiquetaCliente(Cliente cliente, string etiqueta)
        {
            if (cliente == null)
                throw new NullReferenceException("No se envió un cliente válido");
            cliente.Etiquetas.Add(etiqueta);
        }

        public void AgregarEtiquetaCliente(Guid id, string etiqueta)
        {
            Cliente? cliente = BuscarClientePorId(id);
            if (cliente == null)
                throw new ArgumentException("No existe un cliente con ese ID.");
            AgregarEtiquetaCliente(cliente, etiqueta);
        }

     
        public List<Cliente> VerTodos()
        {
            return ListaClientes;
        }
        
        
        
        //Defensa

        
        //recorre la los clientes totales y las ventas de cada cliente y evalua si tiene alguna venta menor al monto ingresado
        public List<Cliente> ListarClientesPorMontoMenor(int monto)
        {
            List<Cliente> clientesPorMonto = new List<Cliente>();
            foreach (Cliente cli in ListaClientes)
            {
                foreach (Venta ven in cli.ListaVentas)
                {
                    if (ven.Total < monto)
                    {
                        clientesPorMonto.Add(cli);
                        break; //Se frena asi no agrega al cliente mas veces.
                    }
                }
            }

            return clientesPorMonto;
        }
        
        //recorre la los clientes totales y las ventas de cada cliente y evalua si tiene alguna venta mayor al monto ingresado
        public List<Cliente> ListarClientesPorMontoMayor(int monto)
        {
            List<Cliente> clientesPorMonto = new List<Cliente>();
            foreach (Cliente cli in ListaClientes)
            {
                foreach (Venta ven in cli.ListaVentas)
                {
                    if (ven.Total > monto)
                    {
                        clientesPorMonto.Add(cli);
                        break;
                    }
                   
                }
            }

            return clientesPorMonto;
        }

        //recorre la los clientes totales y las ventas de cada cliente y evalua si tiene alguna venta que tsu precio este en el ragno de los montos ingresados
        public List<Cliente> ListarCLientesPorMontoRango(int monto1, int monto2)
        {
            List<Cliente> clientesPorMonto = new List<Cliente>();

            foreach (Cliente cli in ListaClientes)
            {
                foreach (Venta ven in cli.ListaVentas)
                {
                    if (ven.Total >= monto1 && ven.Total <= monto2)
                    {
                        clientesPorMonto.Add(cli);
                        break;
                    }
                    
                }
            }

            return clientesPorMonto;
        }

        //Recorre los clientes y susu ventas y dentro de cada venta evalua cada producto apra saber si alguno coincide con el ingresado, cuando sucede lo agrega a la lista y sigue reccorriendo clientes
        public List<Cliente> ListarClientesPorProducto(string nombrePrdocuto)
        {
            List<Cliente> clientesPorProducto = new List<Cliente>();
            
            foreach (Cliente cli in ListaClientes)
            {
                foreach (Venta ven in cli.ListaVentas)
                {
                    foreach (KeyValuePair<Producto, int> pair in ven.ProductosCantidad)
                    {
                        if (pair.Key.Nombre == nombrePrdocuto)
                        {
                            clientesPorProducto.Add(cli);
                            break;
                        }
                    }
                    break;
                    
                }
            }
            return clientesPorProducto;
        }
    }
}
