using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{

    /// <summary>
    /// Esta clase implementa el comando 'history' del bot.
    /// </summary>
    public class HistoryCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Devuelve el historial del usuario que envía el comando
        /// </summary>
        /// <returns>devuelve una lista de contenidos ya reproducidos</returns>
        [Command("history")]
        [Summary("Devuelve el historial de contenido del usuario.")]
        public async Task ExecuteAsync()
        {
            var discordUser = Context.User as SocketGuildUser;
            string discordName = discordUser?.Username ?? "desconocido";

            if (discordName == "desconocido")
            {
                await ReplyAsync("No se pudo obtener el usuario de Discord.");
                return;
            }

            User user = CommandHelper.DiscordNameToUser(discordName);

            List<IContent> history = SystemFacade.Instance.GetConsumedHistory(user);

            if (history.Count == 0)
            {
                await ReplyAsync("No contas con historial.");
                return;
            }

            var respuesta = new System.Text.StringBuilder();
            respuesta.AppendLine("Tu historial:");

            foreach (IContent item in history)
            {
                Music music = item as Music;
                if (music != null)
                {
                    respuesta.AppendLine($"- {music.Title} de {music.Artist} ({music.Genre})");
                }
            }

            await ReplyAsync(respuesta.ToString());
        }
    }
}
