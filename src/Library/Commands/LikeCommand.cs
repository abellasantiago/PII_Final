using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{

    /// <summary>
    /// Esta clase implementa el comando 'like' del bot.
    /// </summary>
    public class LikeCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Marca como me gusta el contenido indicado
        /// </summary>
        [Command("like")]
        [Summary("Agrega un like al contenido.")]
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("Contenido a indicar me gusta")]
            string contentName = null)
        {
            if (contentName == null)
                await ReplyAsync("Ejecuta el comando seguido del titulo del contenido que deseas agregarle un like");

            var discordUser = Context.User as SocketGuildUser;
            string discordName = discordUser?.Username ?? "desconocido";

            if (discordName == "desconocido")
            {
                await ReplyAsync("No se pudo obtener el usuario de Discord.");
                return;
            }

            User user = CommandHelper.DiscordNameToUser(discordName);
            IContent contentToLike = SystemFacade.Instance.GetContentByTitle(contentName);

            if (contentToLike == null){
                await ReplyAsync("Aun no existe un contenido con ese titulo");
                return;
            }

            SystemFacade.Instance.LikeContent(user, contentToLike);
                
            await ReplyAsync($"Se ha agregado un like a: {contentToLike.Title}");
            
        }
    }
}
