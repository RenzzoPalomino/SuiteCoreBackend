using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Infrastructure.Interfaces
{
    public interface IMenuRepository
    {
        /// <summary>
        /// Obtiene los bloques de menú con sus vistas accesibles para el usuario,
        /// según sus gidNumbers de LDAP. Incluye siempre los menús públicos.
        /// </summary>
        /// <param name="gidNumbers">Lista de gidNumbers del usuario autenticado</param>
        Task<List<MenuBlock>> GetMenusByGids(IEnumerable<string> gidNumbers);

        /// <summary>
        /// Obtiene todos los bloques de menú con todas sus vistas activas, sin filtro de roles.
        /// </summary>
        Task<List<MenuBlock>> GetAllMenus();

        /// <summary>
        /// Obtiene el mapeo completo de gidNumbers a menús registrados en la base de datos.
        /// </summary>
        Task<List<RoleMenu>> GetRoleMenus();

        /// <summary>
        /// Asigna un menú a un rol identificado por su gidNumber.
        /// Si la asignación ya existe, no realiza ninguna acción.
        /// </summary>
        /// <param name="gidNumber">Identificador del grupo LDAP</param>
        /// <param name="menuId">Id del menú a asignar</param>
        Task AssignMenuToRole(string gidNumber, int menuId);

        /// <summary>
        /// Elimina la asignación de un menú a un rol.
        /// Si no existe la asignación, no realiza ninguna acción.
        /// </summary>
        /// <param name="gidNumber">Identificador del grupo LDAP</param>
        /// <param name="menuId">Id del menú a desasignar</param>
        Task RemoveMenuFromRole(string gidNumber, int menuId);
    }
}
