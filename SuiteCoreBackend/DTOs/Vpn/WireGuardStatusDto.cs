namespace SuiteCoreBackend.DTOs.Vpn
{
    /// <summary>
    /// Estado completo de la interfaz WireGuard en el servidor VPN.
    /// Retornado por GET /api/vpn/wireguard. Incluye configuración de red y lista de peers.
    /// </summary>
    public class WireGuardStatusDto
    {
        /// <summary>Nombre de la interfaz WireGuard en el sistema (ej. "wg0").</summary>
        public string Interface { get; set; } = string.Empty;

        /// <summary>Red VPN asignada al servidor (ej. "172.16.40.0/24").</summary>
        public string Network { get; set; } = string.Empty;

        /// <summary>IP del servidor WireGuard dentro de la red VPN (ej. "172.16.40.1").</summary>
        public string ServerIp { get; set; } = string.Empty;

        /// <summary>Indica si la interfaz WireGuard está activa y respondiendo.</summary>
        public bool IsActive { get; set; }

        /// <summary>Número de peers con handshake reciente (online en los últimos 3 minutos).</summary>
        public int ConnectedPeers { get; set; }

        /// <summary>Lista de todos los peers configurados, con su estado individual.</summary>
        public List<WireGuardPeerDto> Peers { get; set; } = new();
    }
}
