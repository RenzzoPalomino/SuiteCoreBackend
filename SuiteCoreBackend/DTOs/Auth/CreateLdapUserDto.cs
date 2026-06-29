namespace SuiteCoreBackend.DTOs.Auth
{
    public class CreateLdapUserDto
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string GidNumber { get; set; } = "";
        public string Department { get; set; } = "";
        public string Title { get; set; } = "";
    }
}
