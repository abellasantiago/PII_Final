using System.Collections.Generic;

namespace Proyecto2026
{
    /// <summary>
    /// Interfaz que define el contrato para ordenar una lista de ítems recomendados.
    /// 
    /// SOLID OCP: agregar un nuevo sorter no requiere tocar RecommendationEngine.
    /// 
    /// SOLID DIP: el motor depende de esta abstracción.
    /// </summary>
    public interface IRecommendationSorter
    {
        /// <summary>
        /// Ordena y retorna la lista según el criterio implementado.
        /// </summary>
        /// <param name="items">Lista de ítems a ordenar.</param>
        /// <returns>Lista de ítems ordenados.</returns>
        List<IContent> Sort(List<IContent> items);
    }
}