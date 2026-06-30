using System.Collections.Generic;
using System.Linq;

namespace Proyecto2026
{
    /// <summary>
    /// Clase de tipo IRecommendationFilter que excluye los ítems que contengan tags excluidos por el usuario.
    /// 
    /// SOLID OCP + Patrón Strategy: se agrega al pipeline de recomendación sin modificar
    /// RecommendationEngine ni los otros filtros existentes.
    ///
    /// SOLID SRP: su única responsabilidad es filtrar por exclusiones.
    ///
    /// GRASP Expert: delegamos en UserPreferences.Allows() la decisión de si un ítem
    /// está permitido, porque UserPreferences es quien tiene esa información.
    /// ExcludeTagsFilter no accede directamente a las listas de exclusión.
    /// </summary>

    public class ExcludeTagsFilter : IRecommendationFilter
    {
        /// <summary>
        /// Retorna solo los ítems que no tienen tags excluidos por el usuario.
        /// La evaluación la hace UserPreferences.Allows().
        /// </summary>
        public List<IContent> Apply(User user, List<IContent> items)
        {
            return items.Where(i => user.Preferences.Allows(i)).ToList();
        }
    }
}