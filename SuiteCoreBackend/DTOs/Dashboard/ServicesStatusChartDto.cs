namespace SuiteCoreBackend.DTOs.Dashboard
{
    /// <summary>Gráfico de estado de servicios del sistema.</summary>
    public class ServicesStatusChartDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public ServicesStatusDataDto Datos { get; set; } = new();
    }

    /// <summary>Conteo de servicios por estado.</summary>
    public class ServicesStatusDataDto
    {
        public int Operativos { get; set; }
        public int Caidos { get; set; }
    }
}
