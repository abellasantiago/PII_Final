using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{
    /// <summary>
    /// Comando para recomendar contenido basado en el historial de reproducción del usuario.
    /// </summary>
    public class RecommendByHistoryCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Ejecuta el comando para obtener recomendaciones basadas en el historial de reproducción del usuario.
        /// </summary>
        /// <returns></returns>
        [Command("recommendbyhistory")]
        [Summary("Devuelve recomendaciones basadas en tu historial de reproducción.")]
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

            List<IContent> recommendations = SystemFacade.Instance.GetRecommendationsByHistory(user);

            if (recommendations.Count == 0)
            {
                await ReplyAsync("No tengo recomendaciones basadas en tu historial todavía. Probá con !recommend.");
                return;
            }

            var respuesta = new System.Text.StringBuilder();
            respuesta.AppendLine("Recomendaciones basadas en tu historial:");

            foreach (IContent item in recommendations)
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