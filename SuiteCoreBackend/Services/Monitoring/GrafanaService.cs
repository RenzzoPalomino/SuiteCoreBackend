using SuiteCoreBackend.DTOs.Monitoring;
using SuiteCoreBackend.Infrastructure.Context;
using SuiteCoreBackend.Infrastructure.Interfaces;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Services.Monitoring;

public class GrafanaService : IGrafanaService
{
    
    private readonly IConfiguration _config;
    private readonly IGrafanaRepository _grafanaRepository;

    public GrafanaService(IGrafanaRepository grafanarepo, IConfiguration config)
    {
        _grafanaRepository = grafanarepo;
        _config = config;
    }

    public async Task<IEnumerable<GrafanaPanelDto>> GetPanelsAsync()
    {
        var grafanaUrl = _config["Grafana:Url"];

        var panels = await _grafanaRepository.GetAll();

        return panels.Select(p => new GrafanaPanelDto
        {
            Name = p.Name,
            Url = $"{grafanaUrl}/d-solo/{p.DashboardUid}/suite-core-noc-dashboard?orgId=1&panelId={p.PanelId}"
        });
    }
}
