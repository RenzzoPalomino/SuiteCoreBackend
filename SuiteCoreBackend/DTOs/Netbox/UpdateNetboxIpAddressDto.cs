using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Netbox;

public class UpdateNetboxIpAddressDto
{
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("dns_name")]
    public string? DnsName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
