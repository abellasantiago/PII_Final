using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{

    /// <summary>
    /// Esta clase implementa el comando 'play' del bot.
    /// </summary>
    public class PlayContentCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// devuelve el contenido a reproducir
        /// </summary>
        /// <returns>devuelve un contenido a reproducir</returns>
        [Command("play")]
        [Summary("Reproduce el contenido solicitado.")]
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("Contenido a reproducir")]
            string contentName = null)
        {
            if (contentName == null)
                await ReplyAsync("Ejecuta el comando seguido del titulo del contenido que deseas reproducir");

            var discordUser = Context.User as SocketGuildUser;
            string discordName = discordUser?.Username ?? "desconocido";

            if (discordName == "desconocido")
            {
                await ReplyAsync("No se pudo obtener el usuario de Discord.");
                return;
            }

            User user = CommandHelper.DiscordNameToUser(discordName);
            IContent contentToPlay = SystemFacade.Instance.GetContentByTitle(contentName);

            if (contentToPlay == null){
                await ReplyAsync("Aun no existe un contenido con ese titulo");
                return;
            }
                

            SystemFacade.Instance.RegisterInteraction(user, contentToPlay, InteractionType.Played);

            Music music = contentToPlay as Music;
            if (music != null)
            {
                await ReplyAsync($"Contenido a reproducir: {music.Title} de {music.Artist} ({music.Genre})");
                return;
            }
                
            await ReplyAsync($"Contenido a reproducir: {contentToPlay.Title}");
            
        }
    }
}
