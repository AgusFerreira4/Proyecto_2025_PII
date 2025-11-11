using System;
using System.Collections.Generic;
using System.Linq;
using Library;

namespace ClassLibrary
{
    public class Usuario : Persona
    {
        public bool Suspendido { get; set; }
        public List<Cliente> ListaClientesDeUsuario { get; set; }
        public List<Venta> ListaVentas { get; set; }
        public List<Cotizacion> ListaCotizaciones { get; set; }
        

        public Usuario(
            string nombre,
            string email,
            string apellido,
            string telefono
        )
            : base(nombre, email, apellido, telefono)
        {
            Suspendido = false;
            ListaClientesDeUsuario = new List<Cliente>();
            ListaVentas = new List<Venta>();
            ListaCotizaciones = new List<Cotizacion>();

        }


        public void CrearCliente(string nombre, string apellido, string email, string telefono, string genero,
            DateTime fechaDeNacimiento, Usuario usuarioAsignado)
        {
            AdministrarClientes.Instancia.CrearCliente(nombre, apellido, telefono, email, genero, fechaDeNacimiento, usuarioAsignado);
        }

        public void EliminarCliente(Cliente cliente)
        {
            AdministrarClientes.Instancia.EliminarCliente(cliente);
        }

        public void ModificarCliente(Cliente cliente, string? unNombre, string? unApellido, string? unTelefono, string? unCorreo, DateTime unaFechaNacimiento, string? unGenero )
        {
            AdministrarClientes.Instancia.ModificarCliente(cliente, unNombre, unApellido, unTelefono, unCorreo, unaFechaNacimiento, unGenero);
        }
        
        public void ModificarCliente(Cliente cliente, string? unNombre, string? unApellido, string? unTelefono, string? unCorreo, string? unGenero )
        {
            AdministrarClientes.Instancia.ModificarCliente(cliente, unNombre, unApellido, unTelefono, unCorreo, unGenero);
        }

        public void AgregarEtiquetaACliente(Cliente cliente, string etiqueta)
        {
            AdministrarClientes.Instancia.AgregarEtiquetaCliente(cliente, etiqueta);
        }

        public void BuscarCliente(string criterio)
        {
            AdministrarClientes.Instancia.BuscarCliente(criterio);
        }

        public void AgregarInteraccion(Cliente cliente, Interaccion interaccion)
        {
            //cambiando singleto de administrarinteracciones para poder hacerlo
        }

        public void AgregarCliente(Cliente cliente)
        {
            this.ListaClientesDeUsuario.Add(cliente);
        }

        public List<Cliente> VerClientes()
        {
            return ListaClientesDeUsuario;
        }
        

        public List<Interaccion> VerInteraccionesCliente(Cliente cliente)
        {
            List<Interaccion> interacciones = new List<Interaccion>();
            foreach (Interaccion i in cliente.ListaInteracciones)
            {
                interacciones.Add(i);
            }

            return interacciones;
        }

        public List<Interaccion> VerInteraccionesCliente(Cliente cliente, string tipo)
        {
            List<Interaccion> interacciones = new List<Interaccion>();
            foreach (Interaccion i in cliente.ListaInteracciones)
            {
                if (i.GetType().Name == tipo)
                {
                    interacciones.Add(i);
                }
            }

            return interacciones;
        }
        
        public List<Interaccion> VerInteraccionesCliente(Cliente cliente, DateTime fecha)
        {
            List<Interaccion> interacciones = new List<Interaccion>();
            foreach (Interaccion i in cliente.ListaInteracciones)
            {
                if (i.Fecha == fecha)
                {
                    interacciones.Add(i);
                }
            }

            return interacciones;
        }

        public List<Interaccion> VerInteraccionesCliente(Cliente cliente, DateTime fecha, string tipo)
        {
            List<Interaccion> interacciones = new List<Interaccion>();
            foreach (Interaccion i in cliente.ListaInteracciones)
            {
                if (i.GetType().Name == tipo && i.Fecha == fecha)
                {
                    interacciones.Add(i);
                }
            }

            return interacciones;
        }

        public List<Cliente> VerClientesConPocaInteraccion()
        {
            List<Cliente> clientesPocaInteraccion = new List<Cliente>();
            foreach (Cliente cliente in ListaClientesDeUsuario)
            {
                if (cliente.ListaInteracciones.Count <= 5)
                {
                    clientesPocaInteraccion.Add(cliente);
                }
            }

            return clientesPocaInteraccion;
        }


        public List<Cliente> VerClientesEnVisto()
        {
            List<Cliente> clientesVistos = new List<Cliente>();
            foreach (Cliente cl in ListaClientesDeUsuario)
            {
                if (cl.UsuarioAsignado != null) // etiqueta visto
                {
                    clientesVistos.Add(cl);
                }
            }

            return clientesVistos;
        }

        public List<Venta> VerVentasPorPeriodo(DateTime fechaini, DateTime fechafin)
        {
            List<Venta> ventasPorFecha = new List<Venta>();
            foreach (Venta v in ListaVentas)
            {
                if (v.Fecha >= fechaini && v.Fecha <= fechafin)
                {
                    ventasPorFecha.Add(v);
                }
            }

            return ventasPorFecha;
        }

        public void RegistrarCotizacion(double total, DateTime fecha,
            DateTime fechaLimite, string descripcion)
        {
            Cotizacion cotizacion = new Cotizacion(total, fecha, fechaLimite, descripcion);
            this.ListaCotizaciones.Add(cotizacion);
        }

        public Venta crearVenta(Vendedor vendedor, Cliente cliente, Dictionary<Producto, int> productosCantidad, DateTime fecha)
        {
            Venta venta = new Venta(productosCantidad, 0, fecha, cliente, vendedor);
            RegistrarVenta(venta);
            return venta;
        }

        public List<Venta> ObtenerVentas()
        {
            return ListaVentas;
        }
        public void RegistrarVenta(Venta venta)
        {
            ListaVentas.Add(venta);
        }

        public void AgregarNotaAInteraccion(Interaccion interaccion, string nota)
        {
            foreach (Interaccion i in ListaInteracciones)
            {
                if (i.Equals(interaccion))
                {
                    i.Nota = nota;
                    break;
                }
            }
        }

        public void VerPanelResumen()
        {
            DateTime dosSemanasAdelante = DateTime.Now.AddDays(14); // Para ver las proximas reuniones de aca a 2 semanas
            DateTime ahora = DateTime.Now;
            List<Reuniones> proximasReuniones = new List<Reuniones>();
            
            Console.WriteLine($"--------------Resumen de {this.Nombre}--------------");
            Console.WriteLine("Lista de clientes");
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------------------------");
            foreach (Cliente cliente in ListaClientesDeUsuario)
            {
                Console.WriteLine($"Cliente: {cliente.Nombre}");
                
            }
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Proximas Reuniones");
            foreach (Reuniones reunion in this.ListaInteracciones)
            {
                if (reunion.Fecha >= ahora && reunion.Fecha <= dosSemanasAdelante)
                {
                    proximasReuniones.Add(reunion);
                }
            }

            proximasReuniones = proximasReuniones.OrderByDescending(i => i.Fecha).ToList();
            proximasReuniones.ForEach(i => Console.WriteLine($"Programada con : {i.GetReceptor()}" +
                                                                    $"Lugar: {i.Lugar}" +
                                                                    $"Fecha: {i.Fecha} "));
        }
    }
    
}