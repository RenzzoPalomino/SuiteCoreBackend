namespace SuiteCoreBackend.Models.Entities
{
    public class LdapRole
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
    }

    public class LdapUser
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
