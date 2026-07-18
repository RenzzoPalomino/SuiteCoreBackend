namespace SuiteCoreBackend.DTOs.Dashboard
{
    /// <summary>Estado general del sistema con sus indicadores clave.</summary>
    public class DashboardSummaryDto
    {
        public string Estado { get; set; } = string.Empty;
        public DashboardIndicadoresDto Indicadores { get; set; } = new();
    }

    /// <summary>Indicadores numéricos del estado general del sistema.</summary>
    public class DashboardIndicadoresDto
    {
        public int AlertasActivas { get; set; }
        public int ChecksTotales { get; set; }
        public int ChecksActivos { get; set; }
        public int MaquinasVirtuales { get; set; }
        public int Servicios { get; set; }
    }
}
