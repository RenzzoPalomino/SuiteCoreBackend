namespace SuiteCoreBackend.DTOs.Netbox;

public class NetboxDeviceRoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public bool VmRole { get; set; }
    public string Description { get; set; } = string.Empty;
    public int DeviceCount { get; set; }
    public int VirtualMachineCount { get; set; }
}
