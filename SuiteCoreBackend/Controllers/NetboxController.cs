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

    [HttpGet("ip-addresses/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetIpAddressById(int id)
    {
        try
        {
            var result = await _netboxService.GetIpAddressByIdAsync(id);
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

    [HttpPost("ip-addresses")]
    //[Authorize]
    public async Task<IActionResult> CreateIpAddress([FromBody] CreateNetboxIpAddressDto dto)
    {
        try
        {
            var result = await _netboxService.CreateIpAddressAsync(dto);
            return CreatedAtAction(nameof(GetIpAddressById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpPatch("ip-addresses/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> UpdateIpAddress(int id, [FromBody] UpdateNetboxIpAddressDto dto)
    {
        try
        {
            var result = await _netboxService.UpdateIpAddressAsync(id, dto);
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

    [HttpDelete("ip-addresses/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> DeleteIpAddress(int id)
    {
        try
        {
            await _netboxService.DeleteIpAddressAsync(id);
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

    [HttpGet("vlans")]
    //[Authorize]
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

    [HttpGet("vlans/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetVlanById(int id)
    {
        try
        {
            var result = await _netboxService.GetVlanByIdAsync(id);
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

    [HttpPost("vlans")]
    //[Authorize]
    public async Task<IActionResult> CreateVlan([FromBody] CreateNetboxVlanDto dto)
    {
        try
        {
            var result = await _netboxService.CreateVlanAsync(dto);
            return CreatedAtAction(nameof(GetVlanById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpPatch("vlans/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> UpdateVlan(int id, [FromBody] UpdateNetboxVlanDto dto)
    {
        try
        {
            var result = await _netboxService.UpdateVlanAsync(id, dto);
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

    [HttpDelete("vlans/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> DeleteVlan(int id)
    {
        try
        {
            await _netboxService.DeleteVlanAsync(id);
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

    [HttpGet("cables")]
    //[Authorize]
    public async Task<IActionResult> GetCables()
    {
        try
        {
            var result = await _netboxService.GetCablesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpGet("cables/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetCableById(int id)
    {
        try
        {
            var result = await _netboxService.GetCableByIdAsync(id);
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

    [HttpPost("cables")]
    //[Authorize]
    public async Task<IActionResult> CreateCable([FromBody] CreateNetboxCableDto dto)
    {
        try
        {
            var result = await _netboxService.CreateCableAsync(dto);
            return CreatedAtAction(nameof(GetCableById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpPatch("cables/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> UpdateCable(int id, [FromBody] UpdateNetboxCableDto dto)
    {
        try
        {
            var result = await _netboxService.UpdateCableAsync(id, dto);
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

    [HttpDelete("cables/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> DeleteCable(int id)
    {
        try
        {
            await _netboxService.DeleteCableAsync(id);
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

    [HttpGet("sites")]
    //[Authorize]
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

    [HttpGet("sites/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetSiteById(int id)
    {
        try
        {
            var result = await _netboxService.GetSiteByIdAsync(id);
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

    [HttpPost("sites")]
    //[Authorize]
    public async Task<IActionResult> CreateSite([FromBody] CreateNetboxSiteDto dto)
    {
        try
        {
            var result = await _netboxService.CreateSiteAsync(dto);
            return CreatedAtAction(nameof(GetSiteById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpPatch("sites/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> UpdateSite(int id, [FromBody] UpdateNetboxSiteDto dto)
    {
        try
        {
            var result = await _netboxService.UpdateSiteAsync(id, dto);
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

    [HttpDelete("sites/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> DeleteSite(int id)
    {
        try
        {
            await _netboxService.DeleteSiteAsync(id);
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

    [HttpGet("module-type-profiles")]
    //[Authorize]
    public async Task<IActionResult> GetModuleTypeProfiles()
    {
        try
        {
            var result = await _netboxService.GetModuleTypeProfilesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpGet("module-type-profiles/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetModuleTypeProfileById(int id)
    {
        try
        {
            var result = await _netboxService.GetModuleTypeProfileByIdAsync(id);
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

    [HttpPost("module-type-profiles")]
    //[Authorize]
    public async Task<IActionResult> CreateModuleTypeProfile([FromBody] CreateNetboxModuleTypeProfileDto dto)
    {
        try
        {
            var result = await _netboxService.CreateModuleTypeProfileAsync(dto);
            return CreatedAtAction(nameof(GetModuleTypeProfileById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpPatch("module-type-profiles/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> UpdateModuleTypeProfile(int id, [FromBody] UpdateNetboxModuleTypeProfileDto dto)
    {
        try
        {
            var result = await _netboxService.UpdateModuleTypeProfileAsync(id, dto);
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

    [HttpDelete("module-type-profiles/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> DeleteModuleTypeProfile(int id)
    {
        try
        {
            await _netboxService.DeleteModuleTypeProfileAsync(id);
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

    [HttpGet("manufacturers")]
    //[Authorize]
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

    [HttpGet("manufacturers/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetManufacturerById(int id)
    {
        try
        {
            var result = await _netboxService.GetManufacturerByIdAsync(id);
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

    [HttpPost("manufacturers")]
    //[Authorize]
    public async Task<IActionResult> CreateManufacturer([FromBody] CreateNetboxManufacturerDto dto)
    {
        try
        {
            var result = await _netboxService.CreateManufacturerAsync(dto);
            return CreatedAtAction(nameof(GetManufacturerById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpPatch("manufacturers/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> UpdateManufacturer(int id, [FromBody] UpdateNetboxManufacturerDto dto)
    {
        try
        {
            var result = await _netboxService.UpdateManufacturerAsync(id, dto);
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

    [HttpDelete("manufacturers/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> DeleteManufacturer(int id)
    {
        try
        {
            await _netboxService.DeleteManufacturerAsync(id);
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

    [HttpGet("device-roles")]
    //[Authorize]
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

    [HttpGet("device-roles/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetDeviceRoleById(int id)
    {
        try
        {
            var result = await _netboxService.GetDeviceRoleByIdAsync(id);
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

    [HttpPost("device-roles")]
    //[Authorize]
    public async Task<IActionResult> CreateDeviceRole([FromBody] CreateNetboxDeviceRoleDto dto)
    {
        try
        {
            var result = await _netboxService.CreateDeviceRoleAsync(dto);
            return CreatedAtAction(nameof(GetDeviceRoleById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpPatch("device-roles/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> UpdateDeviceRole(int id, [FromBody] UpdateNetboxDeviceRoleDto dto)
    {
        try
        {
            var result = await _netboxService.UpdateDeviceRoleAsync(id, dto);
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

    [HttpDelete("device-roles/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> DeleteDeviceRole(int id)
    {
        try
        {
            await _netboxService.DeleteDeviceRoleAsync(id);
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

    [HttpGet("device-types")]
    //[Authorize]
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

    [HttpGet("device-types/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetDeviceTypeById(int id)
    {
        try
        {
            var result = await _netboxService.GetDeviceTypeByIdAsync(id);
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

    [HttpGet("devices")]
    //[Authorize]
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

    [HttpGet("devices/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetDeviceById(int id)
    {
        try
        {
            var result = await _netboxService.GetDeviceByIdAsync(id);
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

    [HttpPost("devices")]
    //[Authorize]
    public async Task<IActionResult> CreateDevice([FromBody] CreateNetboxDeviceDto dto)
    {
        try
        {
            var result = await _netboxService.CreateDeviceAsync(dto);
            return CreatedAtAction(nameof(GetDeviceById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpPatch("devices/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> UpdateDevice(int id, [FromBody] UpdateNetboxDeviceDto dto)
    {
        try
        {
            var result = await _netboxService.UpdateDeviceAsync(id, dto);
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

    [HttpDelete("devices/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> DeleteDevice(int id)
    {
        try
        {
            await _netboxService.DeleteDeviceAsync(id);
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

    [HttpGet("racks")]
    //[Authorize]
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

    [HttpGet("racks/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetRackById(int id)
    {
        try
        {
            var result = await _netboxService.GetRackByIdAsync(id);
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

    [HttpPost("racks")]
    //[Authorize]
    public async Task<IActionResult> CreateRack([FromBody] CreateNetboxRackDto dto)
    {
        try
        {
            var result = await _netboxService.CreateRackAsync(dto);
            return CreatedAtAction(nameof(GetRackById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpPatch("racks/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> UpdateRack(int id, [FromBody] UpdateNetboxRackDto dto)
    {
        try
        {
            var result = await _netboxService.UpdateRackAsync(id, dto);
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

    [HttpDelete("racks/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> DeleteRack(int id)
    {
        try
        {
            await _netboxService.DeleteRackAsync(id);
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

    [HttpGet("virtual-machines")]
    //[Authorize]
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

    [HttpGet("virtual-machines/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetVirtualMachineById(int id)
    {
        try
        {
            var result = await _netboxService.GetVirtualMachineByIdAsync(id);
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

    [HttpGet("clusters")]
    //[Authorize]
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

    [HttpGet("clusters/{id:int}")]
    //[Authorize]
    public async Task<IActionResult> GetClusterById(int id)
    {
        try
        {
            var result = await _netboxService.GetClusterByIdAsync(id);
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
}
