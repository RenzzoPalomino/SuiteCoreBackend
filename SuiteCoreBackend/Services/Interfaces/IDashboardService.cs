using SuiteCoreBackend.DTOs.Dashboard;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IDashboardService
    {
        /// <summary>Obtiene el estado general del sistema y sus indicadores clave.</summary>
        Task<DashboardSummaryDto> GetSummaryAsync();

        /// <summary>Obtiene el gráfico de estado de módulos del sistema.</summary>
        Task<ModulesStatusChartDto> GetModulesStatusChartAsync();

        /// <summary>Obtiene el gráfico de estado de servicios del sistema.</summary>
        Task<ServicesStatusChartDto> GetServicesStatusChartAsync();
    }
}
