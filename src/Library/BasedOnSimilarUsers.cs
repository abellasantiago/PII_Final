using System.Collections.Generic;
using System.Linq;

namespace Proyecto2026
{
    /// <summary>
    /// Estrategia que recomienda contenido consumido por usuarios con historial similar.
    /// Dos usuarios se consideran similares si sus historiales comparten al menos un tag.
    /// 
    /// SOLID OCP: se agrega como nueva forma de recomendar sin modificar  las estrategias existentes.
    /// 
    /// GRASP Expert: esta clase es la experta en calcular similitud entre usuarios,
    /// ya que trabaja con los tags del historial, que son los datos que necesita.
    /// </summary>
    public class BasedOnSimilarUsers : IRecommendationStrategy
    {
        private List<User> users;

        /// <summary>
        /// Crea la estrategia con la lista de usuarios del sistema para comparar historiales.
        /// </summary>
        /// <param name="users">Lista de usuarios.</param>
        public BasedOnSimilarUsers(List<User> users)
        {
            this.users = users ?? new List<User>();
        }

        /// <summary>
        /// Retorna ítems consumidos por usuarios similares que también estén disponibles en el catálogo.
        /// </summary>      
        public List<IContent> Recommend(User user, List<IContent> items)
        {
            var userTags = user.History
                .Where(interaction => interaction.Type == InteractionType.Played)
                .SelectMany(interaction => interaction.Item.Tags)
                .ToHashSet();

            if (userTags.Count == 0)
                return new List<IContent>();

            var recommendations = new List<IContent>();

            foreach (var otherUser in users.Where(other => other != user))
            {
                var otherTags = otherUser.History
                    .Where(interaction => interaction.Type == InteractionType.Played)
                    .SelectMany(interaction => interaction.Item.Tags)
                    .ToHashSet();

                if (!otherTags.Any(tag => userTags.Contains(tag)))
                    continue;

                foreach (var interaction in otherUser.History.Where(interaction => interaction.Type == InteractionType.Played))
                {
                    var item = interaction.Item;
                    if (items.Contains(item) && !recommendations.Contains(item))
                        recommendations.Add(item);
                }
            }

            return recommendations;
        }
    }
}
