using System.Collections.Generic;

namespace Proyecto2026
{
    /// <summary>
    /// Almacena las preferencias del usuario.
    ///
    /// SOLID SRP: su única responsabilidad es manejar las preferencias del usuario.
    /// No aplica recomendaciones.
    ///
    /// GRASP Expert: UserPreferences es quien sabe si un ítem está permitido para el usuario,
    /// porque es quien tiene la lista de géneros excluidos.
    /// </summary>
    public class UserPreferences
    {
        public List<string> PreferredTags { get; private set; }
        public List<string> ExcludedTags { get; private set; }

        /// <summary>
        /// Constructor para preferencias nuevas.
        /// </summary>
        public UserPreferences()
        {
            PreferredTags = new List<string>();
            ExcludedTags = new List<string>();
        }

        /// <summary>
        /// Constructor para cargar preferencias existentes.
        /// </summary>
        /// <param name="preferredTags"></param>
        /// <param name="excludedTags"></param>
        public UserPreferences(List<string> preferredTags, List<string> excludedTags)
        {
            PreferredTags = preferredTags;
            ExcludedTags = excludedTags;
        }

        /// <summary>
        /// Retorna true si el ítem pasa el filtro de exclusión.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Allows(IContent item)
        {
            // Verificar que el item no tenga tags excluidos.
            foreach (var tag in item.Tags)
            {
                if (ExcludedTags.Contains(tag))
                    return false; // Se rechaza inmediatamente
            }
            return true; // Si pasa el filtro, se permite
        }
    }
}
