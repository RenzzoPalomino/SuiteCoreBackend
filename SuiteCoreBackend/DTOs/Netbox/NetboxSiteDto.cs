namespace SuiteCoreBackend.DTOs.Netbox;

public class NetboxSiteDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public NetboxStatusDto Status { get; set; } = new();
    public string Facility { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
