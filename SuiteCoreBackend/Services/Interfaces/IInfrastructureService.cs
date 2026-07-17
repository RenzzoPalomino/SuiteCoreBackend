using SuiteCoreBackend.DTOs.Infrastructure;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IInfrastructureService
    {
        /// <summary>Obtiene el resumen general del estado de la infraestructura.</summary>
        Task<InfrastructureSummaryDto> GetSummaryAsync();

        /// <summary>Obtiene el gráfico de uso de recursos (CPU, memoria, almacenamiento).</summary>
        Task<ResourcesUsageChartDto> GetResourcesUsageChartAsync();
    }
}
