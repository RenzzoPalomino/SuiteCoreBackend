namespace SuiteCoreBackend.DTOs.Menu
{
    /// <summary>
    /// Representa un ítem individual de menú dentro de un bloque.
    /// Contiene la información mínima que el frontend necesita para renderizar
    /// el link de navegación y determinar si está asignado al rol actual.
    /// </summary>
    public class MenuItemDto
    {
        /// <summary>Identificador único del menú en la tabla session.menus.</summary>
        public int Id { get; set; }

        /// <summary>Nombre visible del ítem de menú (ej. "Dashboard", "Dispositivos").</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Ruta de navegación del ítem (ej. "/monitoring/dashboard"). Usada por el router del frontend.</summary>
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Indica si este menú está asignado al rol que hace la consulta.
        /// Usado en vistas de administración de permisos para mostrar el estado de asignación.
        /// </summary>
        public bool IsAssigned { get; set; }
    }
}
