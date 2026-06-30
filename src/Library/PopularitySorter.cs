using System.Collections.Generic;
using System.Linq;

namespace Proyecto2026
{
    /// <summary>
    /// Clase de tipo IRecommendationSorter que ordena los ítems recomendados por su popularidad, de mayor a menor.
    /// 
    /// GRASP Expert: esta clase es la experta en aplicar el criterio de popularidad,
    /// porque accede directamente a IContent.Popularity para ordenar.
    /// </summary>
    public class PopularitySorter : IRecommendationSorter
    {
        public List<IContent> Sort(List<IContent> items)
        {
            return items.OrderByDescending(i => i.Popularity).ToList();
        }
    }
}