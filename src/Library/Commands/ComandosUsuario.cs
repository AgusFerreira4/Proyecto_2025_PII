using System;
using System.Threading.Tasks;
using ClassLibrary;
using Discord.Commands;

namespace Library.Commands
{
    [Group("usuario")]
    public class ComandosUsuario : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada fac = Fachada.Instancia;

        /// <summary>
        /// Crea un nuevo usuario en el sistema con nombre, email, apellido y teléfono.
        /// </summary>
        /// <param name="nombre">Nombre del usuario.</param>
        /// <param name="apellido">Apellido del usuario.</param>
        /// <param name="email">Email del usuario.</param>
        /// <param name="telefono">Teléfono del usuario.</param>
        [Command("crearUser")]
        [Summary("Crea un usuario nuevo en el sistema.")]
        public async Task CrearUserAsync(
             string nombre, string apellido,string email,string telefono)
        {
            try
            {
                fac.CrearUsuario(nombre, email, apellido, telefono);

                await ReplyAsync(
                    $"Usuario **{nombre} {apellido}** creado correctamente.");
            }
            catch (Exception ex)
            {
                await ReplyAsync(
                    $"No se pudo crear el usuario: {ex.Message}");
            }
        }
        public async Task EliminarUserAsync(Usuario unUsuario)
        {
            try
            {
                fac.EliminarUsuario(unUsuario);
                await ReplyAsync(
                    $"Usuario **{unUsuario.Nombre} {unUsuario.Apellido}** eliminado correctamente."
                );
            }
            catch (Exception e)
            {
                await ReplyAsync(
                    $"No se pudo crear el usuario: {e.Message}");
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