namespace SuiteCoreBackend.DTOs.Menu
{
    public class MenuBlockDto
    {
        public string Block { get; set; } = string.Empty;
        public short Order { get; set; }
        public List<MenuItemDto> Menus { get; set; } = new();
    }
}
