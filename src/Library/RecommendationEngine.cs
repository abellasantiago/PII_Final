using System.Collections.Generic;
using System.Linq;
using System;

namespace Proyecto2026
{

    /// <summary>
    /// Motor de recomendación: aplica la estrategia y los filtros sobre los ítems del catálogo.
    /// Trabaja con abstracciones.
    /// </summary>
    public class RecommendationEngine
    {
        private IRecommendationStrategy strategy;
        private List<IRecommendationFilter> filters;
        private IRecommendationSorter sorter;

        /// <summary>
        /// Constructor para crear una instancia del motor de recomendación.
        /// </summary>
        /// <param name="strategy">Estrategia de recomendación a utilizar.</param>
        /// <param name="filters">Lista de filtros a aplicar.</param>
        /// <param name="sorter">Estrategia de ordenamiento a utilizar.</param>
        public RecommendationEngine(IRecommendationStrategy strategy, List<IRecommendationFilter> filters, IRecommendationSorter sorter)
        {
            if (strategy == null) 
                throw new ArgumentNullException(nameof(strategy));

            if (sorter == null) 
                throw new ArgumentNullException(nameof(sorter));

            this.strategy = strategy;
            this.sorter = sorter;
            this.filters = filters ?? new List<IRecommendationFilter>();
        }

        /// <summary>
        /// Cambia el criterio de ordenamiento del motor.
        /// Cumplimiento OCP y patrón Strategy: el motor conserva su pipeline y cambia solamente el algoritmo de orden.
        /// </summary>
        /// <param name="sorter">Nuevo criterio de ordenamiento.</param>
        public void SetSorter(IRecommendationSorter sorter)
        {
            if (sorter == null)
                throw new ArgumentNullException(nameof(sorter));

            this.sorter = sorter;
        }

        /// <summary>
        /// Genera recomendaciones para el usuario:
        /// Pipeline: estrategia → filtros → ordenamiento → resultado final.
        /// </summary>
        /// <param name="user">Usuario para el cual se generan las recomendaciones.</param>
        /// <param name="items">Lista de ítems sobre los cuales aplicar la estrategia.</param>
        /// <returns>Lista de ítems recomendados.</returns>
        public List<IContent> Recommend(User user, List<IContent> items)
        {
            // 1. Estrategia: obtiene candidatos como IRecommendableItem
            var recommended = strategy.Recommend(user, items);

            var result = recommended.OfType<IContent>().ToList();

            // 2. Filtros.
            foreach (var filter in filters)
            {
                result = filter.Apply(user, result);
            }

            // 3. Ordenamiento.
            return sorter.Sort(result);
        }
    }
}
