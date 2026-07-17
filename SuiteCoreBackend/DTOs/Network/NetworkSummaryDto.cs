namespace SuiteCoreBackend.DTOs.Network
{
    /// <summary>Resumen general del estado de la red.</summary>
    public class NetworkSummaryDto
    {
        public string Estado { get; set; } = string.Empty;
        public int Dispositivos { get; set; }
        public int DispositivosActivos { get; set; }
        public int Interfaces { get; set; }
        public int AlertasActivas { get; set; }
    }
}
