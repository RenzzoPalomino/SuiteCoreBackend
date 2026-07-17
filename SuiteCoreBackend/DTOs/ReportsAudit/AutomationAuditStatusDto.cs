namespace SuiteCoreBackend.DTOs.ReportsAudit
{
    /// <summary>Estado de la base de datos de auditoría de automatización del SCNO.</summary>
    public class AutomationAuditStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string Integrity { get; set; } = string.Empty;
        public string JournalMode { get; set; } = string.Empty;
        public int ExecutionCount { get; set; }
    }
}
