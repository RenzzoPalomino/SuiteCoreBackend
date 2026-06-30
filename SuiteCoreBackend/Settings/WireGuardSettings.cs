namespace SuiteCoreBackend.Settings
{
    public class WireGuardSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 22;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Interface { get; set; } = "wg0";
    }
}
