using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;

namespace Library.Commands
{
    [Group("usuario")]
    public class ComandosUsuario : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada fac = Fachada.Instancia;

        public ComandosUsuario()
        {
            // El BOT siempre actúa como admin
            var adminBot = new Administrador("BotAdmin", "bot@crm.com", "Bot", "000");
            Fachada.Instancia.SetUsuario(adminBot);
        }


        [Command("crear")]
        public async Task CrearUserAsync(string nombre, string apellido, string email, string telefono)
        {
            try
            {
                fac.CrearUsuario(nombre, email, apellido, telefono);
                await ReplyAsync($"Usuario **{nombre} {apellido}** creado correctamente.");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"No se pudo crear el usuario: {ex.Message}");
            }
        }


        [Command("borrar")]
        public async Task EliminarUserAsync(string idTexto)
        {
            if (!Guid.TryParse(idTexto, out Guid id))
            {
                await ReplyAsync("El ID ingresado no es válido.");
                return;
            }

            try
            {
                fac.EliminarUsuario(id);
                await ReplyAsync($"Usuario con ID `{id}` eliminado correctamente.");
            }
            catch (Exception e)
            {
                await ReplyAsync($"No se pudo borrar el usuario: {e.Message}");
            }
        }

        [Command("suspender")]
        public async Task SuspenderUserAsync(string idTexto)
        {
            if (!Guid.TryParse(idTexto, out Guid id))
            {
                await ReplyAsync("El ID ingresado no es válido.");
                return;
            }

            try
            {
                fac.SuspenderUsuario(id);
                await ReplyAsync($"Usuario `{id}` suspendido correctamente.");
            }
            catch (Exception e)
            {
                await ReplyAsync($"No se pudo suspender el usuario: {e.Message}");
            }
        }

        [Command("habilitar")]
        public async Task HabilitarUserAsync(string idTexto)
        {
            if (!Guid.TryParse(idTexto, out Guid id))
            {
                await ReplyAsync("El ID ingresado no es válido.");
                return;
            }

            try
            {
                fac.RehabilitarUsuario(id);
                await ReplyAsync($"Usuario `{id}` rehabilitado correctamente.");
            }
            catch (Exception e)
            {
                await ReplyAsync($"No se pudo rehabilitar el usuario: {e.Message}");
            }
        }

        [Command("listar")]
        public async Task VerAllUsers()
        {
            try
            {
                List<Usuario> users = fac.VerUsuarios();

                if (users.Count == 0)
                {
                    await ReplyAsync("No hay usuarios registrados.");
                    return;
                }

                string respuesta = "**Usuarios registrados:**\n";

                foreach (Usuario u in users)
                {
                    respuesta +=
                        $"- **{u.Nombre} {u.Apellido}** | Email: `{u.Email}` | ID: `{u.Id}` | Suspendido: {u.Suspendido}\n";
                }

                await ReplyAsync(respuesta);
            }
            catch (Exception e)
            {
                await ReplyAsync($"Error al listar usuarios: {e.Message}");
            }
        }

        //comando que retorne los clientes con ventas mayores o menores a cierto monto o dentro de cierto rango de montos
        [Command("BuscarEntreDosValores")]
        public async Task BuscarClienteEntreValores(string valor1, string valor2)
        {
            if (!Int32.TryParse(valor1, out int v1))
            {
                await ReplyAsync("El primer valor ingresado no es válido.");
                return;
            }

            if (!Int32.TryParse(valor2, out int v2))
            {
                await ReplyAsync("El segundo valor ingresado no es válido.");
                return;
            }


            try
            {
                List<Cliente> cl = fac.BuscarClientesPorVentaEntre(v1, v2);
                if (cl.Count == 0)
                {
                    await ReplyAsync("No se han encontrado clientes.");
                    return;
                }

                string respuesta = "**clientes encontrados:**\n";
                foreach (Cliente c in cl)
                {
                    respuesta +=
                        $"- **{c.Nombre} {c.Apellido}** | Email: `{c.Email}` | Telefono: `{c.Telefono}` | Genero: {c.Genero}\n";
                }

                await ReplyAsync(respuesta);
            }
            catch (Exception e)
            {
                await ReplyAsync(e.Message);
                throw;
            }
        }


//comando que retorne los clientes con ventas de cierto producto o servicio
        [Command("BuscarClientePorProducto")]
        public async Task BuscarClientePorProducto(string nombre, string precio)
        {
            if (!double.TryParse(precio, out double p))
            {
                await ReplyAsync("El precio ingresado no es válido.");
                return;
            }

            string resultado = "**clientes encontrados:**\n";
            Producto pr = new Producto(nombre, p);
            List<Cliente> cl = fac.BuscarClientesPorProducto(pr);


            try
            {
                if (cl.Count == 0)
                {
                    await ReplyAsync("No se han encontrado clientes.");
                    return;
                }

                foreach (Cliente c in cl)
                {
                    resultado +=
                        $"- **{c.Nombre} {c.Apellido}** | Email: `{c.Email}` | Telefono: `{c.Telefono}` | Genero: {c.Genero}\n";
                }

                await ReplyAsync(resultado);
            }
            catch (Exception e)
            {
                await ReplyAsync(e.Message);
                throw;
            }
        }
    }
}
