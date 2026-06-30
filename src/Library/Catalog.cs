using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System;

namespace Proyecto2026
{

    /// <summary>
    /// Clase que representa el catálogo de ítems disponibles en la plataforma.
    /// 
    /// GRASP Expert: Catalog es el experto en saber qué ítems existen, cuáles están
    /// disponibles y cuáles fueron eliminados, porque es quien almacena esa información.
    ///
    /// SOLID SRP: su única responsabilidad es manejar la colección de contenidos.
    /// No aplica recomendaciones ni conoce al usuario.
    ///
    /// SOLID DIP: implementa IContentRepository, por lo que SystemFacade depende de la
    /// abstracción y no de esta clase concreta.
    /// </summary>
    public class Catalog : IContentRepository
    {
        private List<IContent> _items;

        /// <summary>
        /// Inicializa el catálogo vacío.
        /// </summary>
        public Catalog()
        {
            _items = new List<IContent>();
        }

        /// <summary>
        /// Retorna todos los ítems del catálogo.
        /// </summary>
        public IEnumerable<IContent> GetAll()
        {
            return _items.ToList();
        }

        /// <summary>
        /// Busca un ítem disponible por título.
        /// Retorna null si no hay ningún ítem disponible con ese título.
        /// </summary>
        /// <param name="title">Texto a buscar en el título. No puede ser null.</param>
        /// <exception cref="ArgumentNullException">Si title es null.</exception>
        public IContent GetByTitle(string title)
        {
            if (title == null)
                throw new ArgumentNullException(nameof(title));

            string normalizedInput = title.Trim().ToLowerInvariant();


            return _items
                .Where(item => !item.IsDeleted)
                .FirstOrDefault(c => c.Title.Trim().ToLowerInvariant().Contains(normalizedInput));
        }
        
        /// <summary>
        /// Agrega un nuevo ítem al catálogo.
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(IContent item)
        {
            _items.Add(item);
        }


        /// <summary>
        /// Elimina un ítem del catálogo.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(IContent item)
        {
            _items.Remove(item);
        }

        
        public bool Update(string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            var existingContent = GetByTitle(title);
            if (existingContent == null)
            {
                return false;
            }

            var index = _items.FindIndex(c => c.Title == title);
            if (index < 0)
            {
                return false;
            }

            _items[index] = existingContent;
            return true;
        }

        /// <summary>
        /// Retorna solo los ítems disponibles (no eliminados).
        /// </summary>
        /// <returns></returns>
        public List<IContent> GetAvailableItems()
        {
            return _items.Where(item => !item.IsDeleted).ToList();
        }
    }
}
