namespace SuiteCoreBackend.Models.Entities
{
    public class LdapSettings
    {
        public string Server { get; set; } = "";
        public int Port { get; set; } = 636;
        public bool UseSSL { get; set; } = true;
        public string BaseDn { get; set; } = "";
        public string ServiceUser { get; set; } = ""; 
        public string ServicePassword { get; set; } = "";
    }

    public class LdapUser
    {
        public string DisplayName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Username { get; set; } = "";
        public string Department { get; set; } = "";
        public string Title { get; set; } = "";
        public List<string> Groups { get; set; } = new();
        public List<string> Roles { get; set; } = new(); 
    }
}
