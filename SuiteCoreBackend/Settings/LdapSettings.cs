namespace SuiteCoreBackend.Settings
{
    public class LdapSettings
    {
        public string Server { get; set; } = "";
        public int Port { get; set; } = 636;
        public bool UseSSL { get; set; } = true;
        public string BaseDn { get; set; } = "";
        public string ServiceUser { get; set; } = "";
        public string ServicePassword { get; set; } = "";
        public string AdminUser { get; set; } = "";
        public string AdminPassword { get; set; } = "";
        public string DefaultPassword { get; set; } = "";
    }
}
