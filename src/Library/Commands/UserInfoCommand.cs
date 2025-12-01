using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Ucu.Poo.DiscordBot.Domain;

namespace Library.Commands
{

    /// <summary>
    /// Esta clase implementa el comando 'userinfo', alias 'who' o 'whois' del bot.
    /// Este comando retorna información sobre el usuario que envía el mensaje o sobre
    /// otro usuario si se incluye como parámetro..
    /// </summary>
// ReSharper disable once UnusedType.Global
    public class UserInfoCommand : ModuleBase<SocketCommandContext>
    {
        private Fachada fac = Fachada.Instancia;
        /// <summary>
        /// Implementa el comando 'userinfo', alias 'who' o 'whois' del bot.
        /// </summary>
        /// <param name="displayName">El nombre de usuario de Discord a buscar.</param>
        [Command("who")]
        [Summary(
            "Devuelve información sobre el usuario que se indica como " +
            "parámetro o sobre el usuario que envía el mensaje si no se "+
            "indica otro usuario.")]
        // ReSharper disable once UnusedMember.Global
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("El usuario del que tener información, opcional")]
            string displayName = null)
        {
            if (displayName != null)
            {
                SocketGuildUser user =
                    CommandHelper.GetUser(Context, displayName);

                if (user == null)
                {
                    await ReplyAsync(
                        $"No encuentro el usuario '{displayName}' en esta aplicación");
                }
            }

            string userName =
                displayName ?? CommandHelper.GetDisplayName(Context);

            string result = fac.GetUserInfo(userName);

            await ReplyAsync(result);
        }
    }
}
