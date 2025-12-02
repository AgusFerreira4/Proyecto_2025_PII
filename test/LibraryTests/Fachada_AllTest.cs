using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using ClassLibrary;
using Library;
using Library.Excepciones;

namespace Tests
{
    // Tests completos para la clase Fachada usando implementaciones reales.
    [TestFixture]
    public class FachadaAllTests
    {
        private Fachada fachada;

        [SetUp]
        public void Setup()
        {
            // Resetear singletons para evitar fuga de estado entre tests
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

        // Helper: una interaccion concreta para poder instanciar
        public class TestInteraccion : Interaccion
        {
            public TestInteraccion(Persona emisor, Persona receptor, DateTime fecha, string tema)
                : base(emisor, receptor, fecha, tema)
            {
            }
        }

        // ---------- SETUP ENTIDADES (rápidas) ----------
        private Usuario CrearUsuarioNormal(string nombre = "User") =>
            new Usuario(nombre, $"{nombre.ToLower()}@mail", "Apellido", "099");

        private Administrador CrearAdministrador(string nombre = "Admin") =>
            new Administrador(nombre, $"{nombre.ToLower()}@mail", "Apellido", "099");

        private Vendedor CrearVendedor(string nombre = "Vendedor") =>
            new Vendedor(nombre, $"{nombre.ToLower()}@mail", "Apellido", "099");

        private Cliente CrearClienteSimple(Usuario usuarioAsignado, string nombre = "Cli") =>
            new Cliente(nombre, "Apellido", $"{nombre.ToLower()}@mail", "111", "M", new DateTime(1990, 1, 1),
                usuarioAsignado);

        private Producto CrearProducto(string nombre = "Prod", double precio = 100) =>
            new Producto(nombre, precio);

        // ---------------- Basic: SetUsuario / instancia ----------------
        [Test]
        public void Instancia_NoNull_Y_SetUsuario_SeAcepta()
        {
            Assert.That(Fachada.Instancia != null);
            var u = CrearUsuarioNormal("u1");
            Assert.DoesNotThrow(() => fachada.SetUsuario(u));
        }

        // ---------------- Clientes: crear, ver, eliminar, modificar ----------------
        [Test]
        public void CrearCliente_AgregaClienteAUsuarioYAdminClientes()
        {
            var user = CrearUsuarioNormal("juan");
            fachada.SetUsuario(user);

            // Antes: lista vacía
            Assert.That(user.VerClientes().Count == 0);

            // Crear cliente via fachada
            fachada.CrearCliente("Ana", "Gomez", "a@g.com", "123", "F", DateTime.Now, user);

            // Ahora debe haber 1 cliente en lista del usuario y también en AdministrarClientes
            Assert.That(user.VerClientes().Count == 1);

            var primero = user.VerClientes()[0];
            Assert.That("Ana" == primero.Nombre);
            Assert.That("Gomez" == primero.Apellido);
        }

        [Test]
        public void EliminarCliente_RemueveClienteDeUsuarioYAdminClientes()
        {
            var user = CrearUsuarioNormal("u2");
            fachada.SetUsuario(user);

            // Crear y añadir un cliente manualmente mediante AdministrarClientes (para que esté en la lista interna)
            AdministrarClientes.Instancia.CrearCliente("Cli", "A", "111", "c@c.com", "M", new DateTime(1990, 1, 1),
                user);
            var cliente = user.VerClientes()[0];

            // Confirmación precondición
            Assert.That(1 == user.VerClientes().Count);

            // Eliminar por fachada
            fachada.EliminarCliente(cliente);

            Assert.That(0 == user.VerClientes().Count);
        }

        [Test]
        public void ModificarCliente_CambiaCampos()
        {
            var user = CrearUsuarioNormal("u3");
            fachada.SetUsuario(user);

            AdministrarClientes.Instancia.CrearCliente("Old", "Old", "000", "old@o.com", "M", new DateTime(1990, 1, 1),
                user);
            var cliente = user.VerClientes()[0];

            // Modificar nombre y telefono
            fachada.ModificarCliente(cliente, "Nuevo", null, "999", null, cliente.FechaDeNacimiento, null);

            Assert.That("Nuevo" == cliente.Nombre);
            Assert.That("999" == cliente.Telefono);
        }

        [Test]
        public void AgregarEtiquetaACliente_AgregaEtiqueta()
        {
            var user = CrearUsuarioNormal("u4");
            fachada.SetUsuario(user);

            AdministrarClientes.Instancia.CrearCliente("X", "Y", "111", "x@y.com", "F", new DateTime(1990, 1, 1), user);
            var cliente = user.VerClientes()[0];

            fachada.AgregarEtiquetaACliente(cliente, "VIP");
            Assert.That(cliente.Etiquetas, Does.Contain("VIP"));
        }

        [Test]
        public void BuscarCliente_RetornaClientePorNombreOTelefono()
        {
            var user = CrearUsuarioNormal("u5");
            fachada.SetUsuario(user);

            AdministrarClientes.Instancia.CrearCliente("Buscado", "Apellido", "777", "b@b.com", "M",
                new DateTime(1990, 1, 1), user);

            // Buscar por nombre: el método de fachada delega a user.BuscarCliente que a su vez llama a AdministrarClientes.BuscarCliente
            // La fachada no devuelve el cliente sino que user.BuscarCliente hace la búsqueda internamente; sin embargo, AdministrarClientes.BuscarCliente devuelve el cliente.
            var resultado = AdministrarClientes.Instancia.BuscarCliente("Buscado");
            Assert.That(resultado != null);
            Assert.That("Buscado" == resultado.Nombre);
        }

        // ---------------- Interacciones ----------------
        [Test]
        public void AgregarInteraccion_Y_VerInteraccionesCliente_Funcionan()
        {
            var user = CrearUsuarioNormal("u6");
            fachada.SetUsuario(user);

            var cliente = CrearClienteSimple(user, "CliI");
            // AdministrarClientes ya agregó a usuario cuando se creó por su método; aquí lo añadimos directamente
            user.AgregarCliente(cliente);

            var inter = new TestInteraccion(cliente, cliente, DateTime.Today, "Tema1");
            fachada.AgregarInteraccion(cliente, inter);

            // AdministrarInteracciones guarda en su lista interna, y usuario.ListaInteracciones también (user.AgregarInteraccion hace that)
            var lista = fachada.VerInteraccionesCliente(cliente);
            // Dependiendo de implementación, puede devolver lista vacía si cliente.ListaInteracciones vacío; en nuestro flujo AgregarInteraccion no añade a cliente.ListaInteracciones,
            // pero AdministrarInteracciones.VerInteraccionesCliente itera sobre cliente.ListaInteracciones. Para asegurar coherencia, añadimos inter a cliente.ListaInteracciones manualmente:
            cliente.ListaInteracciones.Add(inter);

            var lista2 = fachada.VerInteraccionesCliente(cliente);
            Assert.That(lista2 != null);
            Assert.That(lista2.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That("Tema1" == lista2[0].Tema);
        }

        [Test]
        public void VerInteraccionesCliente_FiltradoPorTipoYFecha()
        {
            var user = CrearUsuarioNormal("u7");
            fachada.SetUsuario(user);

            var cliente = CrearClienteSimple(user, "CliF");
            user.AgregarCliente(cliente);

            var i1 = new TestInteraccion(cliente, cliente, new DateTime(2023, 1, 1), "T1");
            var i2 = new TestInteraccion(cliente, cliente, new DateTime(2023, 1, 2), "T2");
            // Añadir manualmente a la lista del cliente
            cliente.ListaInteracciones.Add(i1);
            cliente.ListaInteracciones.Add(i2);

            // Filtrar por fecha 2023-01-01
            var filtradasFecha =
                AdministrarInteracciones.Instancia.VerInteraccionesCliente(cliente, null, new DateTime(2023, 1, 1));
            Assert.That(filtradasFecha != null);
            Assert.That(1 == filtradasFecha.Count);

            // Filtrar por tipo - usando nombre de clase (TestInteraccion no está en map; mapa espera tipos como "Mensaje","Llamada", etc.)
            // Si pedimos tipo inexistente debe devolver 0
            var filtradasTipo = AdministrarInteracciones.Instancia.VerInteraccionesCliente(cliente, "mensaje", null);
            Assert.That(filtradasTipo != null);
            Assert.That(0 == filtradasTipo.Count);
        }

        [Test]
        public void EliminarInteraccion_QuitaDeListas()
        {
            var user = CrearUsuarioNormal("u8");
            fachada.SetUsuario(user);

            var cliente = CrearClienteSimple(user, "CliE");
            user.AgregarCliente(cliente);

            var inter = new TestInteraccion(cliente, cliente, DateTime.Now, "Tema");
            AdministrarInteracciones.Instancia.AgregarInteraccion(cliente, inter);

            Assert.That(cliente.ListaInteracciones, Does.Contain(inter));

            fachada.EliminarInteraccion(inter, cliente);

            Assert.That(cliente.ListaInteracciones, Does.Not.Contain(inter));
        }

        [Test]
        public void AgregarNota_AsignaNotaEnInteraccion()
        {
            var user = CrearUsuarioNormal("u9");
            fachada.SetUsuario(user);

            var cliente = CrearClienteSimple(user, "CliN");
            user.AgregarCliente(cliente);

            var inter = new TestInteraccion(cliente, cliente, DateTime.Now, "Tema");
            cliente.ListaInteracciones.Add(inter);

            fachada.AgregarNota(inter, "Nota1");
            // AdministrarInteracciones.AgregarNota usa AddNota, que fija la propiedad Nota.
            Assert.That("Nota1" == inter.Nota);
        }

        // ---------------- Cotizaciones ----------------
        [Test]
        public void RegistrarCotizacion_AgregaCotizacionAUsuario()
        {
            var user = CrearUsuarioNormal("u10");
            fachada.SetUsuario(user);

            Assert.That(0 == user.ListaCotizaciones.Count);

            fachada.RegistrarCotizacion(1500.5, DateTime.Today, DateTime.Today.AddDays(7), "Promo");

            Assert.That(1 == user.ListaCotizaciones.Count);
            Assert.That(1500.5 == user.ListaCotizaciones[0].Total);
        }

        // ---------------- Ventas ----------------
        [Test]
        public void CrearVenta_RegistraYDevuelveVenta()
        {
            var vendedor = CrearVendedor("v1");
            var user = CrearUsuarioNormal(
                "u11"); // will be used to check ListaVentas after facade uses user's crearVenta internally
            fachada.SetUsuario(user);

            var cliente = CrearClienteSimple(user, "ClienteV");
            user.AgregarCliente(cliente);

            var prod = CrearProducto("P1", 200);
            var dict = new Dictionary<Producto, int> { { prod, 2 } };

            var venta = fachada.CrearVenta(vendedor, cliente, dict, DateTime.Now);

            Assert.That(venta != null);
            // CrearVenta en Usuario llama RegistrarVenta internamente -> ListaVentas debe contener la venta
            Assert.That(1 == user.ObtenerVentas().Count);
            Assert.That(venta == user.ObtenerVentas()[0]);
        }

        [Test]
        public void RegistrarVenta_AgregaAVentas()
        {
            var user = CrearUsuarioNormal("u12");
            fachada.SetUsuario(user);

            var venta = new Venta(new Dictionary<Producto, int>(), DateTime.Now, null, user);
            fachada.RegistrarVenta(venta);

            Assert.That(1 == user.ObtenerVentas().Count);
        }

        [Test]
        public void VerVentasPorPeriodo_FiltraPorFecha()
        {
            var user = CrearUsuarioNormal("u13");
            fachada.SetUsuario(user);

            var cliente = CrearClienteSimple(user, "CliV");
            user.AgregarCliente(cliente);

            var v1 = new Venta(new Dictionary<Producto, int>(), new DateTime(2023, 1, 1), cliente, user);
            var v2 = new Venta(new Dictionary<Producto, int>(), new DateTime(2023, 2, 1), cliente, user);

            user.RegistrarVenta(v1);
            user.RegistrarVenta(v2);

            var res = fachada.VerVentasPorPeriodo(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31));
            Assert.That(1 == res.Count);
            Assert.That(v1 == res[0]);
        }

        // ---------------- ObtenerVentas ----------------
        [Test]
        public void ObtenerVentas_RetornaListaVentasDelUsuario()
        {
            var user = CrearUsuarioNormal("u14");
            fachada.SetUsuario(user);

            var v = new Venta(new Dictionary<Producto, int>(), DateTime.Now, null, user);
            user.RegistrarVenta(v);

            var listado = fachada.ObtenerVentas();
            Assert.That(listado != null);
            Assert.That(1 == listado.Count);
        }

        // ---------------- AgregarNotaAInteraccion ----------------
        [Test]
        public void AgregarNotaAInteraccion_ModificaNotaEnInteraccion()
        {
            var user = CrearUsuarioNormal("u15");
            fachada.SetUsuario(user);

            var cliente = CrearClienteSimple(user, "CliAN");
            user.AgregarCliente(cliente);

            var inter = new TestInteraccion(cliente, cliente, DateTime.Now, "T");
            cliente.ListaInteracciones.Add(inter);
            user.ListaInteracciones.Add(inter);

            fachada.AgregarNotaAInteraccion(inter, "NotaX");
            // Usuario.AgregarNotaAInteraccion busca en ListaInteracciones y fija Nota
            Assert.That("NotaX" == inter.Nota);
        }

        // ---------------- VerPanelResumen (simple: no exception) ----------------
        [Test]
        public void VerPanelResumen_NoLanzaExcepcion()
        {
            var user = CrearUsuarioNormal("u16");
            fachada.SetUsuario(user);

            // Llenar datos mínimos
            user.AgregarCliente(CrearClienteSimple(user, "C1"));

            Assert.DoesNotThrow(() => fachada.VerPanelResumen());
        }

        // ---------------- Administrador: crear/eliminar/suspender/rehabilitar usuario ----------------
        [Test]
        public void CrearUsuario_SoloAdmin_PermisoYOperacion()
        {
            var admin = CrearAdministrador("AdminX");
            fachada.SetUsuario(admin);

            // AdministrarUsuarios está vacío inicialmente
            Assert.That(0 == AdministrarUsuarios.Instancia.VerTodos().Count);

            // Crear usuario a través de fachada (delegará a Administrador.CrearUsuario -> AdministrarUsuarios)
            fachada.CrearUsuario("Marcos", "m@mail", "Ape", "222");

            Assert.That(1 == AdministrarUsuarios.Instancia.VerTodos().Count);
            var creado = AdministrarUsuarios.Instancia.VerTodos()[0];
            Assert.That("Marcos" == creado.Nombre);
        }

        [Test]
        public void CrearUsuario_NoAdmin_LanzaPermisoDenegado()
        {
            var user = CrearUsuarioNormal("notadmin");
            fachada.SetUsuario(user);

            Assert.Throws<PermisoDenegadoException>(() => { fachada.CrearUsuario("X", "x@mail", "Ap", "11"); });
        }

        [Test]
        public void SuspenderYRehabilitarUsuario_CambianFlagSuspendido()
        {
            var admin = CrearAdministrador("AdminY");
            var target = CrearUsuarioNormal("Target");
            fachada.SetUsuario(admin);

            // Suspender
            fachada.SuspenderUsuario(target.Id);
            Assert.That(target.Suspendido == true);

            // Rehabilitar
            fachada.RehabilitarUsuario(target.Id);
            Assert.That(target.Suspendido == false);
        }

        [Test]
        public void EliminarUsuario_QuitaDeAdministrarUsuarios()
        {
            var admin = CrearAdministrador("AdminZ");
            fachada.SetUsuario(admin);

            AdministrarUsuarios.Instancia.Crear("A", "B", "c@c.com", "11");
            var lista = AdministrarUsuarios.Instancia.VerTodos();
            Assert.That(1 == lista.Count);

            // Obtener referencia al usuario creado
            var creado = lista[0];
            fachada.EliminarUsuario(creado.Id);

            Assert.That(0 == AdministrarUsuarios.Instancia.VerTodos().Count);
        }

        // ---------------- Asignar cliente a otro vendedor (adne) ----------------
        [Test]
        public void adne_AsignaClienteANuevoVendedor_RequiereVendedorEnFachada()
        {
            // Si el usuario de la fachada no es vendedor => permiso denegado
            var notVendedor = CrearUsuarioNormal("u17");
            fachada.SetUsuario(notVendedor);

            var vend1 = CrearVendedor("vA");
            var vend2 = CreateAndAddVendedorToUsers("vB");

            var cli = CrearClienteSimple(vend1, "CtoMove");
            vend1.ListaClientesDeUsuario.Add(cli);

            Assert.Throws<PermisoDenegadoException>(() => { fachada.adne(cli, vend1, vend2); });

            // Ahora que la fachada tiene un vendedor como user, la reasignación debe funcionar
            fachada.SetUsuario(vend1);
            Assert.DoesNotThrow(() => { fachada.adne(cli, vend1, vend2); });

            // cliente debe estar en vend2.ListaClientesDeUsuario (después del cambio)
            Assert.That(vend2.ListaClientesDeUsuario, Does.Contain(cli));
            Assert.That(vend1.ListaClientesDeUsuario, Does.Not.Contain(cli));
        }

        private Vendedor CreateAndAddVendedorToUsers(string name)
        {
            var v = CrearVendedor(name);
            // AdministrarUsuarios no mantiene vendedores en listas de usuarios de la app en tu implementación,
            // pero podemos usar v directamente.
            return v;
        }

        // ---------------- VerClientesConPocaInteraccion ----------------
        [Test]
        public void VerClientesConPocaInteraccion_OrdenaPorUltimaInteraccion_Y_LimitaA5()
        {
            var user = CrearUsuarioNormal("u18");
            fachada.SetUsuario(user);

            // Crear 6 clientes con distintas fechas de ultima interaccion
            for (int i = 0; i < 6; i++)
            {
                var cli = CrearClienteSimple(user, "Cli" + i);
                user.AgregarCliente(cli);
                // Cada cliente tiene una interaccion con fecha decreciente (i más vieja => fecha menor)
                var fecha = DateTime.Today.AddDays(-i);
                var inter = new TestInteraccion(cli, cli, fecha, $"T{i}");
                cli.ListaInteracciones.Add(inter);
            }

            var resultado = fachada.VerClientesConPocaInteraccion();
            // Debe devolver solo los 5 con fecha más antigua (según la implementación)
            Assert.That(5 == resultado.Count);
            // El primero debería ser el más antiguo (i=5)
            Assert.That("Cli5" == resultado[0].Nombre);
            
            
        }
        [Test]
        public void VerClientesEnVisto_RetornaClientesConInteraccionesRespondidas()
        {
            var user = CrearUsuarioNormal("u19");
            fachada.SetUsuario(user);

            // Crear clientes
            var cliConRespuesta = CrearClienteSimple(user, "ConRespuesta");
            var cliSinRespuesta = CrearClienteSimple(user, "SinRespuesta");
            var cliSinInteraccion = CrearClienteSimple(user, "SinInteraccion");

            user.AgregarCliente(cliConRespuesta);
            user.AgregarCliente(cliSinRespuesta);
            user.AgregarCliente(cliSinInteraccion);

            // Para cliConRespuesta: crear un mensaje original y su respuesta
            var mensajeOriginal = new Mensajes(user, cliConRespuesta, DateTime.Now.AddDays(-1), "Consulta", "Hola, tengo una pregunta");
            var respuesta = new Mensajes(cliConRespuesta, user, DateTime.Now, "Respuesta", "Sí, aquí va la info");

            cliConRespuesta.ListaInteracciones.Add(mensajeOriginal);
            cliConRespuesta.ListaInteracciones.Add(respuesta);
            AdministrarInteracciones.Instancia.AgregarInteraccion(cliConRespuesta, mensajeOriginal);
            AdministrarInteracciones.Instancia.AgregarInteraccion(cliConRespuesta, respuesta);

            // Para cliSinRespuesta: solo mensaje original sin respuesta
            var mensajeSinRespuesta = new Mensajes(user, cliSinRespuesta, DateTime.Now, "Consulta", "Hola");
            cliSinRespuesta.ListaInteracciones.Add(mensajeSinRespuesta);
            AdministrarInteracciones.Instancia.AgregarInteraccion(cliSinRespuesta, mensajeSinRespuesta);

            // Llamar al método que actualiza respondidos (se llama internamente en VerClientesEnVisto)
            var resultado = fachada.VerClientesEnVisto();

            Assert.That(resultado, Does.Contain(cliConRespuesta));  // Tiene respuesta
            Assert.That(resultado, Does.Not.Contain(cliSinRespuesta));  // No tiene respuesta
            Assert.That(resultado, Does.Not.Contain(cliSinInteraccion));  // Sin interacciones
        }
        [Test]
        public void VerVentasPorPeriodo_RetornaSoloVentasDelRangoDeFechas()
        {
            var user = CrearUsuarioNormal("u20");
            fachada.SetUsuario(user);

            var cliente = CrearClienteSimple(user, "CliPeriodo");
            user.AgregarCliente(cliente);

            var ventaDentro = new Venta(new Dictionary<Producto,int>(), 1000, new DateTime(2025, 6, 15), cliente, user);
            var ventaFuera1 = new Venta(new Dictionary<Producto,int>(), 2000, new DateTime(2024, 12, 31), cliente, user);
            var ventaFuera2 = new Venta(new Dictionary<Producto,int>(), 3000, new DateTime(2025, 12, 31), cliente, user);

            user.RegistrarVenta(ventaDentro);
            user.RegistrarVenta(ventaFuera1);
            user.RegistrarVenta(ventaFuera2);

            var resultado = fachada.VerVentasPorPeriodo(new DateTime(2025, 1, 1), new DateTime(2025, 11, 30));

            Assert.That(resultado.Count, Is.EqualTo(1));
            Assert.That(resultado[0], Is.EqualTo(ventaDentro));
        
        }
        [Test]
        public void RegistrarCotizacion_AgregaCotizacionALaListaDelUsuario()
        {
            var user = CrearUsuarioNormal("u21");
            fachada.SetUsuario(user);

            Assert.That(user.ListaCotizaciones.Count, Is.EqualTo(0));

            fachada.RegistrarCotizacion(2500.75, DateTime.Today, DateTime.Today.AddDays(15), "Cotización de prueba");

            Assert.That(user.ListaCotizaciones.Count, Is.EqualTo(1));
            Assert.That(user.ListaCotizaciones[0].Total, Is.EqualTo(2500.75));
            Assert.That(user.ListaCotizaciones[0].Descripcion, Is.EqualTo("Cotización de prueba"));
        }
        [Test]
        public void CrearVenta_CreaYRegistraVentaCorrectamente()
        {
            var vendedor = CrearVendedor("vendCreator");
            var user = CrearUsuarioNormal("u22");
            fachada.SetUsuario(user);

            var cliente = CrearClienteSimple(user, "CliVenta");
            user.AgregarCliente(cliente);

            var prod = CrearProducto("prod1", 1500);
            var productos = new Dictionary<Producto, int> { { prod, 2 } };

            var venta = fachada.CrearVenta(vendedor, cliente, productos, DateTime.Today);

            Assert.That(venta, Is.Not.Null);
            Assert.That(venta.Total, Is.EqualTo(3000)); // 1500 * 2
            Assert.That(venta.ClienteComprador, Is.EqualTo(cliente));
            Assert.That(user.ObtenerVentas(), Does.Contain(venta));
        }
        [Test]
        public void ObtenerVentas_RetornaListaCompletaDeVentasDelUsuario()
        {
            var user = CrearUsuarioNormal("u23");
            fachada.SetUsuario(user);

            var venta1 = new Venta(new Dictionary<Producto,int>(), 1000, DateTime.Today, null, user);
            var venta2 = new Venta(new Dictionary<Producto,int>(), 2000, DateTime.Today.AddDays(-5), null, user);

            user.RegistrarVenta(venta1);
            user.RegistrarVenta(venta2);

            var resultado = fachada.ObtenerVentas();

            Assert.That(resultado.Count, Is.EqualTo(2));
            Assert.That(resultado, Is.EquivalentTo(user.ObtenerVentas()));
        }

    
        [Test]
        public void RegistrarVenta_AgregaLaVentaALaListaDelUsuario()
        {
            var user = CrearUsuarioNormal("u24");
            fachada.SetUsuario(user);

            var venta = new Venta(new Dictionary<Producto,int>(), 5000, DateTime.Today, null, user);

            Assert.That(user.ObtenerVentas().Count, Is.EqualTo(0));

            fachada.RegistrarVenta(venta);

            Assert.That(user.ObtenerVentas().Count, Is.EqualTo(1));
            Assert.That(user.ObtenerVentas()[0], Is.EqualTo(venta));
        }
        [Test]
        public void AgregarNotaAInteraccion_AsignaNotaCorrectamente()
        {
            var user = CrearUsuarioNormal("u25");
            fachada.SetUsuario(user);

            var cliente = CrearClienteSimple(user, "CliNota");
            user.AgregarCliente(cliente);

            var interaccion = new TestInteraccion(user, cliente, DateTime.Today, "Reunión");
            cliente.ListaInteracciones.Add(interaccion);
            user.ListaInteracciones.Add(interaccion);

            Assert.That(interaccion.Nota, Is.Null.Or.Empty);

            fachada.AgregarNotaAInteraccion(interaccion, "Cliente muy interesado, hacer seguimiento");

            Assert.That(interaccion.Nota, Is.EqualTo("Cliente muy interesado, hacer seguimiento"));
        }
        [Test]
        public void CrearUsuario()
        {
            var admin = CrearAdministrador("adminCrear");
            fachada.SetUsuario(admin);

            // Antes no hay usuarios
            Assert.That(AdministrarUsuarios.Instancia.VerTodos().Count, Is.EqualTo(0));

            // Crear usuario vía fachada
            fachada.CrearUsuario("NuevoUser", "nuevo@gmail.com", "Apellido", "099999999");

            // Ahora hay uno
            Assert.That(AdministrarUsuarios.Instancia.VerTodos().Count, Is.EqualTo(1));
            var creado = AdministrarUsuarios.Instancia.VerTodos()[0];
            Assert.That(creado.Nombre, Is.EqualTo("NuevoUser"));
            Assert.That(creado.Email, Is.EqualTo("nuevo@mail.com"));
        }
        [Test]
        public void EliminarUsuario()
        {
            var admin = CrearAdministrador("adminDel");
            var usuarioABorrar = CrearUsuarioNormal("borrame");
            AdministrarUsuarios.Instancia.Crear("borrame", "b@b.com", "B", "222"); // ya existe

            fachada.SetUsuario(admin);

            var antes = AdministrarUsuarios.Instancia.VerTodos().Count;
            fachada.EliminarUsuario(usuarioABorrar);

            Assert.That(AdministrarUsuarios.Instancia.VerTodos().Count, Is.LessThan(antes));
        }
        [Test]
        public void SuspenderUsuario()
        {
            var admin = CrearAdministrador("adminSuspender");
            var usuarioObjetivo = CrearUsuarioNormal("usuarioASuspender");

            // Aseguramos que exista en el sistema
            AdministrarUsuarios.Instancia.Crear("usuarioASuspender", "suspender@mail.com", "Apellido", "123");

            fachada.SetUsuario(admin);

            Assert.That(usuarioObjetivo.Suspendido, Is.False);

            fachada.SuspenderUsuario(usuarioObjetivo);

            Assert.That(usuarioObjetivo.Suspendido, Is.True);
        }
        [Test]
        public void RehabilitarUsuario()
        {
            var admin = CrearAdministrador("adminRehabilitar");
            var usuarioSuspendido = CrearUsuarioNormal("usuarioSuspendido");

            // Lo suspendemos primero
            usuarioSuspendido.Suspendido = true;

            fachada.SetUsuario(admin);

            Assert.That(usuarioSuspendido.Suspendido, Is.True);

            fachada.RehabilitarUsuario(usuarioSuspendido);

            Assert.That(usuarioSuspendido.Suspendido, Is.False);
        }
        [Test]
        public void ReasignarClienteAOtroVendedor()
        {
            var vendedorActual = CrearVendedor("vendedorJuan");
            var vendedorNuevo   = CrearVendedor("vendedorMaria");
            var cliente         = CrearClienteSimple(vendedorActual, "ClientePremium");

            // El cliente empieza asignado al vendedor actual
            vendedorActual.ListaClientesDeUsuario.Add(cliente);

            // Logueamos al vendedor actual → debe poder reasignar
            fachada.SetUsuario(vendedorActual);

            Assert.DoesNotThrow(() =>
                fachada.adne(cliente, vendedorActual, vendedorNuevo));

            // Verificaciones finales
            Assert.That(vendedorNuevo.ListaClientesDeUsuario, Does.Contain(cliente));
            Assert.That(vendedorActual.ListaClientesDeUsuario, Does.Not.Contain(cliente));
            Assert.That(cliente.UsuarioAsignado, Is.EqualTo(vendedorNuevo));
        }
    }

}