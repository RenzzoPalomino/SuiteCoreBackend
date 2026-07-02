using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Netbox;

public class CreateNetboxIpAddressDto
{
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("dns_name")]
    public string? DnsName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
