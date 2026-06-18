using Microsoft.EntityFrameworkCore;
using SuiteCoreBackend.DTOs.Monitoring;
using SuiteCoreBackend.Infraestucture.Context;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Services.Monitoring;

public class GrafanaService : IGrafanaService
{
    private readonly SCDbContext _context;
    private readonly IConfiguration _config;

    public GrafanaService(SCDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<IEnumerable<GrafanaPanelDto>> GetPanelsAsync()
    {
        var grafanaUrl = _config["Grafana:Url"];

        var panels = await _context.GrafanaPanels.ToListAsync();

        return panels.Select(p => new GrafanaPanelDto
        {
            Name = p.Name,
            Url = $"{grafanaUrl}/d-solo/{p.DashboardUid}/suite-core-noc-dashboard?orgId=1&panelId={p.PanelId}"
        });
    }
}
