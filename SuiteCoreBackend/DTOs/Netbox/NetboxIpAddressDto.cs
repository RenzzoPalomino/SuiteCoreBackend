namespace SuiteCoreBackend.DTOs.Netbox;

public class NetboxIpAddressDto
{
    public int Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public NetboxStatusDto Status { get; set; } = new();
    public string DnsName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class NetboxStatusDto
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}
