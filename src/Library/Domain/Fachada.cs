using System;
using ClassLibrary;
using System.Collections.Generic;
using Library.Excepciones;
using Ucu.Poo.DiscordBot.Domain;

namespace Library
{
    public class Fachada
    {
        private static Fachada _instancia;

        private Fachada()
        {
            this.usersRepository = new UsersRepository();
        }
        public static Fachada Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new Fachada();
                }

                return _instancia;
            }
            
        }
        public void SetUsuario(Usuario unUsuario)
        {
            user = unUsuario;
        }
        private IUsersRepository usersRepository;

        /// <summary>
        /// Devuelve información del usuario cuyo nombre de usuario en Discord
        /// se recibe como parámetro.
        /// </summary>
        /// <param name="userName">El nombre de usuario de Discord del usuario
        /// buscado. </param>
        /// <returns>Un texto con la información del usuario <see cref="User"/>
        /// con el nombre de usuario provisto, o texto que indica que no existe
        /// un usuario con ese nombre.
        /// </returns>
        public string GetUserInfo(string userName)
        {
            string result;

            User userFound = this.usersRepository.Find(userName);
            if (userFound == null)
            {
                result =
                    $"El usuario de Discord '{userName}' no es usuario de esta aplicación.";
            }
            else
            {
                string roles = string.Join(", ", userFound.Roles);
                result = $"El usuario '{userName}' tiene los roles " +
                         $"'{roles}' en esta aplicación.";
            }

            return result;
        }

        
        private Usuario user { get; set; }
        
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
            
            throw new PermisoDenegadoException("Acceso denegad: se requieren permisos de vendedor");
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

        public void CrearUsuario(string nombre, string email, string apellido, string telefono)
        {
            Administrador admin = VerificarAdministrador(user);
            admin.CrearUsuario(nombre, email, apellido, telefono);
        }

        public void EliminarUsuario(Usuario unUsuario)
        {
            Administrador admin = VerificarAdministrador(user);
            admin.EliminarUsuario(unUsuario);
        }

        public void SuspenderUsuario(Usuario unUsuario)
        {
            Administrador admin = VerificarAdministrador(user);
            admin.SuspenderUsuario(unUsuario);
        }

        public void RehabilitarUsuario(Usuario unUsuario)
        {
            Administrador admin = VerificarAdministrador(user);
            admin.ReahnilitarUsuario(unUsuario);
        }

        public void adne(Cliente cliente, Vendedor vendedor, Vendedor vendedorNuevo)
        {
            vendedor = VerificarVendedor(user);
            vendedor.CambiarVendedorAsignado(cliente, vendedorNuevo);
        }
    }
}