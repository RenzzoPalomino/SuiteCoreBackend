using SuiteCoreBackend.DTOs.Menu;
using SuiteCoreBackend.Infrastructure.Interfaces;
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

            return blocks.Select(b => new MenuBlockDto
            {
                Block = b.Name,
                Order = b.Order,
                Menus = b.Menus.Select(m => new MenuItemDto
                {
                    Id   = m.Id,
                    Name = m.Name,
                    Slug = m.Slug
                }).ToList()
            }).ToList();
        }
    }
}
