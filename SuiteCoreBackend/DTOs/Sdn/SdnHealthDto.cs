namespace SuiteCoreBackend.DTOs.Sdn
{
    /// <summary>Estado general del servicio SCNO.</summary>
    public class SdnHealthDto
    {
        public string Status { get; set; } = string.Empty;
        public string Service { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string Bridge { get; set; } = string.Empty;
        public string Openflow { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
    }
}
