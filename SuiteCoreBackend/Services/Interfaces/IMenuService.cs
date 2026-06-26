using SuiteCoreBackend.DTOs.Menu;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IMenuService
    {
        /// <summary>
        /// Obtiene los bloques de menú con sus vistas accesibles para el usuario,
        /// según sus gidNumbers extraídos del JWT.
        /// </summary>
        /// <param name="gidNumbers">Lista de gidNumbers del usuario autenticado</param>
        Task<List<MenuBlockDto>> GetMenusForUser(IEnumerable<string> gidNumbers);
    }
}
