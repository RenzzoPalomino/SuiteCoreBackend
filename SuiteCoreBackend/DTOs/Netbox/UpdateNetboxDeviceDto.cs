using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Netbox;

public class UpdateNetboxDeviceDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("device_type")]
    public int? DeviceType { get; set; }

    [JsonPropertyName("role")]
    public int? Role { get; set; }

    [JsonPropertyName("site")]
    public int? Site { get; set; }

    [JsonPropertyName("rack")]
    public int? Rack { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}
