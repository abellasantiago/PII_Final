using System;

namespace Proyecto2026
{
    /// <summary>
    /// Clase encargada de representar una acción realizada por el usuario en la app.
    /// 
    /// GRASP Expert: Interaction sabe cuándo ocurrió la acción y con qué ítem porque es quien almacena esos datos.
    ///
    /// SOLID SRP: solo representa y encapsula los datos de una interacción.
    /// </summary>
    public class Interaction
    {
        /// <summary>
        /// Datos que guarda cada interacción (contenido con el que interactuó, tipo de interacción y fecha exacta).
        /// </summary>
        public IContent Item { get; set; }
        public InteractionType Type { get; set; }
        public DateTime Date { get; set; }

        /// <summary>
        /// Crea una interacción registrando el ítem, el tipo de acción y el momento.
        /// </summary>
        public Interaction(IContent item, InteractionType type, DateTime date)
        {
            if (item == null) 
                throw new ArgumentNullException(nameof(item));

            if (!Enum.IsDefined(typeof(InteractionType), type)) 
                throw new ArgumentException("El tipo de interacción no es válido.", nameof(type));

            Item = item;
            Type = type;
            Date = date;
        }

        /// <summary>
        /// Averigua si esta interacción sucedió dentro de un rango reciente de días.
        /// </summary>
        /// <param name="days">Número de días para considerar como reciente.</param>
        /// <returns>True si la interacción es reciente, false en caso contrario.</returns>
        public bool IsRecent(int days)
        {
            return (DateTime.Now - Date).TotalDays <= days;
        }
    }
}
