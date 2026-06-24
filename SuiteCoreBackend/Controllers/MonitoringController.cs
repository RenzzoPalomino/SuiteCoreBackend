using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.DTOs.Monitoring;
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

    [HttpGet("netbox-regions/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetNetboxRegionById(int id)
    {
        try
        {
            var result = await _netboxService.GetRegionByIdAsync(id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpPost("netbox-regions")]
    //[Authorize]
    public async Task<IActionResult> CreateNetboxRegion([FromBody] CreateNetboxRegionDto dto)
    {
        try
        {
            var result = await _netboxService.CreateRegionAsync(dto);
            return CreatedAtAction(nameof(GetNetboxRegions), new { name = result.Name }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpPatch("netbox-regions/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> UpdateNetboxRegion(int id, [FromBody] UpdateNetboxRegionDto dto)
    {
        try
        {
            var result = await _netboxService.UpdateRegionAsync(id, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpDelete("netbox-regions/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> DeleteNetboxRegion(int id)
    {
        try
        {
            await _netboxService.DeleteRegionAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpGet("netbox-ip-addresses")]
    //[Authorize]
    public async Task<IActionResult> GetNetboxIpAddresses()
    {
        try
        {
            var result = await _netboxService.GetIpAddressesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }
}
