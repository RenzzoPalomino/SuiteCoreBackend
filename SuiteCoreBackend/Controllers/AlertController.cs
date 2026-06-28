using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.DTOs.Alert;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/alerts")]
    public class AlertController : ControllerBase
    {
        private readonly IAlertService _alertService;

        public AlertController(IAlertService alertService)
        {
            _alertService = alertService;
        }

        [HttpPost("grafana")]
        public async Task<IActionResult> GrafanaWebhook([FromBody] GrafanaWebhookDto dto, [FromQuery] int? channelId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _alertService.ProcessGrafanaAlertAsync(dto, channelId);
            if (!success)
            {
                return StatusCode(500, new { message = "Se recibió la alerta de Grafana pero hubo errores al notificar a todos los canales de Telegram." });
            }

            return Ok(new { message = "Alerta de Grafana procesada e inyectada con éxito." });
        }

        [HttpPost("librenms")]
        public async Task<IActionResult> LibreNmsWebhook([FromBody] LibreNmsWebhookDto dto, [FromQuery] int? channelId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _alertService.ProcessLibreNmsAlertAsync(dto, channelId);
            if (!success)
            {
                return StatusCode(500, new { message = "Se recibió la alerta de LibreNMS pero hubo errores al notificar a todos los canales de Telegram." });
            }

            return Ok(new { message = "Alerta de LibreNMS procesada e inyectada con éxito." });
        }
    }
}
