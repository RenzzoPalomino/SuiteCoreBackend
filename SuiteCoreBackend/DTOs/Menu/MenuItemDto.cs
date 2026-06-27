namespace SuiteCoreBackend.DTOs.Menu
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool IsAssigned { get; set; }
    }
}
