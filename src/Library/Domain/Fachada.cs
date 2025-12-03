using System;
using System.Collections.Generic;
using ClassLibrary;
using Discord.Commands;
using Library.Excepciones;

namespace Library
{
    public class Fachada
    {
        private static Fachada _instancia;

        private Fachada()
        {
        }

        public static Fachada Instancia => _instancia ??= new Fachada();

        private Usuario user { get; set; }

        public void SetUsuario(Usuario unUsuario)
        {
            user = unUsuario;
        }

        // ====================== USUARIOS ======================
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
            if (u == null) throw new ArgumentException("No existe un usuario con ese ID.");
            admin.EliminarUsuario(u);
        }

        public void SuspenderUsuario(Guid id)
        {
            Administrador admin = VerificarAdministrador(user);
            Usuario? u = AdministrarUsuarios.Instancia.BuscarPorId(id);
            if (u == null) throw new ArgumentException("No existe un usuario con ese ID.");
            admin.SuspenderUsuario(u);
        }

        public void RehabilitarUsuario(Guid id)
        {
            Administrador admin = VerificarAdministrador(user);
            Usuario? u = AdministrarUsuarios.Instancia.BuscarPorId(id);
            if (u == null) throw new ArgumentException("No existe un usuario con ese ID.");
            admin.ReahnilitarUsuario(u);
        }

        // ====================== CLIENTES ======================
        public Cliente CrearCliente(string nombre, string apellido, string email, string telefono, string genero,
            DateTime fechaNacimiento, Usuario usuarioAsignado)
        {
           return user.CrearCliente(nombre, apellido, email, telefono, genero, fechaNacimiento, usuarioAsignado);
        }

        public void EliminarCliente(Guid id)
        {
            user.EliminarCliente(AdministrarClientes.Instancia.BuscarClientePorId(id)
                                 ?? throw new ArgumentException("No existe un cliente con ese ID."));
        }

        public void ModificarCliente(Guid id, string? nombre = null, string? apellido = null, string? telefono = null,
            string? correo = null, DateTime? fechaNacimiento = null, string? genero = null)
        {
            Cliente? cliente = AdministrarClientes.Instancia.BuscarClientePorId(id)
                               ?? throw new ArgumentException("No existe un cliente con ese ID.");
            user.ModificarCliente(cliente, nombre, apellido, telefono, correo,
                fechaNacimiento ?? cliente.FechaDeNacimiento, genero);
        }

        public void AgregarEtiquetaACliente(Guid id, string etiqueta)
        {
            Cliente? cliente = AdministrarClientes.Instancia.BuscarClientePorId(id)
                               ?? throw new ArgumentException("No existe un cliente con ese ID.");
            user.AgregarEtiquetaACliente(cliente, etiqueta);
        }

        public Cliente BuscarClientePorCriterio(string criterio)
        {
            return AdministrarClientes.Instancia.BuscarCliente(criterio);
        }

        public void AgregarInteraccion(Guid idCliente, Interaccion interaccion)
        {
            Cliente? cliente = AdministrarClientes.Instancia.BuscarClientePorId(idCliente)
                               ?? throw new ArgumentException("No existe un cliente con ese ID.");
            user.AgregarInteraccion(cliente, interaccion);
        }

        public List<Cliente> VerClientes()
        {
            return user.VerClientes();
        }

        public List<Interaccion> VerInteraccionesCliente(Guid idCliente)
        {
            Cliente? cliente = AdministrarClientes.Instancia.BuscarClientePorId(idCliente)
                               ?? throw new ArgumentException("No existe un cliente con ese ID.");
            return user.VerInteraccionesCliente(cliente);
        }

        public List<Interaccion> VerInteraccionesCliente(Guid idCliente, string? tipo = null, DateTime? fecha = null)
        {
            Cliente? cliente = AdministrarClientes.Instancia.BuscarClientePorId(idCliente)
                               ?? throw new ArgumentException("No existe un cliente con ese ID.");
            return user.VerInteraccionesCliente(cliente, tipo, fecha);
        }

        public void EliminarInteraccion(Guid idCliente, Interaccion interaccion)
        {
            Cliente? cliente = AdministrarClientes.Instancia.BuscarClientePorId(idCliente)
                               ?? throw new ArgumentException("No existe un cliente con ese ID.");
            user.EliminarInteraccion(interaccion, cliente);
        }

        public void AgregarNota(Guid idCliente, Interaccion interaccion, string nota)
        {
            Cliente? cliente = AdministrarClientes.Instancia.BuscarClientePorId(idCliente)
                               ?? throw new ArgumentException("No existe un cliente con ese ID.");
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

        public List<Cliente> ListarClientesPorMontoMayor(int monto)
        {
            return AdministrarClientes.Instancia.ListarClientesPorMontoMayor(monto);
        }

        //Defensa
        public List<Cliente> ListarClientesPorMontoMenor(int monto)
        {
            return AdministrarClientes.Instancia.ListarClientesPorMontoMenor(monto);

        }

        public List<Cliente> ListarClientesPorRangoDeMontos(int monto1, int monto2)
        {
            return AdministrarClientes.Instancia.ListarCLientesPorMontoRango(monto1, monto2);
        }

        public List<Cliente> ListarClientesPorProducto(string nombreProducto)
        {
            return AdministrarClientes.Instancia.ListarClientesPorProducto(nombreProducto);
        }

        //Defensa

// ====================== VENTAS ======================
        public List<Venta> VerVentasPorPeriodo(DateTime fechaini, DateTime fechafin)
        {
            return user.VerVentasPorPeriodo(fechaini, fechafin);
        }

        public void RegistrarCotizacion(double total, DateTime fecha, DateTime fechaLimite, string descripcion)
        {
            user.RegistrarCotizacion(total, fecha, fechaLimite, descripcion);
        }

        public Venta CrearVenta(Vendedor vendedor, Guid idCliente, Dictionary<Producto, int> productosCantidad,
            DateTime fecha)
        {
            Cliente? cliente = AdministrarClientes.Instancia.BuscarClientePorId(idCliente)
                               ?? throw new ArgumentException("No existe un cliente con ese ID.");
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

        // Cambiar vendedor asignado
        public void CambiarVendedorAsignado(Guid idCliente, Vendedor vendedorNuevo)
        {
            Vendedor vendedor = VerificarVendedor(user);
            Cliente? cliente = AdministrarClientes.Instancia.BuscarClientePorId(idCliente)
                               ?? throw new ArgumentException("No existe un cliente con ese ID.");
            vendedor.CambiarVendedorAsignado(cliente, vendedorNuevo);
        }
    }
}
