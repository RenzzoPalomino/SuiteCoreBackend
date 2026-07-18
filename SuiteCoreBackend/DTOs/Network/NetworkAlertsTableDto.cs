namespace SuiteCoreBackend.DTOs.Network
{
    /// <summary>Tabla de alertas de red activas.</summary>
    public class NetworkAlertsTableDto
    {
        public string Titulo { get; set; } = string.Empty;
        public List<object> Datos { get; set; } = new();
    }
}
