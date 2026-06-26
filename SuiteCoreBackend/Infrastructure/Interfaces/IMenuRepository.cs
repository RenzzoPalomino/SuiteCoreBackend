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
        /// Obtiene los menús asignados a un rol específico, agrupados por bloque.
        /// Solo incluye bloques y menús activos con asignación directa al gidNumber.
        /// </summary>
        /// <param name="gidNumber">Identificador del grupo LDAP</param>
        Task<List<MenuBlock>> GetMenusByRole(string gidNumber);

        /// <summary>
        /// Asigna en bloque un conjunto de menús a un rol. Los nuevos registros
        /// se crean con active = false. Omite los menuIds que ya existen para ese rol.
        /// </summary>
        /// <param name="gidNumber">Identificador del grupo LDAP</param>
        /// <param name="menuIds">Ids de los menús a asignar</param>
        /// <param name="modifiedBy">Usuario que realiza la operación</param>
        Task AssignMenusToRole(string gidNumber, IEnumerable<int> menuIds, string? modifiedBy);
    }
}
