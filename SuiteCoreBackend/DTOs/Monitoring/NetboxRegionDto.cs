namespace SuiteCoreBackend.DTOs.Monitoring;

public class NetboxRegionDto
{
    public string Name { get; set; } = string.Empty;
    public int SiteCount { get; set; }
    public string Description { get; set; } = string.Empty;
}
