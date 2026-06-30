using Discord.Commands;
using Discord.WebSocket;
using Proyecto2026;
using System;

namespace Proyecto2026.Commands
{
    public static class CommandHelper
    {
        public static string GetDisplayName(
            SocketCommandContext context,
            string name = null)
        {
            if (name == null)
            {
                name = context.Message.Author.Username;
            }

            foreach (SocketGuildUser user in context.Guild.Users)
            {
                if (user.Username == name
                    || user.DisplayName == name
                    || user.Nickname == name
                    || user.GlobalName == name)
                {
                    return user.DisplayName;
                }
            }

            return name;
        }

        public static SocketGuildUser GetUser(
            SocketCommandContext context,
            string name)
        {
            if (name == null)
            {
                return null;
            }

            foreach (SocketGuildUser user in context.Guild.Users)
            {
                if (user.Username == name
                    || user.DisplayName == name
                    || user.Nickname == name
                    || user.GlobalName == name)
                {
                    return user;
                }
            }

            return null;
        }

        public static User DiscordNameToUser(string discordName)
        {
            if (discordName == null)
            {
                throw new ArgumentNullException(nameof(discordName));
            }

            if (!SystemFacade.Instance.UserExists(discordName))
            {
                return SystemFacade.Instance.RegisterUser(discordName, "sinCorreo", "sinContraseña");
            }
            else
            {
                return SystemFacade.Instance.Login(discordName, "sinContraseña");
            }
        }
    }
}
