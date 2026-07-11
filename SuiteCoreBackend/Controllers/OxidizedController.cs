using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/oxidized")]
    //[Authorize]
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

        /// <summary>
        /// Obtiene el historial de versiones del backup de un dispositivo registrado en Oxidized.
        /// </summary>
        /// <param name="deviceName">Nombre del dispositivo registrado en Oxidized.</param>
        /// <returns>Lista de versiones disponibles.</returns>
        [HttpGet("devices/{deviceName}/versions")]
        public async Task<IActionResult> GetDeviceVersions(string deviceName)
        {
            var versions = await _oxidizedService.GetDeviceVersionsAsync(deviceName);

            return Ok(new
            {
                deviceName,
                total = versions.Count,
                versions
            });
        }


        [HttpGet("devices/{deviceName}/backup")]
        public async Task<IActionResult> GetDeviceBackup(
            string deviceName,
            [FromQuery] string? oid = null,
            [FromQuery] long? epoch = null,
            [FromQuery] int? num = null,
            [FromQuery] string? group = "")
        {
            try
            {
                var backup = await _oxidizedService.GetDeviceBackupAsync(deviceName,oid,epoch,num,group);

                return Ok(backup);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    details = ex.Message
                });
            }
        }
    }
}
