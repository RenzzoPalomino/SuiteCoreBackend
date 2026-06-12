using SuiteCoreBackend.DTOs.Monitoring;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Services.Monitoring;

public class GrafanaService : IGrafanaService
{
    private readonly IConfiguration _config;

    public GrafanaService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IEnumerable<GrafanaPanelDto>> GetPanelsAsync()
    {
        var grafanaUrl = _config["Grafana:Url"];
        var dashboardUid = _config["Grafana:DashboardUid"];

        var panels = new List<GrafanaPanelDto>
        {
            new()
            {
                Name = "Panel WAN",
                PanelId = 2,
                Url =
                    $"{grafanaUrl}/d-solo/{dashboardUid}/suite-core-noc-dashboard?orgId=1&panelId=2"
            },
            new()
            {
                Name = "RAM Firewall",
                PanelId = 1,
                Url =
                    $"{grafanaUrl}/d-solo/{dashboardUid}/suite-core-noc-dashboard?orgId=1&panelId=1"
            }
        };

        return await Task.FromResult(panels);
    }
}
