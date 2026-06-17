namespace SuiteCoreBackend.Settings
{
    public class RadiusSettings
    {
        public string Server { get; set; } = string.Empty;
        public int AccountingPort { get; set; } = 1813;
        public string SharedSecret { get; set; } = string.Empty;
        public int TimeoutMs { get; set; } = 5000;
    }
}
