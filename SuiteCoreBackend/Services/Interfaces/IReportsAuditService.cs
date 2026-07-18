using SuiteCoreBackend.DTOs.ReportsAudit;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IReportsAuditService
    {
        /// <summary>Obtiene el historial de eventos de alertas gestionados por el SCNO Alert Manager.</summary>
        Task<AlertsHistoryDto> GetAlertsHistoryAsync();

        /// <summary>Obtiene el estado de la base de datos de auditoría de automatización.</summary>
        Task<AutomationAuditStatusDto> GetAuditStatusAsync();
    }
}
