namespace SuiteCoreBackend.DTOs.Monitoring;

public class CreateNetboxRegionDto
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
