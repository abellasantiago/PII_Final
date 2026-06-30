using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;

namespace Proyecto2026.Commands
{

    /// <summary>
    /// Esta clase implementa el comando 'setpreferences', alias 'setpreferences' del bot.
    /// Este comando permite agregar preferencias al usuario que envía el mensaje.
    /// </summary>
// ReSharper disable once UnusedType.Global
    public class SetPreferencesCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Implementa el comando 'setpreferences', alias 'setpreferences' del bot.
        /// </summary>
        /// <param name="preferences">Preferencias a agregar</param>
        [Command("setpreferences")]
        [Summary(
            "Establece las preferencias de etiquetas para un usuario de Discord.")]
        // ReSharper disable once UnusedMember.Global
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("Las preferencias a agregar para el usuario que envía el comando")]
            string preferences = null)
        {
            var discordUser = Context.User as SocketGuildUser;
            string discordName = discordUser?.Username ?? "desconocido";

            if (discordName == "desconocido")
            {
                await ReplyAsync("No se pudo obtener el usuario de Discord.");
                return;
            }

            if (string.IsNullOrWhiteSpace(preferences))
            {
                await ReplyAsync(
                    "Por favor, indique las preferencias después del comando. Ejemplo: !setpreferences rock pop");
                return;
            }

            var preferredTags = new List<string>();
            foreach (var tag in preferences.Split(new[] { ',', ';', ' ' }, System.StringSplitOptions.RemoveEmptyEntries))
            {
                preferredTags.Add(tag.Trim());
            }

            User user = CommandHelper.DiscordNameToUser(discordName);

            SystemFacade.Instance.SetPreferenceTag(user, preferredTags);
            List<string> newPreferences = SystemFacade.Instance.GetUserPreferences(user.Username);

            await ReplyAsync(
                $"Preferencias actualizadas para el usuario '{user.Username}'. Nuevas preferencias: {string.Join(", ", newPreferences)}");
        }
    }
}
