using System.Collections.Generic;
using System.Linq;
using System;

namespace Proyecto2026
{

    /// <summary>
    /// Clase de tipo IRecommendationStrategy que recomienda música del mismo artista.
    /// 
    /// GRASP Expert: esta clase es la experta en filtrar y ordenar contenido por artista.
    ///
    /// SOLID OCP + Patrón Strategy: agrega un criterio de recomendación especializado
    /// sin modificar RecommendationEngine ni las otras estrategias.
    /// </summary>
    public class PopularityByArtistStrategy : IRecommendationStrategy
    {
        private readonly int topN;
        private readonly string artist;

        /// <summary>
        /// Constructor para crear una instancia de PopularityByArtistStrategy.
        /// Esta clase recomienda las canciones del mismo artista.
        /// </summary>
        /// <param name="artist">Artista usado para buscar recomendaciones relacionadas.</param>
        /// <param name="topN">El número de ítems más populares a recomendar.</param>
        public PopularityByArtistStrategy(string artist, int topN)
        {
            if (artist == null) 
                throw new ArgumentNullException(nameof(artist));

            this.artist = artist;

            if (topN <= 0 || topN > 10)
                this.topN = 10;
            else
                this.topN = topN;
        }
        
        /// <summary>
        /// Retorna los ítems del catálogo que pertenecen al artista seleccionado, ordenados por popularidad.
        /// </summary>
        public List<IContent> Recommend(User user, List<IContent> items)
        {
            return items
                .Where(i => i is Music && string.Equals(((Music)i).Artist, artist, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(i => i.Popularity)
                .Take(topN)
                .ToList();
        }
    }
}
