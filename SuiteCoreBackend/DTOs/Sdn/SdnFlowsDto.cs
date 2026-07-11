namespace SuiteCoreBackend.DTOs.Sdn
{
    /// <summary>Flujos OpenFlow activos en el bridge SDN.</summary>
    public class SdnFlowsDto
    {
        public string Status { get; set; } = string.Empty;
        public int FlowCount { get; set; }
        public List<object> Flows { get; set; } = new();
    }
}
