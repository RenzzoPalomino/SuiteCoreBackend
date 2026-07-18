namespace SuiteCoreBackend.DTOs.SdnSupervision
{
    /// <summary>Flujos OpenFlow activos instalados en el bridge SDN.</summary>
    public class SdnSupervisionFlowsDto
    {
        public string Status { get; set; } = string.Empty;
        public int FlowCount { get; set; }
        public List<SdnFlowEntryDto> Flows { get; set; } = new();
    }

    /// <summary>Regla de flujo OpenFlow individual.</summary>
    public class SdnFlowEntryDto
    {
        public int Table { get; set; }
        public int Priority { get; set; }
        public string Actions { get; set; } = string.Empty;
        public string Raw { get; set; } = string.Empty;
    }
}
