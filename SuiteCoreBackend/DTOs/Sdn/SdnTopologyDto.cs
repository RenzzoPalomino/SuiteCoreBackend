namespace SuiteCoreBackend.DTOs.Sdn
{
    /// <summary>Topología SDN: bridge OVS, controlador y puertos activos.</summary>
    public class SdnTopologyDto
    {
        public string Status { get; set; } = string.Empty;
        public string Bridge { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string OvsVersion { get; set; } = string.Empty;
        public List<string> Ports { get; set; } = new();
    }
}
