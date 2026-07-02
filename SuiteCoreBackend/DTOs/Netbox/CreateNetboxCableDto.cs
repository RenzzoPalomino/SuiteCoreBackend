using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Netbox;

public class CreateNetboxCableDto
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("a_terminations")]
    public List<NetboxTerminationDto> ATerminations { get; set; } = new()
    {
        new() { ObjectType = "dcim.interface", ObjectId = 2 }
    };

    [JsonPropertyName("b_terminations")]
    public List<NetboxTerminationDto> BTerminations { get; set; } = new()
    {
        new() { ObjectType = "dcim.interface", ObjectId = 1 }
    };

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;

    [JsonPropertyName("length")]
    public double? Length { get; set; }

    [JsonPropertyName("length_unit")]
    public string? LengthUnit { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
