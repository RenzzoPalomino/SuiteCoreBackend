namespace SuiteCoreBackend.DTOs.Incidents
{
    /// <summary>Resumen general de alertas e incidentes.</summary>
    public class IncidentsSummaryDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public IncidentsIndicadoresDto Indicadores { get; set; } = new();
    }

    /// <summary>Indicadores numéricos de alertas e incidentes.</summary>
    public class IncidentsIndicadoresDto
    {
        public int AlertasActivas { get; set; }
        public int IncidentesHistoricos { get; set; }
        public IncidentsNotificacionesDto Notificaciones { get; set; } = new();
    }

    /// <summary>Estado de los canales de notificación de alertas.</summary>
    public class IncidentsNotificacionesDto
    {
        public bool Telegram { get; set; }
        public bool Correo { get; set; }
    }
}
