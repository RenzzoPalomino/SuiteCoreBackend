namespace SuiteCoreBackend.DTOs.Menu
{
    /// <summary>
    /// Representa un bloque de menú con sus ítems asociados.
    /// Retornado por GET /api/permission/Menus agrupando los menús accesibles
    /// según el gidNumber del usuario autenticado.
    /// </summary>
    public class MenuBlockDto
    {
        /// <summary>Nombre del bloque de menú (ej. "Monitoreo", "Administración").</summary>
        public string Block { get; set; } = string.Empty;

        /// <summary>Orden de visualización del bloque en el menú lateral del frontend.</summary>
        public short Order { get; set; }

        /// <summary>Lista de ítems de menú accesibles dentro de este bloque para el rol del usuario.</summary>
        public List<MenuItemDto> Menus { get; set; } = new();
    }
}
