namespace SuiteCoreBackend.DTOs.Infrastructure
{
    /// <summary>Gráfico de uso de recursos de la infraestructura.</summary>
    public class ResourcesUsageChartDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public ResourcesUsageDataDto Datos { get; set; } = new();
    }

    /// <summary>Porcentaje de uso por tipo de recurso.</summary>
    public class ResourcesUsageDataDto
    {
        public double Cpu { get; set; }
        public double Memoria { get; set; }
        public double Almacenamiento { get; set; }
    }
}
