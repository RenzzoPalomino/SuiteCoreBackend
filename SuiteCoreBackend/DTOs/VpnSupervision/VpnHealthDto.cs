namespace SuiteCoreBackend.DTOs.VpnSupervision
{
    /// <summary>Estado de salud agregado de los componentes VPN gestionados por el SCNO.</summary>
    public class VpnHealthDto
    {
        public string Status { get; set; } = string.Empty;
        public VpnHealthDetailDto Health { get; set; } = new();
    }

    /// <summary>Detalle del estado de salud de los componentes VPN.</summary>
    public class VpnHealthDetailDto
    {
        public string Status { get; set; } = string.Empty;
        public int ComponentsTotal { get; set; }
        public int Healthy { get; set; }
        public int Warning { get; set; }
        public int Critical { get; set; }
        public List<VpnHealthComponentDto> Components { get; set; } = new();
    }

    /// <summary>Estado de salud de un componente VPN individual (Tailscale, WireGuard).</summary>
    public class VpnHealthComponentDto
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}
