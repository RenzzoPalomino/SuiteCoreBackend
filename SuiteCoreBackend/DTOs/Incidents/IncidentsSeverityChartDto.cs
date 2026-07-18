namespace SuiteCoreBackend.DTOs.Incidents
{
    /// <summary>Gráfico de dona con las alertas agrupadas por severidad.</summary>
    public class IncidentsSeverityChartDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public IncidentsSeverityDataDto Datos { get; set; } = new();
    }

    /// <summary>Conteo de alertas por nivel de severidad.</summary>
    public class IncidentsSeverityDataDto
    {
        public int Criticas { get; set; }
        public int Advertencias { get; set; }
        public int Informativas { get; set; }
    }
}
