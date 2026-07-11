namespace SuiteCoreBackend.DTOs.Vpn
{
    /// <summary>
    /// Representa un peer (cliente) conectado a la interfaz WireGuard del servidor VPN.
    /// Los datos son parseados de la salida de <c>sudo wg show</c> vía SSH.
    /// </summary>
    public class WireGuardPeerDto
    {
        /// <summary>Nombre legible del cliente, resuelto desde comentarios en wg0.conf o generado desde la IP.</summary>
        public string ClientName { get; set; } = string.Empty;

        /// <summary>IP asignada al peer dentro de la red VPN (ej. "172.16.40.50").</summary>
        public string VpnIp { get; set; } = string.Empty;

        /// <summary>Texto del último handshake reportado por WireGuard (ej. "1 minute, 5 seconds ago").</summary>
        public string Handshake { get; set; } = string.Empty;

        /// <summary>Bytes recibidos desde este peer (dirección RX del servidor).</summary>
        public long RxBytes { get; set; }

        /// <summary>Bytes enviados hacia este peer (dirección TX del servidor).</summary>
        public long TxBytes { get; set; }

        /// <summary>
        /// Indica si el peer está activo. Se considera online cuando el último handshake
        /// ocurrió hace menos de 3 minutos (intervalo normal de renegociación WireGuard).
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>Última vez visto en formato legible (ej. "active", "5m ago", "2h ago", "3d ago").</summary>
        public string LastSeen { get; set; } = string.Empty;
    }
}
