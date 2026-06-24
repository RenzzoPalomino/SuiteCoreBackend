namespace SuiteCoreBackend.DTOs.Oxidized
{
    public class OxidizedBackupDto
    {
        public string DeviceName { get; set; } = string.Empty;
        public string Config { get; set; } = string.Empty;
        public string? BackupType { get; set; }
        public string? Oid { get; set; }
        public long? Epoch { get; set; }
        public int? Num { get; set; }
        public DateTime RetrievedAt { get; set; } = DateTime.Now;
    }
}
