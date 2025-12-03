using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;

namespace Library.Commands
{
    [Group("cliente")]
    public class ComandosCliente : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada fac = Fachada.Instancia;

        public ComandosCliente()
        {
            var adminBot = new Administrador("BotAdmin", "bot@crm.com", "Bot", "000");
            Fachada.Instancia.SetUsuario(adminBot);
        }

        [Command("crear")]
        public async Task CrearClienteAsync(string nombre, string apellido, string email, string telefono, string genero, DateTime fechaNacimiento, string idUsuario)
        {
            if (!Guid.TryParse(idUsuario, out Guid guidUsuario))
            {
                await ReplyAsync("El ID de usuario no es válido.");
                return;
            }

            Usuario? usuarioAsignado = fac.BuscarUsuarioPorId(guidUsuario);
            if (usuarioAsignado == null)
            {
                await ReplyAsync("No se encontró un usuario con ese ID.");
                return;
            }

            try
            {
                fac.CrearCliente(nombre, apellido, email, telefono, genero, fechaNacimiento, usuarioAsignado);
                await ReplyAsync($"Cliente **{nombre} {apellido}** creado correctamente.");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"No se pudo crear el cliente: {ex.Message}");
            }
        }

        [Command("eliminar")]
        public async Task EliminarClienteAsync(string idCliente)
        {
            if (!Guid.TryParse(idCliente, out Guid guidCliente))
            {
                await ReplyAsync("El ID del cliente no es válido.");
                return;
            }

            try
            {
                fac.EliminarCliente(guidCliente);
                await ReplyAsync($"Cliente con ID `{guidCliente}` eliminado correctamente.");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"No se pudo eliminar el cliente: {ex.Message}");
            }
        }

        [Command("modificar")]
        public async Task ModificarClienteAsync(string idCliente, string? nombre = null, string? apellido = null, string? telefono = null, string? email = null, DateTime? fechaNacimiento = null, string? genero = null)
        {
            if (!Guid.TryParse(idCliente, out Guid guidCliente))
            {
                await ReplyAsync("El ID del cliente no es válido.");
                return;
            }

            try
            {
                fac.ModificarCliente(guidCliente, nombre, apellido, telefono, email, fechaNacimiento, genero);
                await ReplyAsync($"Cliente con ID `{guidCliente}` modificado correctamente.");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"No se pudo modificar el cliente: {ex.Message}");
            }
        }

        [Command("listar")]
        public async Task ListarClientesAsync()
        {
            List<Cliente> clientes = fac.VerClientes();
            if (clientes.Count == 0)
            {
                await ReplyAsync("No hay clientes registrados.");
                return;
            }

            string mensaje = "**Clientes registrados:**\n";
            foreach (Cliente c in clientes)
            {
                mensaje += $"- **{c.Nombre} {c.Apellido}** | Email: `{c.Email}` | ID: `{c.Id}` | Tel: {c.Telefono}\n";
            }

            await ReplyAsync(mensaje);
        }

        [Command("buscar")]
        public async Task BuscarClienteAsync(string criterio)
        {
            try
            {
                Cliente cliente = fac.BuscarClientePorCriterio(criterio);
                await ReplyAsync($"Cliente encontrado: **{cliente.Nombre} {cliente.Apellido}** | Email: `{cliente.Email}` | Tel: {cliente.Telefono} | ID: `{cliente.Id}`");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"No se encontró ningún cliente con ese criterio: {ex.Message}");
            }
        }
        
        //Defensa (no me dio el tiempo para agregar todos los comanods pero en las pruebas unitarias funcionan todos)
        [Command("listarPorPoducto")]
        public async Task ListarPorProducto(string nombreProducto)
        {
            try
            {
                List<Cliente> lista = fac.ListarClientesPorProducto(nombreProducto);
                string mensaje = "Lista Clinetes \n";
                foreach (Cliente cli in lista)
                {
                    mensaje += $"Cliente {cli.Nombre}";
                }

                ReplyAsync(mensaje);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}


/*
 * comando que retorne los clientes con ventas mayores o menores a cierto monto o dentro de un cierto rango de montos
 * comando que retorne los clientes con ventas de cierto producto o servicio.
 *
 * Incluir pruebas unitarias
*/