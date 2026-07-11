using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Netbox;

public class CreateNetboxDeviceRoleDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("vm_role")]
    public bool? VmRole { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
