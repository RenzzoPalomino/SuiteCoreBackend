namespace SuiteCoreBackend.DTOs.Netbox;

public class NetboxDeviceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public NetboxDeviceTypeDto DeviceType { get; set; } = new();
    public NetboxDeviceRoleNestedDto Role { get; set; } = new();
    public NetboxSiteNestedDto Site { get; set; } = new();
    public NetboxRackNestedDto? Rack { get; set; }
    public NetboxStatusDto Status { get; set; } = new();
}

public class NetboxDeviceTypeDto
{
    public int Id { get; set; }
    public NetboxManufacturerNestedDto Manufacturer { get; set; } = new();
    public string Model { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int UHeight { get; set; }
    public NetboxAirflowDto? Airflow { get; set; }
    public double? Weight { get; set; }
    public NetboxWeightUnitDto? WeightUnit { get; set; }
    public string? Description { get; set; }
    public int DeviceCount { get; set; }
}

public class NetboxAirflowDto
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}

public class NetboxWeightUnitDto
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}

public class NetboxManufacturerNestedDto
{
    public int Id { get; set; }
    public string Slug { get; set; } = string.Empty;
}

public class NetboxDeviceRoleNestedDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class NetboxSiteNestedDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class NetboxRackNestedDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
