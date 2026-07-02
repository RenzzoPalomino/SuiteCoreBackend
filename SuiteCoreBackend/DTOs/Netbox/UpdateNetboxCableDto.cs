using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Netbox;

public class UpdateNetboxCableDto
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("a_terminations")]
    public List<NetboxTerminationDto>? ATerminations { get; set; }

    [JsonPropertyName("b_terminations")]
    public List<NetboxTerminationDto>? BTerminations { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("length")]
    public double? Length { get; set; }

    [JsonPropertyName("length_unit")]
    public string? LengthUnit { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
