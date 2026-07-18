namespace SuiteCoreBackend.DTOs.VpnSupervision
{
    /// <summary>Host donde corre el componente VPN (compartido por Tailscale y WireGuard).</summary>
    public class VpnHostDto
    {
        public string Hostname { get; set; } = string.Empty;
        public string ManagementIp { get; set; } = string.Empty;
    }

    /// <summary>Estado del servicio systemd asociado al componente VPN.</summary>
    public class VpnServiceInfoDto
    {
        public string Name { get; set; } = string.Empty;
        public bool Active { get; set; }
        public string State { get; set; } = string.Empty;
    }

    /// <summary>Estado de la interfaz de red asociada al componente VPN.</summary>
    public class VpnInterfaceInfoDto
    {
        public string Name { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public List<string> Addresses { get; set; } = new();
    }
}
