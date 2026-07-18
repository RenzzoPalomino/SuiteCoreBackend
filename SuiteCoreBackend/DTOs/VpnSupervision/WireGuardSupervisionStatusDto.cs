namespace SuiteCoreBackend.DTOs.VpnSupervision
{
    /// <summary>Estado detallado de WireGuard reportado por el SCNO, incluyendo peers y prueba de conectividad.</summary>
    public class WireGuardSupervisionStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public string Health { get; set; } = string.Empty;
        public string CheckedAt { get; set; } = string.Empty;
        public string Component { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public VpnHostDto Host { get; set; } = new();
        public VpnServiceInfoDto Service { get; set; } = new();
        public VpnInterfaceInfoDto Interface { get; set; } = new();
        public WireGuardDetailDto WireGuard { get; set; } = new();
        public WireGuardConnectivityTestDto ConnectivityTest { get; set; } = new();
    }

    /// <summary>Detalle de la interfaz WireGuard, peers activos y datos crudos del dump.</summary>
    public class WireGuardDetailDto
    {
        public WireGuardInterfaceInfoDto Interface { get; set; } = new();
        public int PeersTotal { get; set; }
        public List<WireGuardPeerInfoDto> Peers { get; set; } = new();
        public WireGuardDumpDto Dump { get; set; } = new();
    }

    /// <summary>Configuración de la interfaz WireGuard.</summary>
    public class WireGuardInterfaceInfoDto
    {
        public string Name { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;
        public string PrivateKey { get; set; } = string.Empty;
        public string ListeningPort { get; set; } = string.Empty;
    }

    /// <summary>Peer WireGuard con métricas de transferencia legibles.</summary>
    public class WireGuardPeerInfoDto
    {
        public string PublicKey { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string AllowedIps { get; set; } = string.Empty;
        public string LatestHandshake { get; set; } = string.Empty;
        public string Transfer { get; set; } = string.Empty;
        public string Rx { get; set; } = string.Empty;
        public string Tx { get; set; } = string.Empty;
    }

    /// <summary>Datos crudos equivalentes a la salida de `wg show dump`.</summary>
    public class WireGuardDumpDto
    {
        public WireGuardInterfaceDumpDto InterfaceDump { get; set; } = new();
        public List<WireGuardPeerDumpDto> PeersDump { get; set; } = new();
    }

    /// <summary>Dump crudo de la configuración de la interfaz WireGuard.</summary>
    public class WireGuardInterfaceDumpDto
    {
        public string PrivateKey { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;
        public string ListenPort { get; set; } = string.Empty;
        public string Fwmark { get; set; } = string.Empty;
    }

    /// <summary>Dump crudo de un peer WireGuard.</summary>
    public class WireGuardPeerDumpDto
    {
        public string PublicKey { get; set; } = string.Empty;
        public string PresharedKey { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string AllowedIps { get; set; } = string.Empty;
        public long LatestHandshakeEpoch { get; set; }
        public long TransferRxBytes { get; set; }
        public long TransferTxBytes { get; set; }
        public string PersistentKeepalive { get; set; } = string.Empty;
    }

    /// <summary>Resultado de la prueba de conectividad (ping) a través del túnel WireGuard.</summary>
    public class WireGuardConnectivityTestDto
    {
        public string Target { get; set; } = string.Empty;
        public bool PingOk { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
