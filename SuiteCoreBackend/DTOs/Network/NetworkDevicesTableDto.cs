namespace SuiteCoreBackend.DTOs.Network
{
    /// <summary>Tabla de dispositivos de red monitoreados.</summary>
    public class NetworkDevicesTableDto
    {
        public string Titulo { get; set; } = string.Empty;
        public List<NetworkDeviceDto> Datos { get; set; } = new();
    }

    /// <summary>Dispositivo de red monitoreado por LibreNMS.</summary>
    public class NetworkDeviceDto
    {
        public int DeviceId { get; set; }
        public string Hostname { get; set; } = string.Empty;
        public string Display { get; set; } = string.Empty;
        public string SysName { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public string Os { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Status { get; set; }
        public string StatusLabel { get; set; } = string.Empty;
        public long Uptime { get; set; }
        public string LastPolled { get; set; } = string.Empty;
        public string LastPing { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
