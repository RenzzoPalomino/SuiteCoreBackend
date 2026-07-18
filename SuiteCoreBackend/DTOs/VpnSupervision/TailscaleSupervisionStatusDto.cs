namespace SuiteCoreBackend.DTOs.VpnSupervision
{
    /// <summary>Estado detallado de Tailscale reportado por el SCNO, incluyendo nodos de la malla.</summary>
    public class TailscaleSupervisionStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public string Health { get; set; } = string.Empty;
        public string CheckedAt { get; set; } = string.Empty;
        public string Component { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public VpnHostDto Host { get; set; } = new();
        public VpnServiceInfoDto Service { get; set; } = new();
        public VpnInterfaceInfoDto Interface { get; set; } = new();
        public string TailscaleIp { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public TailscaleNodesDto Nodes { get; set; } = new();
        public TailscaleRawAvailableDto RawAvailable { get; set; } = new();
    }

    /// <summary>Resumen y listado de nodos de la malla Tailscale.</summary>
    public class TailscaleNodesDto
    {
        public int Total { get; set; }
        public int Online { get; set; }
        public int Offline { get; set; }
        public List<TailscaleNodeDto> Items { get; set; } = new();
    }

    /// <summary>Nodo individual de la malla Tailscale.</summary>
    public class TailscaleNodeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Hostname { get; set; } = string.Empty;
        public string Os { get; set; } = string.Empty;
        public bool Online { get; set; }
        public List<string> TailscaleIps { get; set; } = new();
        public string LastSeen { get; set; } = string.Empty;
        public string Relay { get; set; } = string.Empty;
        public bool Active { get; set; }
    }

    /// <summary>Disponibilidad de datos crudos de estado de Tailscale.</summary>
    public class TailscaleRawAvailableDto
    {
        public bool StatusJson { get; set; }
        public bool StatusText { get; set; }
    }
}
