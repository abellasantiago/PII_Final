using System.Collections.Generic;
using System.Linq;

namespace Proyecto2026
{
    /// <summary>
    /// Estrategia que recomienda ítems cuyos tags coinciden con los géneros preferidos del usuario.
    /// 
    /// GRASP Expert: esta clase es la experta en filtrar candidatos por preferencias,
    /// porque trabaja con exactamente los datos que necesita (PreferredTags y tags de los ítems).
    /// 
    /// SOLID OCP + Patrón Strategy: implementa IRecommendationStrategy y puede intercambiarse
    /// en el motor sin modificarlo ni tocar las otras estrategias.
    ///
    /// SOLID SRP: su única responsabilidad es seleccionar candidatos por preferencias.
    /// El filtrado y el ordenamiento los delegan otras piezas del pipeline.
    /// </summary>
    public class BasedOnPreferences : IRecommendationStrategy
    {

        /// <summary>
        /// Metodo que devuelve una lista de ítems recomendados basados en las preferencias definidas por el usuario.
        /// </summary>
        /// <param name="user">Usuario para el cual se generan las recomendaciones.</param>
        /// <param name="items">Lista de ítems disponibles para recomendar.</param>
        /// <returns>Lista de ítems recomendados.</returns>
        /// </summary>
        public List<IContent> Recommend(User user, List<IContent> items)
        {
            var preferred = user.Preferences.PreferredTags;

            if (preferred.Count == 0)
                return items;

            return items
                .Where(i => i.Tags.Any(t => preferred.Contains(t)))
                .ToList();
        }
    }
}
