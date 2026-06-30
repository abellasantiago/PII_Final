using System.Collections.Generic;

namespace Proyecto2026
{
    /// <summary>
    /// Contrato para el repositorio de contenidos del catálogo.
    ///
    /// SOLID DIP: SystemFacade depende de esta abstracción y no de Catalog directamente,
    /// lo que permite cambiar la implementación sin tener que modificar la fachada.
    /// </summary>
    public interface IContentRepository
    {

        void AddItem(IContent content);
        IContent GetByTitle(string title);
        IEnumerable<IContent> GetAll();
        bool Update(string  title);
        void RemoveItem(IContent item);
    }
}