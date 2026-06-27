using SuiteCoreBackend.DTOs.Menu;
using SuiteCoreBackend.Infrastructure.Interfaces;
using SuiteCoreBackend.Models.Entities;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Services.Implementations
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<List<MenuBlockDto>> GetMenusForUser(IEnumerable<string> gidNumbers)
        {
            var blocks = await _menuRepository.GetMenusByGids(gidNumbers);
            return MapToDto(blocks);
        }

        public async Task<List<MenuBlockDto>> GetMenusByRole(string gidNumber)
        {
            var blocks = await _menuRepository.GetMenusByRole(gidNumber);
            return MapToDto(blocks);
        }

        public async Task<List<MenuBlockDto>> GetAllMenusWithAssignment(string gidNumber)
        {
            var allBlocks = await _menuRepository.GetAllMenus();
            var assignedIds = (await _menuRepository.GetMenusByRole(gidNumber))
                .SelectMany(b => b.Menus)
                .Select(m => m.Id)
                .ToHashSet();

            return [.. allBlocks.Select(b => new MenuBlockDto
            {
                Block = b.Name,
                Order = b.Order,
                Menus = [.. b.Menus.Select(m => new MenuItemDto
                {
                    Id         = m.Id,
                    Name       = m.Name,
                    Slug       = m.Slug,
                    IsAssigned = assignedIds.Contains(m.Id)
                })]
            })];
        }

        public async Task AssignMenusToRole(string gidNumber, IEnumerable<int> menuIds, string? modifiedBy)
        {
            await _menuRepository.AssignMenusToRole(gidNumber, menuIds, modifiedBy);
        }

        private static List<MenuBlockDto> MapToDto(List<MenuBlock> blocks) =>
            [.. blocks.Select(b => new MenuBlockDto
            {
                Block = b.Name,
                Order = b.Order,
                Menus = [.. b.Menus.Select(m => new MenuItemDto
                {
                    Id   = m.Id,
                    Name = m.Name,
                    Slug = m.Slug
                })]
            })];
    }
}
