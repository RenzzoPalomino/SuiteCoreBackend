namespace SuiteCoreBackend.DTOs.Network
{
    /// <summary>Gráfico de dona con el estado de los dispositivos de red.</summary>
    public class NetworkDevicesStatusChartDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public NetworkDevicesStatusDataDto Datos { get; set; } = new();
    }

    /// <summary>Conteo de dispositivos de red por estado.</summary>
    public class NetworkDevicesStatusDataDto
    {
        public int Activos { get; set; }
        public int Caidos { get; set; }
    }
}
