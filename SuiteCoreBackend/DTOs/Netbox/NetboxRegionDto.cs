namespace SuiteCoreBackend.DTOs.Netbox;

public class NetboxRegionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int SiteCount { get; set; }
    public string Description { get; set; } = string.Empty;
}
