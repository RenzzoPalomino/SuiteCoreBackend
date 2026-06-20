namespace SuiteCoreBackend.DTOs.Oxidized
{
    public class OxidizedBackupDto
    {
        public string DeviceName { get; set; } = string.Empty;
        public string Config { get; set; } = string.Empty;
        public DateTime RetrievedAt { get; set; } = DateTime.Now;
    }
}
