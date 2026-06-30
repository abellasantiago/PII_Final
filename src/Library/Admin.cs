namespace Proyecto2026
{
    /// <summary>
    /// Usuario con permisos de administración. Hereda de GenericUser.
    /// 
    /// SOLID LSP: Admin puede usarse en cualquier lugar donde se espere un GenericUser
    /// sin romper el comportamiento del sistema.
    ///
    /// </summary>
    public class Admin : GenericUser
    {
        /// <summary>
        /// Crea un administrador con sus datos de identificación.
        /// </summary>
        /// <param name="username">Nombre de usuario.</param>
        /// <param name="email">Correo electrónico.</param>
        /// <param name="hashedPassword">Contraseña (mínimo 6 caracteres).</param>
        public Admin(string username, string email, string hashedPassword)
            : base(username, email, hashedPassword)
        {
        }
    }
}