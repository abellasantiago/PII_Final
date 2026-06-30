using System.Collections.Generic;

namespace Proyecto2026
{
    /// <summary>
    /// Interfaz que define el contrato para filtros aplicables sobre una lista de ítems.
    /// 
    /// SOLID OCP: se pueden agregar nuevos filtros sin modificar RecommendationEngine.
    /// 
    /// SOLID DIP: el motor trabaja contra esta abstracción, no contra los filtros concretos.
    /// </summary>
    public interface IRecommendationFilter
    {
        /// <summary>
        /// Aplica el filtro y retorna solo los ítems que pasan la condición.
        /// </summary>
        /// <param name="user">Usuario para el cual se generan las recomendaciones.</param>
        /// <param name="items">Lista de ítems sobre los cuales aplicar el filtro.</param>
        /// <returns>Lista de ítems que cumplen con la condición del filtro.</returns>
        List<IContent> Apply(User user, List<IContent> items);
    }
}
