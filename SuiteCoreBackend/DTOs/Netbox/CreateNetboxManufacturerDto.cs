using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Netbox;

public class CreateNetboxManufacturerDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
