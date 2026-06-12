namespace SuiteCoreBackend.DTOs.Auth
{
    public class LdapUserDto    
    {
        public string DisplayName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Username { get; set; } = "";   // uid
        public string UidNumber { get; set; } = "";
        public string GidNumber { get; set; } = "";
        public string Department { get; set; } = "";
        public string Title { get; set; } = "";
        public List<string> Groups { get; set; } = new();
        public List<string> Roles { get; set; } = new();
    }
}
