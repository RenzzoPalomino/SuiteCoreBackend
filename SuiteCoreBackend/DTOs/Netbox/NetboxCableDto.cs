using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Netbox;

public class NetboxCableDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public List<NetboxTerminationDto> ATerminations { get; set; } = new();
    public List<NetboxTerminationDto> BTerminations { get; set; } = new();
    public NetboxStatusDto Status { get; set; } = new();
    public string Color { get; set; } = string.Empty;
    public double? Length { get; set; }
    public NetboxLengthUnitDto? LengthUnit { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class NetboxTerminationDto
{
    [JsonPropertyName("object_type")]
    public string ObjectType { get; set; } = string.Empty;

    [JsonPropertyName("object_id")]
    public int ObjectId { get; set; }
}

public class NetboxLengthUnitDto
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}
