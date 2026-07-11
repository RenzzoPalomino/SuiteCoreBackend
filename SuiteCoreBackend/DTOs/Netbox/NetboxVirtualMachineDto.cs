namespace SuiteCoreBackend.DTOs.Netbox;

public class NetboxVirtualMachineDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public object? VirtualMachineType { get; set; }
    public NetboxDeviceRoleNestedDto? Role { get; set; }
    public NetboxStatusDto Status { get; set; } = new();
    public NetboxStartOnBootDto? StartOnBoot { get; set; }
    public NetboxSiteNestedDto? Site { get; set; }
    public NetboxClusterNestedDto? Cluster { get; set; }
    public NetboxPrimaryIpDto? PrimaryIp { get; set; }
    public NetboxPrimaryIpDto? PrimaryIp4 { get; set; }
    public double? Vcpus { get; set; }
    public int? Memory { get; set; }
    public int? Disk { get; set; }
    public string? Description { get; set; }
}

public class NetboxStartOnBootDto
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}

public class NetboxClusterNestedDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class NetboxPrimaryIpDto
{
    public int Id { get; set; }
    public NetboxIpFamilyDto Family { get; set; } = new();
    public string Address { get; set; } = string.Empty;
}

public class NetboxIpFamilyDto
{
    public int Value { get; set; }
    public string Label { get; set; } = string.Empty;
}
