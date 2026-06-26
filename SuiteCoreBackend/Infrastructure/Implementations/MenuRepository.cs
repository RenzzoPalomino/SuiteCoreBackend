using Microsoft.EntityFrameworkCore;
using SuiteCoreBackend.Helpers;
using SuiteCoreBackend.Infrastructure.Context;
using SuiteCoreBackend.Infrastructure.Interfaces;
using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Infrastructure.Implementations
{
    public class MenuRepository : IMenuRepository
    {
        private readonly SCDbContext _context;

        public MenuRepository(SCDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuBlock>> GetMenusByGids(IEnumerable<string> gidNumbers)
        {
            return await _context.MenuBlocks
                .Where(b => b.Active)
                .OrderBy(b => b.Order)
                .Select(b => new MenuBlock
                {
                    Id = b.Id,
                    Name = b.Name,
                    Order = b.Order,
                    Menus = b.Menus
                        .Where(m => m.Active && (m.IsPublic || m.RoleMenus.Any(rm => gidNumbers.Contains(rm.GidNumber))))
                        .OrderBy(m => m.Order)
                        .ToList()
                })
                .Where(b => b.Menus.Count > 0)
                .ToListAsync();
        }

        public async Task<List<MenuBlock>> GetAllMenus()
        {
            return await _context.MenuBlocks
                .Where(b => b.Active)
                .OrderBy(b => b.Order)
                .Include(b => b.Menus.Where(m => m.Active).OrderBy(m => m.Order))
                .ToListAsync();
        }

        public async Task<List<RoleMenu>> GetRoleMenus()
        {
            return await _context.RoleMenus
                .Include(rm => rm.Menu)
                .Where(rm => rm.Active)
                .ToListAsync();
        }

        public async Task<List<MenuBlock>> GetMenusByRole(string gidNumber)
        {
            return await _context.MenuBlocks
                .Where(b => b.Active)
                .OrderBy(b => b.Order)
                .Select(b => new MenuBlock
                {
                    Id = b.Id,
                    Name = b.Name,
                    Order = b.Order,
                    Menus = b.Menus
                        .Where(m => m.Active && m.RoleMenus.Any(rm => rm.GidNumber == gidNumber && rm.Active))
                        .OrderBy(m => m.Order)
                        .ToList()
                })
                .Where(b => b.Menus.Count > 0)
                .ToListAsync();
        }

        public async Task AssignMenusToRole(string gidNumber, IEnumerable<int> menuIds, string? modifiedBy)
        {
            var menuIdList = menuIds.ToList();
            var now = new DateTimeHelper().GetPeruDateTime();

            // Desactivar todos los registros actuales del rol
            var existing = await _context.RoleMenus
                .Where(rm => rm.GidNumber == gidNumber)
                .ToListAsync();

            foreach (var rm in existing)
            {
                rm.Active = false;
                rm.ModifiedBy = modifiedBy;
                rm.ModifiedAt = now;
            }

            // Activar los entrantes (existentes) o insertar los nuevos
            var existingIds = existing.Select(rm => rm.MenuId).ToHashSet();

            foreach (var menuId in menuIdList)
            {
                if (existingIds.Contains(menuId))
                {
                    var rm = existing.First(rm => rm.MenuId == menuId);
                    rm.Active = true;
                }
                else
                {
                    _context.RoleMenus.Add(new RoleMenu
                    {
                        GidNumber = gidNumber,
                        MenuId = menuId,
                        Active = true,
                        ModifiedBy = modifiedBy,
                        ModifiedAt = now
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
