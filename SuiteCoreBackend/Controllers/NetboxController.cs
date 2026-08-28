using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.DTOs.Netbox;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers;

[ApiController]
[Route("api/netbox")]
[Authorize]
public class NetboxController : ControllerBase
{
    private readonly INetboxService _netboxService;

    public NetboxController(INetboxService netboxService)
    {
        _netboxService = netboxService;
    }

    [HttpGet("ip-addresses")]
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

    [HttpGet("vlans")]
    public async Task<IActionResult> GetVlans()
    {
        try
        {
            var result = await _netboxService.GetVlansAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpGet("sites")]
    public async Task<IActionResult> GetSites()
    {
        try
        {
            var result = await _netboxService.GetSitesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpGet("manufacturers")]
    public async Task<IActionResult> GetManufacturers()
    {
        try
        {
            var result = await _netboxService.GetManufacturersAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpGet("device-roles")]
    public async Task<IActionResult> GetDeviceRoles()
    {
        try
        {
            var result = await _netboxService.GetDeviceRolesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpGet("device-types")]
    public async Task<IActionResult> GetDeviceTypes()
    {
        try
        {
            var result = await _netboxService.GetDeviceTypesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpGet("devices")]
    public async Task<IActionResult> GetDevices()
    {
        try
        {
            var result = await _netboxService.GetDevicesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpGet("racks")]
    public async Task<IActionResult> GetRacks()
    {
        try
        {
            var result = await _netboxService.GetRacksAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpGet("virtual-machines")]
    public async Task<IActionResult> GetVirtualMachines()
    {
        try
        {
            var result = await _netboxService.GetVirtualMachinesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpGet("clusters")]
    public async Task<IActionResult> GetClusters()
    {
        try
        {
            var result = await _netboxService.GetClustersAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }
}
