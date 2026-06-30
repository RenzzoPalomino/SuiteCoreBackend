namespace SuiteCoreBackend.DTOs.Vpn
{
    /// <summary>
    /// Estado general del gateway VPN. Retornado por GET /api/vpn/status.
    /// Refleja la disponibilidad del servidor radius-vpn y su tiempo de actividad.
    /// </summary>
    public class VpnGatewayStatusDto
    {
        /// <summary>Nombre del servidor VPN (ej. "radius-vpn").</summary>
        public string Server { get; set; } = string.Empty;

        /// <summary>IP de gestión del servidor VPN dentro de la red interna (ej. "172.16.20.12").</summary>
        public string ManagementIp { get; set; } = string.Empty;

        /// <summary>Tiempo de actividad del sistema formateado (ej. "12d 4h", "3h 20m").</summary>
        public string Uptime { get; set; } = string.Empty;

        /// <summary>Indica si el servidor VPN está en línea y responde a SSH.</summary>
        public bool IsOnline { get; set; }
    }
}
