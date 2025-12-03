using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using ClassLibrary;
using Library;
using Library.Excepciones;



namespace Tests1
{
    //Test para la defensa del proyecto, se testean las historias de usuario para cliente
    [TestFixture]
    public class PruebasDefensa
    {
        private Fachada fachada;
        
        [SetUp]
        public void Setup()
        {
            ResetSingleton(typeof(Fachada), "_instancia");
            ResetSingleton(typeof(AdministrarClientes), "_instancia");
            ResetSingleton(typeof(AdministrarUsuarios), "_instancia");
            ResetSingleton(typeof(AdministrarInteracciones), "_instancia");

            fachada = Fachada.Instancia;
        }
        
        private void ResetSingleton(Type t, string fieldName)
        {
            // Busca campo private static _instancia y lo deja en null
            var f = t.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            if (f != null)
                f.SetValue(null, null);
        }
        
        // ---------- Setup De Entidades ----------
        private Vendedor CrearVendedor(string nombre = "Vendedor") =>
            new Vendedor(nombre, $"{nombre.ToLower()}@mail", "Apellido", "099");
        
        private Producto CrearProducto(string nombre = "Prod", double precio = 100) =>
            new Producto(nombre, precio);

        [Test]
        public void ListarClientesPorMontoMenor_RetornaListaDeCLientes()
        {
            var vendedor = CrearVendedor("pepe");
            fachada.SetUsuario(vendedor);
            var cliente1 = fachada.CrearCliente("Ana", "Gomez", "a@g.com", "123", "F", DateTime.Now, vendedor);
            var cliente2 = fachada.CrearCliente("Pedro", "Gomez", "a@g.com", "234", "M", DateTime.Now, vendedor);
            var cliente3 = fachada.CrearCliente("Juana", "Gomez", "a@g.com", "456", "F", DateTime.Now, vendedor);
            
            var producto1 =  CrearProducto("Chevrolet", 400);
            var producto2 =  CrearProducto("Fiat", 200);
            var producto3 =  CrearProducto("Fusca", 300);
            var producto4 =  CrearProducto("Papas", 20);
            var producto5 =  CrearProducto("Manzanas", 15);

            Dictionary<Producto, int> listaAutos = new Dictionary<Producto, int>();
            listaAutos.Add(producto1, 1);
            listaAutos.Add(producto2, 2);
            listaAutos.Add(producto3, 4);
            
            Dictionary<Producto, int> listaProductos = new Dictionary<Producto, int>();
            listaProductos.Add(producto4, 3);
            listaProductos.Add(producto5, 2);
            

            var venta1 = fachada.CrearVenta(vendedor, cliente1.Id, listaAutos, DateTime.Now);
            var venta2 = fachada.CrearVenta(vendedor, cliente2.Id, listaProductos, DateTime.Now);
            var venta3  =fachada.CrearVenta(vendedor, cliente3.Id, listaProductos, DateTime.Now);
            
            
            Assert.That(fachada.ListarClientesPorMontoMenor(100).Count, Is.LessThan(3)); // solo hay 2 clientes que cumplen ese requisito
        }
        
        [Test]
        public void ListarClientesPorMontoMayor_RetornaListaDeCLientes()
        {
            var vendedor = CrearVendedor("pepe");
            fachada.SetUsuario(vendedor);
            var cliente1 = fachada.CrearCliente("Ana", "Gomez", "a@g.com", "123", "F", DateTime.Now, vendedor);
            var cliente2 = fachada.CrearCliente("Pedro", "Gomez", "a@g.com", "234", "M", DateTime.Now, vendedor);
            var cliente3 = fachada.CrearCliente("Juana", "Gomez", "a@g.com", "456", "F", DateTime.Now, vendedor);
            
            var producto1 =  CrearProducto("Chevrolet", 400);
            var producto2 =  CrearProducto("Fiat", 200);
            var producto3 =  CrearProducto("Fusca", 300);
            var producto4 =  CrearProducto("Papas", 20);
            var producto5 =  CrearProducto("Manzanas", 15);

            Dictionary<Producto, int> listaAutos = new Dictionary<Producto, int>();
            listaAutos.Add(producto1, 50);
            listaAutos.Add(producto2, 50);
            listaAutos.Add(producto3, 50);
            
            Dictionary<Producto, int> listaProductos = new Dictionary<Producto, int>();
            listaProductos.Add(producto4, 50);
            listaProductos.Add(producto5, 50);
            

            var venta1 = fachada.CrearVenta(vendedor, cliente1.Id, listaAutos, DateTime.Now);
            var venta2 = fachada.CrearVenta(vendedor, cliente2.Id, listaProductos, DateTime.Now);
            var venta3  =fachada.CrearVenta(vendedor, cliente3.Id, listaProductos, DateTime.Now);
            
            
            Assert.That(fachada.ListarClientesPorMontoMayor(100).Count, Is.GreaterThanOrEqualTo(3)); // en estos casos los 3 clientes cumplen ese requisito
        }
        
