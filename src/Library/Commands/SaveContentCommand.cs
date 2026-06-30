using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{
    /// <summary>
    /// Comando para guardar un contenido en la lista de guardados del usuario.
    /// </summary>
    public class SaveContentCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Ejecuta el comando para guardar un contenido en la lista de guardados del usuario.
        /// </summary>
        /// <param name="contentName"></param>
        /// <returns></returns>
        [Command("save")]
        [Summary("Guarda un contenido para escucharlo más tarde.")]
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("Título del contenido a guardar")]
            string contentName = null)
        {
            if (string.IsNullOrWhiteSpace(contentName))
            {
                await ReplyAsync("Ejecutá el comando seguido del título del contenido que querés guardar.");
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
            IContent content = SystemFacade.Instance.GetContentByTitle(contentName);

            if (content == null)
            {
                await ReplyAsync("No existe un contenido con ese título.");
                return;
            }

            SystemFacade.Instance.SaveContent(user, content);
            await ReplyAsync($"'{content.Title}' guardado para más tarde.");
        }
    }
}