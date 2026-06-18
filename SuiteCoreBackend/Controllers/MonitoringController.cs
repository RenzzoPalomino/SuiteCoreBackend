using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.Services.Interfaces;
using SuiteCoreBackend.Services.Monitoring;

namespace SuiteCoreBackend.Controllers;

[ApiController]
[Route("api/monitoring")]
public class MonitoringController : ControllerBase
{
    private readonly ILibreNmsService _service;
    private readonly IGrafanaService _grafanaService;

    public MonitoringController(ILibreNmsService service, IGrafanaService grafanaService)
    {
        _service = service;
        _grafanaService = grafanaService;
    }

    [HttpGet("device-types")]
    //[Authorize]
    public async Task<IActionResult> GetDeviceTypes()
    {
        var result = await _service.GetDeviceTypesAsync();
        return Ok(result);
    }

    [HttpGet("grafana-panels")]
    public async Task<IActionResult> GetGrafanaPanels()
    {
        var result = await _grafanaService.GetPanelsAsync();
        return Ok(result);
    }
}