        [Test]
        public void ListarClientesPorMontoRango_RetornaListaDeCLientes()
        {
            var vendedor = CrearVendedor("pepe");
            fachada.SetUsuario(vendedor);
            var cliente1 = fachada.CrearCliente("Ana", "Gomez", "a@g.com", "123", "F", DateTime.Now, vendedor);
            var cliente2 = fachada.CrearCliente("Pedro", "Gomez", "a@g.com", "234", "M", DateTime.Now, vendedor);
            var cliente3 = fachada.CrearCliente("Juana", "Gomez", "a@g.com", "456", "F", DateTime.Now, vendedor);
            
            var producto1 =  CrearProducto("Chevrolet", 10);
            var producto2 =  CrearProducto("Fiat", 10);
            var producto3 =  CrearProducto("Fusca", 30);
            var producto4 =  CrearProducto("Papas", 20);
            var producto5 =  CrearProducto("Manzanas", 15);

            Dictionary<Producto, int> listaAutos = new Dictionary<Producto, int>();
            listaAutos.Add(producto1, 10);
            listaAutos.Add(producto2, 2);
            listaAutos.Add(producto3, 1);
            
            Dictionary<Producto, int> listaProductos = new Dictionary<Producto, int>();
            listaProductos.Add(producto4, 2);
            listaProductos.Add(producto5, 2);
            

            var venta1 = fachada.CrearVenta(vendedor, cliente1.Id, listaAutos, DateTime.Now);
            var venta2 = fachada.CrearVenta(vendedor, cliente2.Id, listaProductos, DateTime.Now);
            var venta3  =fachada.CrearVenta(vendedor, cliente3.Id, listaProductos, DateTime.Now);
            
            
            Assert.That(fachada.ListarClientesPorRangoDeMontos(100, 500).Count, Is.EqualTo(1)); // solo hay 1 cliente que cumplen ese requisito
        }
        
        [Test]
        public void ListarClientesPorProducto_RetornaListaDeCLientes()
        {
            var vendedor = CrearVendedor("pepe");
            fachada.SetUsuario(vendedor);
            var cliente1 = fachada.CrearCliente("Ana", "Gomez", "a@g.com", "123", "F", DateTime.Now, vendedor);
            var cliente2 = fachada.CrearCliente("Pedro", "Gomez", "a@g.com", "234", "M", DateTime.Now, vendedor);
            var cliente3 = fachada.CrearCliente("Juana", "Gomez", "a@g.com", "456", "F", DateTime.Now, vendedor);
            
            var producto1 =  CrearProducto("Chevrolet", 400);
            var producto2 =  CrearProducto("Fiat", 200);
            var producto3 =  CrearProducto("Fusca", 300);
            var producto4 =  CrearProducto("Papas", 20);
            var producto5 =  CrearProducto("Manzanas", 15);

            Dictionary<Producto, int> listaAutos = new Dictionary<Producto, int>();
            listaAutos.Add(producto1, 1);
            listaAutos.Add(producto2, 2);
            listaAutos.Add(producto3, 4);
            
            Dictionary<Producto, int> listaProductos = new Dictionary<Producto, int>();
            listaProductos.Add(producto4, 3);
            listaProductos.Add(producto5, 2);
            

            var venta1 = fachada.CrearVenta(vendedor, cliente1.Id, listaAutos, DateTime.Now);
            var venta2 = fachada.CrearVenta(vendedor, cliente2.Id, listaProductos, DateTime.Now);
            var venta3  =fachada.CrearVenta(vendedor, cliente3.Id, listaProductos, DateTime.Now);
            
            
            Assert.That(fachada.ListarClientesPorProducto("Papas").Count, Is.EqualTo(2)); // solo hay 2 clientes que cumplen ese requisito
        }

    }
}