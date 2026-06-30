using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{

    /// <summary>
    /// Esta clase implementa el comando 'myrecommendations' del bot.
    /// </summary>
    public class RecommendByPreferencesCommand : ModuleBase<SocketCommandContext>
    {

        /// <summary>
        /// devuelve recomendaciones personalizadas para el usuario que envía el comando
        /// </summary>
        /// <returns>devuelve una lista de recomendaciones basadas en las preferencias del usuario</returns>
        [Command("myrecommendations")]
        [Summary("Devuelve recomendaciones de música basadas en tus preferencias e historial.")]
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

            List<IContent> recommendations = SystemFacade.Instance.GetRecommendationsByPreferences(user);

            if (recommendations.Count == 0)
            {
                await ReplyAsync("No tengo recomendaciones para vos todavía.");
                return;
            }

            var respuesta = new System.Text.StringBuilder();
            respuesta.AppendLine("Recomendaciones para vos, basadas en tus preferencias:");

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
