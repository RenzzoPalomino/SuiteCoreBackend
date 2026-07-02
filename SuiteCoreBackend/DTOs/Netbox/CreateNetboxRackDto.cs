using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Netbox;

public class CreateNetboxRackDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("site")]
    public int Site { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("u_height")]
    public int UHeight { get; set; }

    [JsonPropertyName("starting_unit")]
    public int StartingUnit { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
