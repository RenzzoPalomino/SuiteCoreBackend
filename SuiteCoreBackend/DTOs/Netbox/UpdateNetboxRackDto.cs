using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Netbox;

public class UpdateNetboxRackDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("site")]
    public int? Site { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("width")]
    public int? Width { get; set; }

    [JsonPropertyName("u_height")]
    public int? UHeight { get; set; }

    [JsonPropertyName("starting_unit")]
    public int? StartingUnit { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
