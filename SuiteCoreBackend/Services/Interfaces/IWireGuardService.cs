using SuiteCoreBackend.DTOs.Vpn;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IWireGuardService
    {
        /// <summary>
        /// Conecta al servidor VPN via SSH y ejecuta <c>sudo wg show</c> para obtener
        /// el estado de la interfaz WireGuard y la lista de peers activos.
        /// Parsea handshake, bytes transferidos y determina si el peer está online
        /// (último handshake menor a 3 minutos).
        /// </summary>
        Task<WireGuardStatusDto> GetStatusAsync();

        /// <summary>
        /// Conecta al servidor VPN via SSH y ejecuta <c>cat /proc/uptime</c> para
        /// obtener el tiempo de actividad del sistema, junto con los datos estáticos
        /// del gateway (servidor, IP de gestión).
        /// </summary>
        Task<VpnGatewayStatusDto> GetGatewayStatusAsync();

        /// <summary>
        /// Lee <c>/proc/net/dev</c> via SSH para obtener las métricas acumulativas de
        /// tráfico de la interfaz WireGuard: bytes, paquetes y errores de RX/TX.
        /// No requiere sudo. Incluye timestamp UTC para que el frontend calcule
        /// throughput en Mbps comparando dos snapshots consecutivos.
        /// </summary>
        Task<WireGuardStatsDto> GetStatsAsync();
    }
}
