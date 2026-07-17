namespace SuiteCoreBackend.DTOs.Dashboard
{
    /// <summary>Gráfico de estado de módulos del sistema.</summary>
    public class ModulesStatusChartDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public ModulesStatusDataDto Datos { get; set; } = new();
    }

    /// <summary>Conteo de módulos por estado.</summary>
    public class ModulesStatusDataDto
    {
        public int Operativos { get; set; }
        public int Advertencia { get; set; }
        public int Criticos { get; set; }
    }
}
