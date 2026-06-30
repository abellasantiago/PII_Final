using System.Collections.Generic;
using System.Linq;

namespace Proyecto2026
{

    /// <summary>
    /// Estrategia que recomienda ítems cuyos tags tienen algo en común con lo que el usuario ya escuchó.
    ///
    /// GRASP Expert: esta clase es la experta en derivar candidatos a partir del historial,
    /// porque trabaja con exactamente los datos que necesita (historial y tags).
    /// 
    /// SOLID SRP: su única responsabilidad es seleccionar candidatos por historial.
    /// El filtrado de consumidos y el ordenamiento los delegan otras piezas del pipeline.
    /// </summary>
    public class BasedOnHistory : IRecommendationStrategy
    {

        /// <summary>
        /// Metodo que devuelve una lista de ítems recomendados basados en el historial de consumo del usuario.
        /// </summary>
        /// <param name="user">Usuario para el cual se generan las recomendaciones.</param>
        /// <param name="items">Lista de ítems disponibles para recomendar.</param>
        /// <returns>Lista de ítems recomendados.</returns>
        /// </summary>

        public List<IContent> Recommend(User user, List<IContent> items)
        {
            if (user.History.Count == 0)
                return items;

            // Recolectar todos los tags de ítems consumidos.
            var consumedTags = user.History
                .Where(i => i.Type == InteractionType.Played)
                .SelectMany(i => i.Item.Tags)
                .ToHashSet();

            // Recomendar ítems que compartan al menos un tag con el historial.
            return items
                .Where(i => i.Tags.Any(t => consumedTags.Contains(t)))
                .ToList();
        }
    }
}
