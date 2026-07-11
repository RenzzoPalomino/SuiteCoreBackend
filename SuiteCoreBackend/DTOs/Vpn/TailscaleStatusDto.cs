namespace SuiteCoreBackend.DTOs.Vpn
{
    /// <summary>
    /// Estado de conectividad Tailscale del nodo servidor (security-core).
    /// Retornado por GET /api/vpn/tailscale. Incluye la lista de todos los peers del tailnet.
    /// </summary>
    public class TailscaleStatusDto
    {
        /// <summary>Indica si el nodo local está conectado al control plane de Tailscale.</summary>
        public bool IsConnected { get; set; }

        /// <summary>IP mesh asignada al nodo servidor dentro del tailnet (rango 100.x.x.x).</summary>
        public string MeshIp { get; set; } = string.Empty;

        /// <summary>Hostname del nodo local tal como aparece en Tailscale (ej. "security-core").</summary>
        public string NodeName { get; set; } = string.Empty;

        /// <summary>
        /// Indica si hay conexión activa a través de un relay DERP de Tailscale.
        /// True cuando al menos un peer está conectado por relay.
        /// </summary>
        public bool DerpActive { get; set; }

        /// <summary>Lista de todos los dispositivos del tailnet, ordenados online primero.</summary>
        public List<TailscalePeerDto> Peers { get; set; } = new();
    }
}
