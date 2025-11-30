using System;
using System.Collections.Generic;
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
                    total += par.Key.Precio + par.Value;
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
    }
}