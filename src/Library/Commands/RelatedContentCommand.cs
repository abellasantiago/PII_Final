using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{
    /// <summary>
    /// Comando para recomendar contenido relacionado a una música específica.
    /// </summary>
    public class RelatedContentCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Ejecuta el comando para obtener recomendaciones relacionadas a la música indicada.
        /// </summary>
        /// <param name="contentName"></param>
        /// <returns></returns>
        [Command("related")]
        [Summary("Devuelve recomendaciones relacionadas a la música que indicás.")]
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("Título de la música base")]
            string contentName = null)
        {
            if (string.IsNullOrWhiteSpace(contentName))
            {
                await ReplyAsync("Ejecutá el comando seguido del título de la música base. Ejemplo: !related Bohemian Rhapsody");
                return;
            }

            var discordUser = Context.User as SocketGuildUser;
            string discordName = discordUser?.Username ?? "desconocido";

            if (discordName == "desconocido")
            {
                await ReplyAsync("No se pudo obtener el usuario de Discord.");
                return;
            }

            User user = CommandHelper.DiscordNameToUser(discordName);
            Music music = SystemFacade.Instance.GetMusicByTitleOrArtist(contentName);

            List<IContent> recommendations;
            string encabezado;

            if (music != null)
            {
                recommendations = SystemFacade.Instance.GetRelatedRecommendationsByGenre(music.Genre);
                encabezado = $"Contenido relacionado a '{music.Title}' ({music.Artist}):";
            }
            else
            {
                recommendations = SystemFacade.Instance.GetRelatedRecommendationsByGenre(contentName);
                encabezado = $"Contenido del género '{contentName}':";
            }

            if (recommendations.Count == 0)
            {
                await ReplyAsync($"No encontré contenido relacionado a '{contentName}'.");
                return;
            }

            var respuesta = new System.Text.StringBuilder();
            respuesta.AppendLine(encabezado);

            foreach (IContent item in recommendations)
            {
                Music related = item as Music;
                if (related != null)
                {
                    respuesta.AppendLine($"- {related.Title} de {related.Artist} ({related.Genre})");
                }
            }

            await ReplyAsync(respuesta.ToString());
        }
    }
}