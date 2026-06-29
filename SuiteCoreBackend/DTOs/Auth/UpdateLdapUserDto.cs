namespace SuiteCoreBackend.DTOs.Auth
{
    public class UpdateLdapUserDto
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string? GidNumber { get; set; }
    }
}
