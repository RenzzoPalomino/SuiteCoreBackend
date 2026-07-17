namespace SuiteCoreBackend.DTOs.Infrastructure
{
    /// <summary>Resumen general del estado de la infraestructura.</summary>
    public class InfrastructureSummaryDto
    {
        public string Estado { get; set; } = string.Empty;
        public int NodosActivos { get; set; }
        public int MaquinasVirtuales { get; set; }
        public double MemoriaUso { get; set; }
        public double AlmacenamientoUso { get; set; }
    }
}
