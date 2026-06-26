using SuiteCoreBackend.DTOs.Menu;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IMenuService
    {
        /// <summary>
        /// Obtiene los bloques de menú con sus vistas accesibles para el usuario,
        /// según sus gidNumbers extraídos del JWT.
        /// </summary>
        Task<List<MenuBlockDto>> GetMenusForUser(IEnumerable<string> gidNumbers);

        /// <summary>
        /// Obtiene los menús asignados a un rol específico, agrupados por bloque.
        /// </summary>
        Task<List<MenuBlockDto>> GetMenusByRole(string gidNumber);

        /// <summary>
        /// Reemplaza los menús activos de un rol: desactiva los existentes
        /// y activa únicamente los menuIds entrantes.
        /// </summary>
        Task AssignMenusToRole(string gidNumber, IEnumerable<int> menuIds, string? modifiedBy);
    }
}
