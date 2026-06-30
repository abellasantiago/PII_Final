using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{

    /// <summary>
    /// Esta clase implementa el comando 'setexclusions', alias 'setexclusions' del bot.
    /// Este comando permite agregar preferencias al usuario que envía el mensaje.
    /// </summary>
// ReSharper disable once UnusedType.Global
    public class SetExcludedCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Implementa el comando 'setexclusions', alias 'setexclusions' del bot.
        /// </summary>
        /// <param name="excluded">Exclusiones a agregar</param>
        [Command("setexclusions")]
        [Summary(
            "Establece las exclusiones de etiquetas para un usuario de Discord.")]
        // ReSharper disable once UnusedMember.Global
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("Las exclusiones a agregar para el usuario que envía el comando")]
            string excluded = null)
        {
            var discordUser = Context.User as SocketGuildUser;
            string discordName = discordUser?.Username ?? "desconocido";

            if (discordName == "desconocido")
            {
                await ReplyAsync("No se pudo obtener el usuario de Discord.");
                return;
            }

            if (string.IsNullOrWhiteSpace(excluded))
            {
                await ReplyAsync(
                    "Por favor, indique las preferencias después del comando. Ejemplo: !setpreferences rock pop");
                return;
            }

            var excludedTags = new List<string>();
            foreach (var tag in excluded.Split(new[] { ',', ';', ' ' }, System.StringSplitOptions.RemoveEmptyEntries))
            {
                excludedTags.Add(tag.Trim());
            }

            User user = CommandHelper.DiscordNameToUser(discordName);

            SystemFacade.Instance.SetExcludedTag(user, excludedTags);

            List<string> newPreferences = SystemFacade.Instance.GetUserPreferences(user.Username);
            List<string> newExclusions = SystemFacade.Instance.GetUserExclusions(user.Username);

            await ReplyAsync(
                $"Preferencias actualizadas para el usuario '{user.Username}'. Nuevas preferencias: {string.Join(", ", newExclusions)}");
        }
    }
}
