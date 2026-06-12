namespace SuiteCoreBackend.DTOs.Monitoring;

public class GrafanaPanelDto
{
    public string Name { get; set; } = string.Empty;

    public int PanelId { get; set; }

    public string Url { get; set; } = string.Empty;
}
