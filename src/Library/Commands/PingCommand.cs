using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{

    /// <summary>
    /// Esta clase implementa el comando 'ping' del bot.
    /// Este comando retorna 'pong'.
    /// </summary>
// ReSharper disable once UnusedType.Global
    public class PingCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Implementa el comando 'ping'.
        /// </summary>
        [Command("ping")]
        [Summary(
            "Devuelve 'pong'.")]
        // ReSharper disable once UnusedMember.Global
        public async Task ExecuteAsync()
        {
            await ReplyAsync("pong");
        }
    }
}
