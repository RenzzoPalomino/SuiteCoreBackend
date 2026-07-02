namespace SuiteCoreBackend.DTOs.Netbox;

public class NetboxRackDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public NetboxSiteNestedDto Site { get; set; } = new();
    public NetboxStatusDto Status { get; set; } = new();
    public NetboxRackWidthDto Width { get; set; } = new();
    public int UHeight { get; set; }
    public int StartingUnit { get; set; }
    public string Description { get; set; } = string.Empty;
    public int DeviceCount { get; set; }
}

public class NetboxRackWidthDto
{
    public int Value { get; set; }
    public string Label { get; set; } = string.Empty;
}
