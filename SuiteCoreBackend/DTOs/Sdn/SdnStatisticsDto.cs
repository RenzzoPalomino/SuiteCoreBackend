namespace SuiteCoreBackend.DTOs.Sdn
{
    /// <summary>Estadísticas de puertos SDN. El schema de cada puerto se define
    /// cuando el SCNO comience a retornar datos en el array.</summary>
    public class SdnStatisticsDto
    {
        public string Status { get; set; } = string.Empty;
        public List<object> Ports { get; set; } = new();
    }
}
