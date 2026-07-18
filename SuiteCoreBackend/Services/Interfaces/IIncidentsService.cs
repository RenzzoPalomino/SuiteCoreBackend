using SuiteCoreBackend.DTOs.Incidents;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IIncidentsService
    {
        /// <summary>Obtiene el resumen general de alertas e incidentes.</summary>
        Task<IncidentsSummaryDto> GetSummaryAsync();

        /// <summary>Obtiene el gráfico de alertas agrupadas por severidad.</summary>
        Task<IncidentsSeverityChartDto> GetSeverityChartAsync();

        /// <summary>Obtiene el gráfico de alertas agrupadas por módulo del sistema.</summary>
        Task<IncidentsModulesChartDto> GetModulesChartAsync();

        /// <summary>Obtiene los eventos recientes registrados en Graylog.</summary>
        Task<GraylogEventsResultDto> GetEventsAsync();

        /// <summary>Obtiene los eventos de seguridad recientes registrados en Graylog.</summary>
        Task<GraylogEventsResultDto> GetSecurityEventsAsync();
    }
}
