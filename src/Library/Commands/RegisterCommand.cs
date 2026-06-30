using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{
    // HU1: registro de usuario nuevo en el sistema
    public class RegisterCommand : ModuleBase<SocketCommandContext>
    {
        // registra al usuario de Discord como usuario del sistema
        [Command("register")]
        [Summary("Registra tu usuario en el sistema de recomendaciones.")]
        public async Task ExecuteAsync()
        {
            var discordUser = Context.User as SocketGuildUser;
            string discordName = discordUser?.Username ?? "desconocido";

            if (discordName == "desconocido")
            {
                await ReplyAsync("No se pudo obtener el usuario de Discord.");
                return;
            }

            if (SystemFacade.Instance.UserExists(discordName))
            {
                await ReplyAsync($"El usuario '{discordName}' ya está registrado.");
                return;
            }

            SystemFacade.Instance.RegisterUser(discordName, "sinCorreo", "sinContraseña");
            await ReplyAsync($"Usuario '{discordName}' registrado correctamente. Ya podés usar el sistema.");
        }
    }
}