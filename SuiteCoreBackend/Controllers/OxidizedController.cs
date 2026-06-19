using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/oxidized")]
    [Authorize]
    public class OxidizedController : Controller
    {
        private readonly IOxidizedService _oxidizedService;

        public OxidizedController(IOxidizedService oxidizedService)
        {
            _oxidizedService = oxidizedService;
        }

        [HttpGet("devices")]
        public async Task<IActionResult> GetDevices()
        {

            try
            {
                var devices = await _oxidizedService.GetDevicesAsync();

                return Ok(new
                {
                    total = devices.Count,
                    devices
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, details = ex.Message });
            }
            
        }

        [HttpGet("backups")]
        public async Task<IActionResult> GetAllBackups()
        {
            try
            {
                var backups = await _oxidizedService.GetAllDeviceBackupsAsync();

                return Ok(new
                {
                    total = backups.Count,
                    backups
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, details = ex.Message });
            }
        }

        [HttpGet("devices/{deviceName}/backup")]
        public async Task<IActionResult> GetDeviceBackup(string deviceName)
        {
            try
            {
                var backup = await _oxidizedService.GetDeviceBackupAsync(deviceName);

                return Ok(backup);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, details = ex.Message });
            }
        }
    }
}
