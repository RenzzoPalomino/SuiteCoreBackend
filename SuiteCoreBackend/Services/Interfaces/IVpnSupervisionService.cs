using SuiteCoreBackend.DTOs.VpnSupervision;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IVpnSupervisionService
    {
        /// <summary>Obtiene el estado de salud agregado de los componentes VPN (Tailscale, WireGuard).</summary>
        Task<VpnHealthDto> GetHealthAsync();

        /// <summary>Obtiene el estado detallado de Tailscale y los nodos de la malla.</summary>
        Task<TailscaleSupervisionStatusDto> GetTailscaleStatusAsync();

        /// <summary>Obtiene el estado detallado de WireGuard, sus peers y la prueba de conectividad.</summary>
        Task<WireGuardSupervisionStatusDto> GetWireGuardStatusAsync();
    }
}
