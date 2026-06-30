namespace Proyecto2026
{
    /// <summary>
    /// Define los únicos valores válidos que puede tomar la propiedad 'Type' en Interaction.
    /// 
    /// GRASP Protected Variations: si en el futuro se agregan nuevos tipos de interacción,
    /// solo hay que modificar este enum; el resto del sistema se adapta sin cambios adicionales.
    /// </summary>
    public enum InteractionType
    {
        Played,
        Liked,
        Disliked,
        Saved
    }
}