namespace SuiteCoreBackend.DTOs.Netbox;

public class NetboxVlanDto
{
    public int Id { get; set; }
    public int Vid { get; set; }
    public string Name { get; set; } = string.Empty;
    public NetboxStatusDto Status { get; set; } = new();
    public string Description { get; set; } = string.Empty;
}
