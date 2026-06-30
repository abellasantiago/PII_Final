using System;
using System.Collections.Generic;

namespace Proyecto2026
{
    /// <summary>
    /// Contrato que debe cumplir cualquier tipo de contenido que se agregue a la plataforma.
    ///
    /// SOLID DIP: RecommendationEngine y los filtros trabajan contra esta interfaz.
    ///
    /// SOLID ISP: la interfaz solo expone lo mínimo que el motor necesita para operar (título, tags, popularidad, fecha).
    /// Los atributos específicos de cada dominio (artista, álbum, género) van en las clases concretas.
    ///
    /// GRASP Protected Variations: si se incorpora un nuevo tipo de contenido,
    /// basta con implementar esta interfaz sin tocar nada del sistema existente.
    /// </summary>
    public interface IContent
    {
        string Title { get; set; } // Debe tener título.
        List<string> Tags { get; set; } // Lista de etiquetas (sirven para las recomendaciones y preferencias).
        int Popularity { get; set; } // Puntaje o popularidad del contenido en la plataforma.
        DateTime Date { get; set; } // Fecha de lanzamiento o de publicación del contenido.
        bool IsDeleted { get; } // Indica si el contenido ha sido eliminado o no.
    }
}