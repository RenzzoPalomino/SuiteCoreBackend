using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.DTOs.Netbox;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers;

[ApiController]
[Route("api/netbox")]
public class NetboxController : ControllerBase
{
    private readonly INetboxService _netboxService;

    public NetboxController(INetboxService netboxService)
    {
        _netboxService = netboxService;
    }

    [HttpGet("regions")]
    //[Authorize]
    public async Task<IActionResult> GetRegions()
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

    [HttpGet("regions/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetRegionById(int id)
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

    [HttpPost("regions")]
    //[Authorize]
    public async Task<IActionResult> CreateRegion([FromBody] CreateNetboxRegionDto dto)
    {
        try
        {
            var result = await _netboxService.CreateRegionAsync(dto);
            return CreatedAtAction(nameof(GetRegionById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpPatch("regions/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> UpdateRegion(int id, [FromBody] UpdateNetboxRegionDto dto)
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

    [HttpDelete("regions/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> DeleteRegion(int id)
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

    [HttpGet("ip-addresses")]
    //[Authorize]
    public async Task<IActionResult> GetIpAddresses()
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
