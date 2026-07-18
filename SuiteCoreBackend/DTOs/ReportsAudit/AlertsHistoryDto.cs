namespace SuiteCoreBackend.DTOs.ReportsAudit
{
    /// <summary>Historial de eventos de alertas gestionados por el SCNO Alert Manager.</summary>
    public class AlertsHistoryDto
    {
        public bool Success { get; set; }
        public string Service { get; set; } = string.Empty;
        public int Count { get; set; }
        public int Limit { get; set; }
        public List<AlertHistoryEventDto> Events { get; set; } = new();
    }

    /// <summary>Evento individual del historial de alertas.</summary>
    public class AlertHistoryEventDto
    {
        public string Timestamp { get; set; } = string.Empty;
        public string Service { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Event { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public string Elapsed { get; set; } = string.Empty;
        public AlertConfirmationDto Confirmation { get; set; } = new();
        public List<AlertNotificationResultDto> Notifications { get; set; } = new();
    }

    /// <summary>Progreso de confirmación del evento (observaciones requeridas vs. registradas).</summary>
    public class AlertConfirmationDto
    {
        public int Observations { get; set; }
        public int Required { get; set; }
    }

    /// <summary>Resultado del envío de notificación por canal para un evento de alerta.</summary>
    public class AlertNotificationResultDto
    {
        public string Channel { get; set; } = string.Empty;
        public bool Ok { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
