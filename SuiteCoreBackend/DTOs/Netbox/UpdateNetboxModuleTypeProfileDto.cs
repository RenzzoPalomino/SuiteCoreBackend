using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Netbox;

public class UpdateNetboxModuleTypeProfileDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("schema")]
    public object? Schema { get; set; }
}
