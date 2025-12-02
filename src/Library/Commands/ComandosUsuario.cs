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

        public async Task SuspenderUserAsync(Usuario unUsuario)
        {
            try
            {
                fac.SuspenderUsuario(unUsuario);
                await ReplyAsync(
                $"Usuario **{unUsuario.Nombre} {unUsuario.Apellido}** suspendido correctamente.");
            }
            catch (Exception e)
            {
                await ReplyAsync($"No se pudo suspender el usuario: {e.Message}");
            }
        }

        public async Task RehabilitarUserAsync(Usuario unUsuario)
        {
            try
            {
                fac.RehabilitarUsuario(unUsuario);
                await ReplyAsync($"Usuario: {unUsuario.Nombre} {unUsuario.Apellido} rehabilitado correctamente.");

            }
            catch (Exception e)
            {
                await ReplyAsync($"No se ha podido rehabilitar el usuario: {e.Message}");
            }
        }
        
    }
}
