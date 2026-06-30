using System.Collections.Generic;
using System;

namespace Proyecto2026
{
    /// <summary>
    /// Representa un usuario del sistema.
    ///
    /// GRASP Expert: User es el experto en su propio historial y preferencias,
    /// porque es quien tiene esa información. Cuando algo necesita esos datos, se los pide a él.
    ///
    /// SOLID SRP: User se encarga solo de sus datos (credenciales, historial, preferencias).
    ///
    /// SOLID LSP: User puede usarse donde se espere un GenericUser sin romper nada.
    /// </summary>
    public class User : GenericUser
    {
        public UserPreferences Preferences { get; private set; }
        public List<Interaction> History { get; private set; }

        /// <summary>
        /// Constructor para crear un usuario nuevo
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="hashedPassword"></param>
        public User(string username, string email, string hashedPassword)
            : base(username, email, hashedPassword)
        {
            // Como es nuevo, las preferencias y le historial empiezan vacías.
            Preferences = new UserPreferences();
            History = new List<Interaction>();
        }

        /// <summary>
        /// Constructor para usuarios que ya existen.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="hashedPassword"></param>
        /// <param name="preferences"></param>
        /// <param name="history"></param>
        public User(string username, string email, string hashedPassword,
                    UserPreferences preferences, List<Interaction> history)
            : base(username, email, hashedPassword)
        {
            Preferences = preferences;
            History = history;
        }

        /// <summary>
        /// Registra que el usuario consumió un ítem.
        /// </summary>
        /// <param name="item"></param>
        public void Consume(IContent item)
        {
            var interaction = new Interaction(item, InteractionType.Played, DateTime.Now);
            History.Add(interaction);
        }
    }
}
