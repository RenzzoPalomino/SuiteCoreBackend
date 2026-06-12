using SuiteCoreBackend.DTOs.Monitoring;

namespace SuiteCoreBackend.Services.Interfaces;

public interface IGrafanaService
{
    Task<IEnumerable<GrafanaPanelDto>> GetPanelsAsync();
}
