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
    private readonly INetboxService _netboxService;

    public MonitoringController(ILibreNmsService service, IGrafanaService grafanaService, INetboxService netboxService)
    {
        _service = service;
        _grafanaService = grafanaService;
        _netboxService = netboxService;
    }

    [HttpGet("device-types")]
    //[Authorize]
    public async Task<IActionResult> GetDeviceTypes()
    {
        var result = await _service.GetDeviceTypesAsync();
        return Ok(result);
    }

    [HttpGet("grafana-panels")]
    //[Authorize]
    public async Task<IActionResult> GetGrafanaPanels()
    {
        try
        {
            var result = await _grafanaService.GetPanelsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpGet("netbox-regions")]
    //[Authorize]
    public async Task<IActionResult> GetNetboxRegions()
    {
        try
        {
            var result = await _netboxService.GetRegionsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }
}
