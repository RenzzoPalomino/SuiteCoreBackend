using System.Text.Json.Serialization;

namespace SuiteCoreBackend.Models.Entities;

public class NetboxResponse
{
    [JsonPropertyName("results")]
    public List<NetboxRegionResult> Results { get; set; } = new();
}

public class NetboxRegionResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("site_count")]
    public int SiteCount { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

public class NetboxIpResponse
{
    [JsonPropertyName("results")]
    public List<NetboxIpAddressResult> Results { get; set; } = new();
}

public class NetboxIpAddressResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public NetboxStatusResult Status { get; set; } = new();

    [JsonPropertyName("dns_name")]
    public string DnsName { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

public class NetboxStatusResult
{
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;
}

public class NetboxVlanResponse
{
    [JsonPropertyName("results")]
    public List<NetboxVlanResult> Results { get; set; } = new();
}

public class NetboxVlanResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("vid")]
    public int Vid { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public NetboxStatusResult Status { get; set; } = new();

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

public class NetboxCableResponse
{
    [JsonPropertyName("results")]
    public List<NetboxCableResult> Results { get; set; } = new();
}

public class NetboxCableResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("a_terminations")]
    public List<NetboxTerminationResult> ATerminations { get; set; } = new();

    [JsonPropertyName("b_terminations")]
    public List<NetboxTerminationResult> BTerminations { get; set; } = new();

    [JsonPropertyName("status")]
    public NetboxStatusResult Status { get; set; } = new();

    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;

    [JsonPropertyName("length")]
    public double? Length { get; set; }

    [JsonPropertyName("length_unit")]
    public NetboxLengthUnitResult? LengthUnit { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

public class NetboxTerminationResult
{
    [JsonPropertyName("object_type")]
    public string ObjectType { get; set; } = string.Empty;

    [JsonPropertyName("object_id")]
    public int ObjectId { get; set; }
}

public class NetboxLengthUnitResult
{
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;
}

public class NetboxSiteResponse
{
    [JsonPropertyName("results")]
    public List<NetboxSiteResult> Results { get; set; } = new();
}

public class NetboxSiteResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public NetboxStatusResult Status { get; set; } = new();

    [JsonPropertyName("facility")]
    public string Facility { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

public class NetboxModuleTypeProfileResponse
{
    [JsonPropertyName("results")]
    public List<NetboxModuleTypeProfileResult> Results { get; set; } = new();
}

public class NetboxModuleTypeProfileResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("schema")]
    public object? Schema { get; set; }
}

public class NetboxManufacturerResponse
{
    [JsonPropertyName("results")]
    public List<NetboxManufacturerResult> Results { get; set; } = new();
}

public class NetboxManufacturerResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

public class NetboxDeviceRoleResponse
{
    [JsonPropertyName("results")]
    public List<NetboxDeviceRoleResult> Results { get; set; } = new();
}

public class NetboxDeviceRoleResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;

    [JsonPropertyName("vm_role")]
    public bool VmRole { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("device_count")]
    public int DeviceCount { get; set; }

    [JsonPropertyName("virtualmachine_count")]
    public int VirtualMachineCount { get; set; }
}

public class NetboxDeviceResponse
{
    [JsonPropertyName("results")]
    public List<NetboxDeviceResult> Results { get; set; } = new();
}

public class NetboxDeviceResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("device_type")]
    public NetboxDeviceTypeResult DeviceType { get; set; } = new();

    [JsonPropertyName("role")]
    public NetboxDeviceRoleNestedResult Role { get; set; } = new();

    [JsonPropertyName("site")]
    public NetboxSiteNestedResult Site { get; set; } = new();

    [JsonPropertyName("rack")]
    public NetboxRackNestedResult? Rack { get; set; }

    [JsonPropertyName("status")]
    public NetboxStatusResult Status { get; set; } = new();
}

public class NetboxDeviceTypeResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("manufacturer")]
    public NetboxManufacturerNestedResult Manufacturer { get; set; } = new();

    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;
}

public class NetboxManufacturerNestedResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;
}

public class NetboxDeviceRoleNestedResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class NetboxSiteNestedResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class NetboxRackNestedResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class NetboxRackResponse
{
    [JsonPropertyName("results")]
    public List<NetboxRackResult> Results { get; set; } = new();
}

public class NetboxRackResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("site")]
    public NetboxSiteNestedResult Site { get; set; } = new();

    [JsonPropertyName("status")]
    public NetboxStatusResult Status { get; set; } = new();

    [JsonPropertyName("width")]
    public NetboxRackWidthResult Width { get; set; } = new();

    [JsonPropertyName("u_height")]
    public int UHeight { get; set; }

    [JsonPropertyName("starting_unit")]
    public int StartingUnit { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("device_count")]
    public int DeviceCount { get; set; }
}

public class NetboxRackWidthResult
{
    [JsonPropertyName("value")]
    public int Value { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;
}
