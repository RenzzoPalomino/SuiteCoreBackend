using SuiteCoreBackend.DTOs.Vpn;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface ITailscaleService
    {
        /// <summary>
        /// Consulta la API REST de Tailscale (<c>GET /api/v2/tailnet/{tailnet}/devices</c>)
        /// y retorna el estado de conectividad del nodo local junto con la lista de peers
        /// del tailnet, incluyendo hostname, IP mesh, sistema operativo y última vez visto.
        /// </summary>
        Task<TailscaleStatusDto> GetStatusAsync();

        /// <summary>
        /// Retorna la política de acceso VPN estática configurada en la aplicación.
        /// Define qué redes destino son accesibles desde la red VPN WireGuard (172.16.40.0/24).
        /// </summary>
        VpnAccessPolicyDto GetAccessPolicy();
    }
}
