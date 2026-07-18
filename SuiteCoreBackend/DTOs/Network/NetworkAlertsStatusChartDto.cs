namespace SuiteCoreBackend.DTOs.Network
{
    /// <summary>Gráfico de barras con el conteo de alertas de red por severidad.</summary>
    public class NetworkAlertsStatusChartDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public NetworkAlertsStatusDataDto Datos { get; set; } = new();
    }

    /// <summary>Conteo de alertas de red por severidad.</summary>
    public class NetworkAlertsStatusDataDto
    {
        public int Criticas { get; set; }
        public int Advertencias { get; set; }
    }
}
