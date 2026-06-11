using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers;

[ApiController]
[Route("api/monitoring")]
public class MonitoringController : ControllerBase
{
    private readonly ILibreNmsService _service;

    public MonitoringController(ILibreNmsService service)
    {
        _service = service;
    }

    [HttpGet("device-types")]
    public async Task<IActionResult> GetDeviceTypes()
    {
        var result = await _service.GetDeviceTypesAsync();
        return Ok(result);
    }
}
