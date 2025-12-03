using System;
using System.Collections.Generic;
using ClassLibrary;
using Library.Excepciones;

namespace Library
{
    public class Fachada
    {
        private static Fachada _instancia;

        private Fachada() { }

        public static Fachada Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new Fachada();
                return _instancia;
            }
        }

        private Usuario user { get; set; }

        public void SetUsuario(Usuario unUsuario)
        {
            user = unUsuario;
        }

  
        public Usuario? BuscarUsuarioPorId(Guid id)
        {
            return AdministrarUsuarios.Instancia.BuscarPorId(id);
        }

        public List<Usuario> VerUsuarios()
        {
            return AdministrarUsuarios.Instancia.VerTodos();
        }

     
        private Administrador VerificarAdministrador(Usuario usuario)
        {
            if (usuario is Administrador admin)
                return admin;

            throw new PermisoDenegadoException("Acceso denegado: se requieren permisos de administrador.");
        }

        private Vendedor VerificarVendedor(Usuario usuario)
        {
            if (usuario is Vendedor vendedor)
                return vendedor;

            throw new PermisoDenegadoException("Acceso denegado: se requieren permisos de vendedor.");
        }

 
        public void CrearUsuario(string nombre, string email, string apellido, string telefono)
        {
            Administrador admin = VerificarAdministrador(user);
            admin.CrearUsuario(nombre, email, apellido, telefono);
        }

        public void EliminarUsuario(Guid id)
        {
            Administrador admin = VerificarAdministrador(user);

            Usuario? u = AdministrarUsuarios.Instancia.BuscarPorId(id);
            if (u == null)
                throw new ArgumentException("No existe un usuario con ese ID.");

            admin.EliminarUsuario(u);
        }

        public void SuspenderUsuario(Guid id)
        {
            Administrador admin = VerificarAdministrador(user);

            Usuario? u = AdministrarUsuarios.Instancia.BuscarPorId(id);
            if (u == null)
                throw new ArgumentException("No existe un usuario con ese ID.");

            admin.SuspenderUsuario(u);
        }

        public void RehabilitarUsuario(Guid id)
        {
            Administrador admin = VerificarAdministrador(user);

            Usuario? u = AdministrarUsuarios.Instancia.BuscarPorId(id);
            if (u == null)
                throw new ArgumentException("No existe un usuario con ese ID.");

            admin.ReahnilitarUsuario(u);
        }
        
        public void CrearCliente(string nombre, string apellido, string email, string telefono, string genero,
            DateTime fechaNacimiento, Usuario usuarioAsignado)
        {
            user.CrearCliente(nombre, apellido, email, telefono, genero, fechaNacimiento, usuarioAsignado);
        }

        public void EliminarCliente(Cliente cliente)
        {
            user.EliminarCliente(cliente);
        }

        public void ModificarCliente(Cliente cliente, string? nombre, string? apellido, string? telefono,
            string? correo, DateTime fechaNacimiento, string? genero)
        {
            user.ModificarCliente(cliente, nombre, apellido, telefono, correo, fechaNacimiento, genero);
        }

        public void AgregarEtiquetaACliente(Cliente cliente, string etiqueta)
        {
            user.AgregarEtiquetaACliente(cliente, etiqueta);
        }

        public void BuscarCliente(string criterio)
        {
            user.BuscarCliente(criterio);
        }

        public void AgregarInteraccion(Cliente cliente, Interaccion interaccion)
        {
            user.AgregarInteraccion(cliente, interaccion);
        }

        public void AgregarCliente(Cliente cliente)
        {
            user.AgregarCliente(cliente);
        }

        public List<Cliente> VerClientes()
        {
            return user.VerClientes();
        }

        public List<Interaccion> VerInteraccionesCliente(Cliente cliente)
        {
            return user.VerInteraccionesCliente(cliente);
        }

        public List<Interaccion> VerInteraccionesCliente(Cliente cliente, string? tipo = null, DateTime? fecha = null)
        {
            return user.VerInteraccionesCliente(cliente, tipo, fecha);
        }

        public void EliminarInteraccion(Interaccion interaccion, Cliente cliente)
        {
            user.EliminarInteraccion(interaccion, cliente);
        }

        public void AgregarNota(Interaccion interaccion, string nota)
        {
            user.AgregarNota(interaccion, nota);
        }

        public List<Cliente> VerClientesConPocaInteraccion()
        {
            return user.VerClientesConPocaInteraccion();
        }

        public List<Cliente> VerClientesEnVisto()
        {
            return user.VerClientesEnVisto();
        }

        public List<Venta> VerVentasPorPeriodo(DateTime fechaini, DateTime fechafin)
        {
            return user.VerVentasPorPeriodo(fechaini, fechafin);
        }

        public void RegistrarCotizacion(double total, DateTime fecha, DateTime fechaLimite, string descripcion)
        {
            user.RegistrarCotizacion(total, fecha, fechaLimite, descripcion);
        }

        public Venta CrearVenta(Vendedor vendedor, Cliente cliente, Dictionary<Producto, int> productosCantidad, DateTime fecha)
        {
            return user.crearVenta(vendedor, cliente, productosCantidad, fecha);
        }

        public List<Venta> ObtenerVentas()
        {
            return user.ObtenerVentas();
        }

        public void RegistrarVenta(Venta venta)
        {
            user.RegistrarVenta(venta);
        }

        public void AgregarNotaAInteraccion(Interaccion interaccion, string nota)
        {
            user.AgregarNotaAInteraccion(interaccion, nota);
        }

        public void VerPanelResumen()
        {
            user.VerPanelResumen();
        }

        public void adne(Cliente cliente, Vendedor vendedor, Vendedor vendedorNuevo)
        {
            vendedor = VerificarVendedor(user);
            vendedor.CambiarVendedorAsignado(cliente, vendedorNuevo);
        }

        public List<Cliente> BuscarClientesPorVentaEntre(int v1, int v2)
        {
            return AdministrarUsuarios.Instancia.BuscarClientePorVentaEntre(v1, v2);
        }

        public List<Cliente> BuscarClientesPorProducto(Producto producto)
        {
            return AdministrarUsuarios.Instancia.BuscarClientesPorProducto(producto);
        }
    }
}
