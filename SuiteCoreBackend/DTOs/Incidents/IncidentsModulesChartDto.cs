namespace SuiteCoreBackend.DTOs.Incidents
{
    /// <summary>Gráfico de barras con las alertas agrupadas por módulo del sistema.</summary>
    public class IncidentsModulesChartDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;

        /// <summary>Conteo de alertas por módulo. Clave = nombre del módulo, valor = cantidad de alertas.</summary>
        public Dictionary<string, int> Datos { get; set; } = new();
    }
}
