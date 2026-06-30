using System.Collections.Generic;
using System.Linq;

namespace Proyecto2026
{
    /// <summary>
    /// Clase de tipo IRecommendationFilter que excluye los ítems que el usuario ya consumio.
    /// </summary>
    public class ExcludeConsumedFilter : IRecommendationFilter
    {

        /// <summary>
        /// Metodo que devuelve una lista de ítems ya consumidos por el usuario.
        /// </summary>
        /// <param name="user">Usuario para el cual se excluye el contenido.</param>
        /// <param name="items">Lista de ítems a filtrar.</param>
        /// <returns>Lista de ítems filtrados.</returns>
        /// </summary>
        public List<IContent> Apply(User user, List<IContent> items)
        {
            var consumed = user.History
                .Where(i => i.Type == InteractionType.Played)
                .Select(i => i.Item)
                .ToHashSet();

            return items.Where(i => !consumed.Contains(i)).ToList();
        }
    }
}
