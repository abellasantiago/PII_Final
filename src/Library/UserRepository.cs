using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto2026
{
    /// <summary>
    /// Repositorio de usuarios. Maneja el almacenamiento y la búsqueda de usuarios.
    ///
    /// SOLID SRP: su única responsabilidad es gestionar la colección de usuarios.
    ///
    /// SOLID DIP: implementa IUserRepository, por lo que SystemFacade depende de la abstracción y no de esta clase concreta.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users;

        /// <summary>
        /// Inicializa el repositorio vacío.
        /// </summary>
        public UserRepository()
        {
            _users = new List<User>();
        }

        /// <summary>
        /// Retorna todos los usuarios registrados.
        /// </summary>
        public IEnumerable<User> GetAll()
        {
            return _users.ToList();
        }

        /// <summary>
        /// Busca un usuario por su nombre.
        /// </summary>
        public User GetByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        /// <summary>
        /// Agrega un usuario al repositorio.
        /// </summary>
        public void Add(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _users.Add(user);
        }

        public bool Update(string username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));

            var user = GetByUsername(username);
            if (user == null)
                return false;

            var index = _users.FindIndex(u => u.Username == username);
            if (index < 0)
                return false;

            _users[index] = user;
            return true;
        }

        /// <summary>
        /// Elimina un usuario del repositorio.
        /// </summary>
        /// <returns>true si se eliminó; false si no se encontró.</returns>
        public bool Delete(string username)
        {
            var user = GetByUsername(username);
            if (user == null)
            {
                return false;
            }

            return _users.Remove(user);
        }

    }
}
