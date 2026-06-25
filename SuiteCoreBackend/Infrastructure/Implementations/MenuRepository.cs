using Microsoft.EntityFrameworkCore;
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
                .ToListAsync();
        }

        public async Task AssignMenuToRole(string gidNumber, int menuId)
        {
            var exists = await _context.RoleMenus
                .AnyAsync(rm => rm.GidNumber == gidNumber && rm.MenuId == menuId);

            if (exists) return;

            _context.RoleMenus.Add(new RoleMenu { GidNumber = gidNumber, MenuId = menuId });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMenuFromRole(string gidNumber, int menuId)
        {
            var entry = await _context.RoleMenus
                .FirstOrDefaultAsync(rm => rm.GidNumber == gidNumber && rm.MenuId == menuId);

            if (entry is null) return;

            _context.RoleMenus.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }
}
