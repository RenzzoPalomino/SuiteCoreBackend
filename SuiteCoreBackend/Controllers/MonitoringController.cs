using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.DTOs.Monitoring;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers;

[ApiController]
[Route("api/monitoring")]
public class MonitoringController : ControllerBase
{
    private readonly ILibreNmsService _service;
    private readonly IGrafanaService _grafanaService;
    private readonly IGrafanaEmbedService _grafanaEmbedService;

    public MonitoringController(ILibreNmsService service, IGrafanaService grafanaService, IGrafanaEmbedService grafanaEmbedService)
    {
        _service = service;
        _grafanaService = grafanaService;
        _grafanaEmbedService = grafanaEmbedService;
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

    [HttpGet("grafana-embed-links")]
    //[Authorize]
    public async Task<IActionResult> GetGrafanaEmbedLinks()
    {
        try
        {
            var result = await _grafanaEmbedService.GetEmbedLinksAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }
}
