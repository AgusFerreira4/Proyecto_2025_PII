using System;
using System.Collections.Generic;
using Library;

namespace ClassLibrary
{
    public class Venta
    {
        public Dictionary<Producto, int> ProductosCantidad { get; set; }

        public double Total
        {
            get
            {
                double total = 0;
                foreach (KeyValuePair<Producto, int> par in ProductosCantidad)
                {
                    total += par.Key.Precio * par.Value;
                }

                return total;
            }
            set{}
        }

        public DateTime Fecha { get; set; }
        public Cliente ClienteComprador { get; set; }
        public Usuario UsuarioVendedor { get; set; }

        public Venta(Dictionary<Producto, int> productosCantidad, DateTime fecha, Cliente clienteComprador,
            Usuario usuarioVendedor)
        {
            ProductosCantidad = productosCantidad;
            Fecha = fecha;
            ClienteComprador = clienteComprador;
            UsuarioVendedor = usuarioVendedor;
        }

        public void AgregarProducto(Producto producto, int cantidad)
        {
            ProductosCantidad.Add(producto, cantidad);
        }
        
       public Cliente Ventas_mayores_a(Cliente unCliente)
       {
           foreach (Cliente cliente in AdministrarClientes.ListaClientes)
           {
               if(Total  > 2000)
               {
                   return cliente;
               }
           }
           return null;
        }
       
       public string  ventas_Producto(string Criterio, Producto producto)
       {
           var ventas = Usuario.ListaVentas;
           var clientesConProducto = new HashSet<Cliente>();
           string respuesta = $"**Clientes que compraron {Criterio}:**";

           foreach (var venta in ventas)
           {
               foreach (var item in venta.ProductosCantidad)
               {
                   if (item.Key.Nombre.Equals(Criterio, StringComparison.OrdinalIgnoreCase))
                   {
                       clientesConProducto.Add(venta.ClienteComprador);
                       
                   }

                   foreach (var cliente in clientesConProducto)
                   {
                       return respuesta += $" **{cliente.Nombre} {cliente.Apellido}**";
                   }
               }
           }

           return null;
       }
    }
}