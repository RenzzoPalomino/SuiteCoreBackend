using SuiteCoreBackend.DTOs.Network;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface INetworkService
    {
        /// <summary>Obtiene el resumen general del estado de la red.</summary>
        Task<NetworkSummaryDto> GetSummaryAsync();

        /// <summary>Obtiene el gráfico de alertas de red por severidad.</summary>
        Task<NetworkAlertsStatusChartDto> GetAlertsStatusChartAsync();

        /// <summary>Obtiene el gráfico de estado de los dispositivos de red.</summary>
        Task<NetworkDevicesStatusChartDto> GetDevicesStatusChartAsync();

        /// <summary>Obtiene el gráfico de estado de las interfaces de red.</summary>
        Task<NetworkInterfacesStatusChartDto> GetInterfacesStatusChartAsync();

        /// <summary>Obtiene la tabla de alertas de red activas.</summary>
        Task<NetworkAlertsTableDto> GetAlertsTableAsync();

        /// <summary>Obtiene la tabla de dispositivos de red monitoreados.</summary>
        Task<NetworkDevicesTableDto> GetDevicesTableAsync();

        /// <summary>Obtiene la tabla de interfaces de red monitoreadas.</summary>
        Task<NetworkInterfacesTableDto> GetInterfacesTableAsync();
    }
}
