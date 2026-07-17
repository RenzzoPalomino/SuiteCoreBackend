namespace SuiteCoreBackend.DTOs.Network
{
    /// <summary>Tabla de interfaces de red monitoreadas.</summary>
    public class NetworkInterfacesTableDto
    {
        public string Titulo { get; set; } = string.Empty;
        public List<NetworkInterfaceDto> Datos { get; set; } = new();
    }

    /// <summary>Interfaz de red de un dispositivo monitoreado por LibreNMS.</summary>
    public class NetworkInterfaceDto
    {
        public int PortId { get; set; }
        public string IfName { get; set; } = string.Empty;
        public string? IfDescr { get; set; }
        public string? IfAlias { get; set; }
        public string? IfOperStatus { get; set; }
        public string? IfAdminStatus { get; set; }
        public int? DeviceId { get; set; }
    }
}
