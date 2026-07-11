namespace SuiteCoreBackend.DTOs.Netbox;

public class NetboxClusterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public NetboxClusterTypeNestedDto Type { get; set; } = new();
    public NetboxStatusDto Status { get; set; } = new();
    public string Description { get; set; } = string.Empty;
    public int DeviceCount { get; set; }
    public int VirtualMachineCount { get; set; }
    public double? AllocatedVcpus { get; set; }
    public long? AllocatedMemory { get; set; }
    public long? AllocatedDisk { get; set; }
}

public class NetboxClusterTypeNestedDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
