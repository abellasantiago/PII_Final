using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{

    /// <summary>
    /// Esta clase implementa el comando 'dislike' del bot.
    /// </summary>
    public class DislikeCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Marca como no me gusta el contenido indicado
        /// </summary>
        [Command("dislike")]
        [Summary("Agrega un dislike al contenido.")]
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("Contenido a indicar no me gusta")]
            string contentName = null)
        {
            if (contentName == null)
                await ReplyAsync("Ejecuta el comando seguido del titulo del contenido que deseas agregarle un dislike");

            var discordUser = Context.User as SocketGuildUser;
            string discordName = discordUser?.Username ?? "desconocido";

            if (discordName == "desconocido")
            {
                await ReplyAsync("No se pudo obtener el usuario de Discord.");
                return;
            }

            User user = CommandHelper.DiscordNameToUser(discordName);
            IContent contentToDislike = SystemFacade.Instance.GetContentByTitle(contentName);

            if (contentToDislike == null){
                await ReplyAsync("Aun no existe un contenido con ese titulo");
                return;
            }

            SystemFacade.Instance.DislikeContent(user, contentToDislike);
                
            await ReplyAsync($"Se ha agregado un dislike a: {contentToDislike.Title}");
            
        }
    }
}
