using System.Collections.Generic;

namespace Proyecto2026
{

    /// <summary>
    /// Contrato para las distintas estrategias de recomendación.
    /// 
    /// SOLID OCP: se pueden agregar nuevas estrategias sin modificar RecommendationEngine.
    /// 
    /// SOLID DIP: el motor depende de esta abstracción, no de ninguna implementación concreta.
    /// 
    /// SOLID ISP: interfaz mínima con un solo método; cada estrategia implementa solo lo que necesita.
    /// </summary>
    public interface IRecommendationStrategy
    {
        /// <summary>
        /// Genera una lista de ítems recomendados para el usuario.
        /// </summary>
        /// <param name="user">Usuario para el cual se generan las recomendaciones.</param>
        /// <param name="items">Lista de ítems sobre los cuales aplicar la estrategia.</param>
        /// <returns>Lista de ítems recomendados.</returns>
        List<IContent> Recommend(User user, List<IContent> items);
    }
}
