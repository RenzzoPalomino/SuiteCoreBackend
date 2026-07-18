namespace SuiteCoreBackend.DTOs.Network
{
    /// <summary>Gráfico de dona con el estado de las interfaces de red.</summary>
    public class NetworkInterfacesStatusChartDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public NetworkInterfacesStatusDataDto Datos { get; set; } = new();
    }

    /// <summary>Conteo de interfaces de red por estado.</summary>
    public class NetworkInterfacesStatusDataDto
    {
        public int Activas { get; set; }
        public int Inactivas { get; set; }
    }
}
