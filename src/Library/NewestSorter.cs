using System.Collections.Generic;
using System.Linq;

namespace Proyecto2026
{
    /// <summary>
    /// Ordena recomendaciones por fecha, mostrando primero el contenido más reciente.
    /// Cumplimiento OCP y patrón Strategy: agrega un criterio de orden sin modificar los ordenadores existentes.
    /// </summary>
    public class NewestSorter : IRecommendationSorter
    {
        public List<IContent> Sort(List<IContent> items)
        {
            return items.OrderByDescending(item => item.Date).ToList();
        }
    }
}
