namespace SuiteCoreBackend.DTOs.Auth
{
    public class LdapRoleDto
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int TotalUsers { get; set; }
        public List<LdapUserDto> Users { get; set; } = new();
    }
}
