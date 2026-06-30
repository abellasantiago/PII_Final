using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{

    /// <summary>
    /// Esta clase implementa el comando 'userinfo', alias 'who' o 'whois' del bot.
    /// Este comando retorna el nombre de usuario.
    /// </summary>
// ReSharper disable once UnusedType.Global
    public class UserInfoCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Implementa el comando 'userinfo', alias 'who' o 'whois' del bot.
        /// </summary>
        /// <param name="displayName">El nombre de usuario de Discord</param>
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
                    return;
                }
            }

            string userName =
                displayName ?? Context.User.Username;

            await ReplyAsync(userName);
        }
    }
}
