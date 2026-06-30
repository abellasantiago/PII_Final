using System;

namespace Proyecto2026
{
    /// <summary>
    /// Clase base abstracta para todos los tipos de usuario del sistema.
    /// Encapsula la contraseña para proteger los datos del usuario.
    /// 
    /// SOLID SRP: esta clase solo se ocupa de la identidad del usuario (username, email, contraseña).
    ///
    /// SOLID LSP: las subclases User y Admin pueden reemplazar a GenericUser en cualquier 
    /// contexto sin alterar el comportamiento.
    /// </summary>
    public abstract class Security
    {
        public string User { get; set; }
        private string HashedPassword { get; set; }

        /// <summary>
        /// Metodo que realiza un inicio de sesion, chequeando conicidencia de username y password.
        /// </summary>
        /// <param name="username">Usuario a iniciar sesion</param>
        /// <param name="password">Contraseña del usuario</param>
        /// <returns>Retorna un bool dependiendo si el usuario ingreso correctamente</returns>
        /// </summary>
        public bool LoginUser(string username, string password)
        {
            return User == username && HashedPassword == password;
        }
    }
}