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
            //Añadir excepción en caso de no mandar un cliente o el usuario esté suspendido
            cliente.Etiquetas.Add(etiqueta);
                
        }
            
        public Cliente BuscarCliente(string criterio)
        {
            if (string.IsNullOrEmpty(criterio)) return null;
            //Esto de arriba debería de ser una excepción en caso de no dar un criterio
            //También debe de ser una excepción si no se encuentra al cliente o si la lista de clientes está vacía
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
            return null;
            }
            
        }
}
    
