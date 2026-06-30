using System;
using System.Collections.Generic;

namespace Proyecto2026
{
    /// <summary>
    /// Representa una canción en el catálogo. Es el único dominio concreto del sistema
    /// e implementa IContent para que el motor pueda tratarlo de forma abstracta.
    ///     
    /// SOLID LSP: Music puede usarse en cualquier lugar donde se espere un IContent
    /// sin que el sistema se rompa.
    /// </summary>
    public class Music : IContent
    {
        public string Title { get; set; }
        public List<string> Tags { get; set; }
        public int Popularity { get; set; }
        public DateTime Date { get; set; }

        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }

        private bool deleted = false;

        /// <summary>
        /// Constructor para crear una instancia de Music con sus propiedades.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="artist"></param>
        /// <param name="album"></param>
        /// <param name="genre"></param>
        /// <param name="date"></param>
        /// <param name="popularity"></param>
        public Music(string title, string artist, string album, string genre, DateTime date, int popularity = 0)
        {
            if (title == null) 
                throw new ArgumentNullException(nameof(title));

            if (artist == null) 
                throw new ArgumentNullException(nameof(artist));

            if (album == null) 
                throw new ArgumentNullException(nameof(album));
            
            if (genre == null) 
                throw new ArgumentNullException(nameof(genre));
            
            Title = title;
            Artist = artist;
            Album = album;
            Genre = genre;
            Date = date;
            Popularity = popularity;
            Tags = new List<string> { genre };
        }

        // Marca el ítem como eliminado sin borrarlo físicamente.
        // Permite que Catalog lo excluya sin romper el historial de interacciones existentes.
        public void MarkAsDeleted()
        {
            deleted = true;
        }

        // Indica si el ítem fue marcado como eliminado.
        public bool IsDeleted => deleted;
    }
}
