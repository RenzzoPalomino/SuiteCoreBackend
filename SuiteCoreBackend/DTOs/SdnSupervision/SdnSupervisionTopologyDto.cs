namespace SuiteCoreBackend.DTOs.SdnSupervision
{
    /// <summary>Topología del bridge SDN: estado, controlador OpenFlow y puertos activos.</summary>
    public class SdnSupervisionTopologyDto
    {
        public string Status { get; set; } = string.Empty;
        public string Bridge { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string OvsVersion { get; set; } = string.Empty;
        public List<string> Ports { get; set; } = new();
    }
}
