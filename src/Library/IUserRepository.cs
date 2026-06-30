using System.Collections.Generic;

namespace Proyecto2026
{
    /// <summary>
    /// Contrato para el repositorio de usuarios, que permite almacenar y listar.
    /// 
    /// SOLID DIP: SystemFacade depende de esta abstracción y no de UserRepository directamente,
    /// lo que facilita cambiar la implementación sin modificar la fachada.
    /// </summary>
    public interface IUserRepository
    {

        void Add(User user);
        User GetByUsername(string username);
        IEnumerable<User> GetAll();
        bool Update(string  username);
        bool Delete(string username);
    }
}