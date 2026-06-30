using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto2026
{
    /// <summary>
    /// Fachada del sistema de recomendaciones.
    ///
    /// Patrón Facade: único punto de entrada al sistema; oculta la complejidad
    /// de Catalog, RecommendationEngine y UserRepository al cliente.
    /// 
    /// Bajo acoplamiento: el bot y los tests solo conocen SystemFacade, sin acoplarse a ninguna clase interna. 
    ///
    /// GRASP Creator: crea instancias de User e Interaction porque es quien tiene
    /// todos los datos necesarios para hacerlo. 
    /// </summary>
    public class SystemFacade
    {
        private Catalog catalog;
        private RecommendationEngine engine;
        private UserRepository users;
        private List<Admin> admins;

        private static SystemFacade instance;

        /// <summary>
        /// Constructor para crear una instancia de la fachada.
        /// </summary>
        /// <param name="catalog"></param>
        /// <param name="engine"></param>
        public SystemFacade(Catalog catalog, RecommendationEngine engine)
        {
            this.catalog = catalog ?? new Catalog();
            AddMusic(new Music("Bohemian Rhapsody", "Queen", "A Night at the Opera", "rock", new DateTime(1975, 10, 31), 95));
            AddMusic(new Music("Hotel California", "Eagles", "Hotel California", "rock", new DateTime(1977, 2, 22), 90));
            AddMusic(new Music("Blinding Lights", "The Weeknd", "After Hours", "pop", new DateTime(2019, 11, 29), 98));
            AddMusic(new Music("Shape of You", "Ed Sheeran", "Divide", "pop", new DateTime(2017, 1, 6), 97));
            AddMusic(new Music("So What", "Miles Davis", "Kind of Blue", "jazz", new DateTime(1959, 8, 17), 85));
            AddMusic(new Music("Take Five", "Dave Brubeck", "Time Out", "jazz", new DateTime(1959, 7, 1), 82));
            AddMusic(new Music("No era cierto", "No Te Va Gustar", "Este fuerte viento que sopla", "rock", new DateTime(2002, 1, 1), 82));
            AddMusic(new Music("Noche de pirata", "Fabricio Mosquera", "Cumbia y Plena Vol. 1", "cumbia", new DateTime(2018, 1, 1), 72));
            AddMusic(new Music("Lose Yourself", "Eminem", "8 Mile", "rap", new DateTime(2002, 10, 28), 93));
            AddMusic(new Music("HUMBLE.", "Kendrick Lamar", "DAMN.", "rap", new DateTime(2017, 4, 7), 91));
            AddMusic(new Music("Todo", "Los Negroni", "Cumbia y Plena Vol. 1", "plena", new DateTime(2018, 1, 1), 78));
            AddMusic(new Music("Jose sabia", "La Vela Puerca", "De bichos y flores", "rock", new DateTime(2001, 10, 1), 86));
            AddMusic(new Music("Raro", "El Cuarteto de Nos", "Raro", "rock", new DateTime(2006, 1, 1), 84));

            this.engine = engine ?? new RecommendationEngine(
            new PopularityStrategy(),
            new List<IRecommendationFilter>() { new ExcludeConsumedFilter(), new ExcludeTagsFilter() },
            new PopularitySorter());
            this.users = new UserRepository();
            this.admins = new List<Admin>();
            this.admins.Add(new Admin("gersofri", "gersofri@gmail.com", "admin123"));
        }

        public static SystemFacade Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SystemFacade(null, null);
                }
                return instance;
            }
        }

        // Usuarios

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="username">El nombre de usuario.</param>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <param name="hashedPassword">La contraseña del usuario.</param>
        /// <returns>El usuario registrado.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public User RegisterUser(string username, string email, string hashedPassword)
        {
            if (UserExists(username))
                throw new InvalidOperationException($"El usuario '{username}' ya existe.");

            var user = new User(username, email, hashedPassword);
            users.Add(user);
            return user;
        }

        /// <summary>
        /// Verifica si un usuario con el nombre dado ya existe en el sistema.
        /// </summary>
        /// <param name="username">El nombre de usuario.</param>
        /// <returns>True si el usuario existe, false en caso contrario.</returns>
        public bool UserExists(string username)
        {
            User user = users.GetByUsername(username);
            if (user != null)
                return true;
            
            return false;
        }

        /// <summary>
        /// Realiza el inicio de sesión de un usuario verificando su nombre de usuario y contraseña.
        /// </summary>
        /// <param name="username">El nombre de usuario.</param>
        /// <param name="password">La contraseña del usuario.</param>
        /// <returns>El usuario autenticado.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public User Login(string username, string password)
        {
            User user = users.GetByUsername(username);
            
            if (user == null || !user.LoginUser(username, password))
                throw new InvalidOperationException("Usuario o contraseña incorrectos.");
            return user;
        }

        /// <summary>
        /// Establece las preferencias del usuario: géneros preferidos.
        /// Cumplimiento Expert: SystemFacade delega en UserPreferences, que es el experto
        /// en gestionar sus propias listas. La facade no manipula las listas directamente.
        /// Cumplimiento SRP: la lógica de qué tags son válidos corresponde a UserPreferences,
        /// no a la facade.
        /// </summary>
        /// <param name="user">El usuario cuyas preferencias se actualizan.</param>
        /// <param name="preferredTags">Lista de tags que el usuario prefiere.</param>
        public void SetPreferenceTag(User user, List<string> preferredTags)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.Preferences.PreferredTags.Clear();
            user.Preferences.PreferredTags.AddRange(preferredTags ?? new List<string>());
        }


        /// <summary>
        /// Devuelve las preferencias del usuario: géneros preferidos
        /// </summary>
        /// <param name="username">El usuario cuyas preferencias se devuelven.</param>
        public List<string> GetUserPreferences(string username)
        {
            User user = users.GetByUsername(username);
            if (user == null)
                throw new InvalidOperationException($"El usuario '{username}' no existe.");

            return user.Preferences.PreferredTags;
        }

        /// <summary>
        /// Establece las exclusiones del usuario: géneros excluidos.
        /// Cumplimiento Expert: SystemFacade delega en UserPreferences, que es el experto
        /// en gestionar sus propias listas. La facade no manipula las listas directamente.
        /// Cumplimiento SRP: la lógica de qué tags son válidos corresponde a UserPreferences,
        /// no a la facade.
        /// </summary>
        /// <param name="user">El usuario cuyas exclusiones se actualizan.</param>
        /// <param name="excludedTags">Lista de tags que el usuario quiere excluir.</param>
        public void SetExcludedTag(User user, List<string> excludedTags)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.Preferences.ExcludedTags.Clear();
            user.Preferences.ExcludedTags.AddRange(excludedTags ?? new List<string>());
        }

                /// <summary>
        /// Devuelve las exclusiones del usuario: géneros excluidos.
        /// </summary>
        /// <param name="username">El usuario cuyas exclusiones se devuelven.</param>
        public List<string> GetUserExclusions(string username)
        {
            User user = users.GetByUsername(username);
            if (user == null)
                throw new InvalidOperationException($"El usuario '{username}' no existe.");

            return user.Preferences.ExcludedTags;
        }

        /// <summary>
        /// Define el criterio de ordenamiento de las recomendaciones.
        /// Cumplimiento Facade: expone una operación simple sin mostrar el motor al cliente.
        /// </summary>
        /// <param name="sorter">Criterio de ordenamiento.</param>
        public void SetRecommendationSorter(IRecommendationSorter sorter)
        {
            engine.SetSorter(sorter);
        }
        
        // Catálogo

        /// <summary>
        /// Agrega una nueva canción o álbum al catálogo del sistema.
        /// </summary>
        /// <param name="music">La canción o álbum a agregar.</param>
        public void AddMusic(Music music)
        {
            catalog.AddItem(music);
        }

        // Interacciones

        /// <summary>
        /// Devuelve el contenido solicitado en base a su titulo.
        /// </summary>
        /// <param name="title">El titulo del contenido.</param>
        public IContent GetContentByTitle(string title)
        {
            return catalog.GetByTitle(title);
        }

        /// <summary>
        /// Registra una nueva interacción del usuario con un contenido específico.
        /// </summary>
        /// <param name="user">El usuario que realiza la interacción.</param>
        /// <param name="item">El contenido con el que el usuario interactúa.</param>
        /// <param name="type">El tipo de interacción.</param>
        public void RegisterInteraction(User user, IContent item, InteractionType type)
        {
            var interaction = new Interaction(item, type, DateTime.Now);
            user.History.Add(interaction);
        }

        /// <summary>
        /// Registra una valoración positiva y la refleja en las preferencias del usuario.
        /// </summary>
        /// <param name="user">Usuario que valora el contenido.</param>
        /// <param name="item">Contenido valorado positivamente.</param>
        public void LikeContent(User user, IContent item)
        {
            RegisterInteraction(user, item, InteractionType.Liked);
            AddTagsWithoutDuplicates(user.Preferences.PreferredTags, item.Tags);
        }

        /// <summary>
        /// Registra una valoración negativa y la refleja en las exclusiones del usuario.
        /// </summary>
        /// <param name="user">Usuario que valora el contenido.</param>
        /// <param name="item">Contenido valorado negativamente.</param>
        public void DislikeContent(User user, IContent item)
        {
            RegisterInteraction(user, item, InteractionType.Disliked);
            AddTagsWithoutDuplicates(user.Preferences.ExcludedTags, item.Tags);
        }

        /// <summary>
        /// Guarda un contenido para consumirlo más tarde.
        /// </summary>
        /// <param name="user">Usuario que guarda el contenido.</param>
        /// <param name="item">Contenido guardado.</param>
        public void SaveContent(User user, IContent item)
        {
            RegisterInteraction(user, item, InteractionType.Saved);
        }

        /// <summary>
        /// Obtiene los contenidos consumidos por un usuario.
        /// Cumplimiento Expert: la información se toma del historial del usuario.
        /// </summary>
        /// <param name="user">Usuario cuyo historial se consulta.</param>
        /// <returns>Lista de contenidos consumidos.</returns>
        public List<IContent> GetConsumedHistory(User user)
        {
            var consumed = new List<IContent>();

            foreach (var interaction in user.History)
            {
                if (interaction.Type == InteractionType.Played && !consumed.Contains(interaction.Item))
                    consumed.Add(interaction.Item);
            }

            return consumed;
        }

        /// <summary>
        /// Obtiene los contenidos guardados por un usuario.
        /// Cumplimiento Expert: la información se toma del historial del usuario.
        /// </summary>
        /// <param name="user">Usuario cuyos guardados se consultan.</param>
        /// <returns>Lista de contenidos guardados.</returns>
        public List<IContent> GetSavedContent(User user)
        {
            var saved = new List<IContent>();

            foreach (var interaction in user.History)
            {
                if (interaction.Type == InteractionType.Saved && !saved.Contains(interaction.Item))
                    saved.Add(interaction.Item);
            }

            return saved;
        }

        // Recomendaciones

        /// <summary>
        /// Obtiene una lista de recomendaciones para un usuario específico, 
        /// basándose en popularidad del catálogo disponible.
        /// </summary>
        /// <param name="user">El usuario para quien se generan recomendaciones.</param>
        /// <returns>Una lista de contenido recomendados.</returns>
        public List<IContent> GetRecommendations(User user)
        {
            var available = catalog.GetAvailableItems();
            return engine.Recommend(user, available);
        }

        /// <summary>
        /// Obtiene una lista de recomendaciones personalizadas para un usuario específico, 
        /// basándose en sus preferencias.
        /// </summary>
        /// <param name="user">El usuario para quien se generan recomendaciones.</param>
        /// <returns>Una lista de contenido recomendados.</returns>
        public List<IContent> GetRecommendationsByPreferences(User user)
        {
            var available = catalog.GetAvailableItems();
            var strategy = new BasedOnPreferences();
            var filters = new List<IRecommendationFilter> { new ExcludeConsumedFilter(), new ExcludeTagsFilter() };
            var sorter = new PopularitySorter();
            var preferenceEngine = new RecommendationEngine(strategy, filters, sorter);
            return preferenceEngine.Recommend(user, available);
        }

        /// <summary>
        /// Obtiene recomendaciones basadas en el historial de consumo del usuario.
        /// </summary>
        /// <param name="user">Usuario para quien se generan recomendaciones.</param>
        /// <returns>Lista de contenidos recomendados.</returns>
        public List<IContent> GetRecommendationsByHistory(User user)
        {
            var available = catalog.GetAvailableItems();
            var strategy = new BasedOnHistory();
            var filters = new List<IRecommendationFilter> { new ExcludeConsumedFilter() };
            var sorter = new PopularitySorter();
            var historyEngine = new RecommendationEngine(strategy, filters, sorter);
            return historyEngine.Recommend(user, available);
        }

        /// <summary>
        /// Obtiene recomendaciones basadas en usuarios con historial similar.
        /// Cumplimiento OCP y patrón Strategy: usa una estrategia específica sin modificar las existentes.
        /// </summary>
        /// <param name="user">Usuario para quien se generan recomendaciones.</param>
        /// <returns>Lista de contenidos recomendados.</returns>
        public List<IContent> GetRecommendationsFromSimilarUsers(User user)
        {
            var strategy = new BasedOnSimilarUsers(users.GetAll().ToList());
            var filters = new List<IRecommendationFilter> { new ExcludeConsumedFilter() };
            var sorter = new PopularitySorter();
            var similarUsersEngine = new RecommendationEngine(strategy, filters, sorter);
            return similarUsersEngine.Recommend(user, catalog.GetAvailableItems());
        }

        /// <summary>
        /// Obtiene recomendaciones relacionadas a una música base a partir de su artista.
        /// Cumplimiento Facade: oculta la creación de estrategia, filtros y motor puntual.
        /// Cumplimiento OCP y patrón Strategy: usa PopularityByArtistStrategy sin modificar el motor principal.
        /// </summary>
        /// <param name="user">Usuario para quien se generan recomendaciones.</param>
        /// <param name="music">Música base usada para identificar atributos relacionados.</param>
        /// <returns>Lista de contenidos relacionados.</returns>
        public List<IContent> GetSameArtistRecommendations(User user, Music music)
        {
            if (music == null)
                throw new ArgumentNullException(nameof(music));

            var strategy = new PopularityByArtistStrategy(music.Artist, 10);

            var filters = new List<IRecommendationFilter>();
            var sorter = new PopularitySorter();
            var relatedEngine = new RecommendationEngine(strategy, filters, sorter);
            return relatedEngine.Recommend(user, catalog.GetAvailableItems());
        }

        /// <summary>
        /// Obtiene recomendaciones relacionadas a un género específico.
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public List<IContent> GetRelatedRecommendationsByGenre(string genre)
        {
            if (genre == null)
                throw new ArgumentNullException(nameof(genre));

            string g = genre.Trim().ToLowerInvariant();
            var result = new List<IContent>();

            foreach (IContent item in catalog.GetAvailableItems())
            {
                Music music = item as Music;
                if (music == null) continue;

                if (music.Genre.ToLowerInvariant().Contains(g))
                    result.Add(music);
            }

            result.Sort((a, b) => b.Popularity.CompareTo(a.Popularity));
            return result;
        }

        /// <summary>
        /// Obtiene una música del catálogo por título o artista, ignorando mayúsculas y minúsculas.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Music GetMusicByTitleOrArtist(string query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            string q = query.Trim().ToLowerInvariant();

            foreach (IContent item in catalog.GetAvailableItems())
            {
                Music music = item as Music;
                if (music == null) continue;

                if (music.Title.ToLowerInvariant().Contains(q) || music.Artist.ToLowerInvariant().Contains(q))
                    return music;
            }

            return null;
        }

        /// <summary>
        /// Devuelve el admin con ese username, o null si no existe o no es admin.
        /// </summary>
        /// <param name="username">Nombre del usuario a verificar.</param>
        public Admin GetAdminByUsername(string username)
        {
            foreach (Admin admin in admins)
            {
                if (admin.Username == username)
                    return admin;
            }
            return null;
        }

        /// <summary>
        /// Elimina una música del catálogo visible mediante borrado lógico.
        /// Cumplimiento Expert: Music conoce cómo marcarse como eliminada.
        /// Cumplimiento Facade: el cliente no necesita conocer el detalle interno de eliminación.
        /// </summary>
        /// <param name="admin">Administrador que realiza la operación.</param>
        /// <param name="music">Música a eliminar.</param>
        public void RemoveMusic(Admin admin, Music music)
        {
            if (admin == null)
                throw new ArgumentNullException(nameof(admin));

            if (music == null)
                throw new ArgumentNullException(nameof(music));

            music.MarkAsDeleted();
        }

        private void AddTagsWithoutDuplicates(List<string> destination, List<string> tags)
        {
            foreach (var tag in tags)
            {
                if (!destination.Contains(tag))
                    destination.Add(tag);
            }
        }
    }
}
