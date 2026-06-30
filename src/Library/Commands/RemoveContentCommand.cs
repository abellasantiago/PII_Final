using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{
    /// <summary>
    /// Comando para eliminar contenido del catálogo (solo administradores).
    /// </summary>
    public class RemoveContentCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Elimina un contenido del catálogo (solo administradores)
        /// </summary>
        /// <param name="contentName"></param>
        /// <returns></returns>
        [Command("remove")]
        [Summary("Elimina un contenido del catálogo (solo administradores).")]
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("Título del contenido a eliminar")]
            string contentName = null)
        {
            if (string.IsNullOrWhiteSpace(contentName))
            {
                await ReplyAsync("Ejecutá el comando seguido del título del contenido a eliminar.");
                return;
            }

            var discordUser = Context.User as SocketGuildUser;
            string discordName = discordUser?.Username ?? "desconocido";

            if (discordName == "desconocido")
            {
                await ReplyAsync("No se pudo obtener el usuario de Discord.");
                return;
            }

            Admin admin = SystemFacade.Instance.GetAdminByUsername(discordName);
            if (admin == null)
            {
                await ReplyAsync("No tenés permisos de administrador para eliminar contenido.");
                return;
            }

            IContent content = SystemFacade.Instance.GetContentByTitle(contentName);
            if (content == null)
            {
                await ReplyAsync("No existe un contenido con ese título.");
                return;
            }

            Music music = content as Music;
            if (music == null)
            {
                await ReplyAsync("El contenido indicado no es una música.");
                return;
            }

            SystemFacade.Instance.RemoveMusic(admin, music);
            await ReplyAsync($"'{music.Title}' eliminado del catálogo.");
        }
    }
}