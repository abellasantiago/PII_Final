using System.Collections.Generic;
using System.Linq;

namespace Proyecto2026
{

    /// <summary>
    /// Clase de tipo IRecommendationStrategy que recomienda los ítems más populares del catálogo.\
    /// 
    /// GRASP Expert: esta clase es la experta en seleccionar candidatos por popularidad,
    /// porque accede directamente a IContent.Popularity para ordenar.
    /// </summary>
    public class PopularityStrategy : IRecommendationStrategy
    {
        private readonly int topN;

        /// <summary>
        /// Constructor para crear una instancia de PopularityStrategy.
        /// </summary>
        /// <param name="topN">El número de ítems más populares a recomendar. El maximo es 10 para mantener el sistema estable.</param>
        public PopularityStrategy(int topN = 10)
        {
            if (topN <= 0 || topN > 10)
                this.topN = 10;
            else
                this.topN = topN;
        }
        
        /// <summary>
        /// Retorna los topN ítems más populares del catálogo disponible.
        /// </summary>
        public List<IContent> Recommend(User user, List<IContent> items)
        {
            return items
                .OrderByDescending(i => i.Popularity)
                .Take(topN)
                .ToList();
        }
    }
}
