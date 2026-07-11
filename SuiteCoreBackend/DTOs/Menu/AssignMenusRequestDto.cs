namespace SuiteCoreBackend.DTOs.Menu
{
    /// <summary>
    /// Cuerpo de la petición para asignar o desasignar menús a un rol (gidNumber).
    /// Usado por los endpoints de gestión de permisos en PermissionController.
    /// La lista de IDs reemplaza la asignación actual del rol.
    /// </summary>
    public class AssignMenusRequestDto
    {
        /// <summary>Lista de IDs de menús (session.menus.id) a asignar al rol.</summary>
        public List<int> MenuIds { get; set; } = new();
    }
}
