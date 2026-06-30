using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{
    /// <summary>
    /// Comando que recomienda contenido basado en usuarios con gustos similares al usuario que envía el comando.
    /// </summary>
    public class RecommendSimilarUsersCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Ejecuta el comando para recomendar contenido basado en usuarios con gustos similares.
        /// </summary>
        /// <returns></returns>
        [Command("recommendsimilar")]
        [Summary("Devuelve recomendaciones basadas en usuarios con gustos similares a los tuyos.")]
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

            List<IContent> recommendations = SystemFacade.Instance.GetRecommendationsFromSimilarUsers(user);

            if (recommendations.Count == 0)
            {
                await ReplyAsync("No encontré usuarios similares con contenido nuevo para recomendarte.");
                return;
            }

            var respuesta = new System.Text.StringBuilder();
            respuesta.AppendLine("Recomendaciones basadas en usuarios con gustos similares:");

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