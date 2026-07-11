using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.DTOs.Sdn;
using SuiteCoreBackend.Services.Interfaces;
using System.Text.Json;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/sdn")]
    //[Authorize]
    public class SdnController : ControllerBase
    {
        private readonly ISdnService _sdn;

        public SdnController(ISdnService sdn)
        {
            _sdn = sdn;
        }

        /// <summary>Estado del servicio SCNO: bridge OVS, versión OpenFlow y timestamp.</summary>
        [HttpGet("health")]
        public async Task<IActionResult> GetHealth()
        {
            try
            {
                var result = await _sdn.GetHealthAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener estado del SCNO: {ex.Message}" });
            }
        }

        /// <summary>Topología SDN: bridge, controlador OpenFlow, versión OVS y puertos activos.</summary>
        [HttpGet("topology")]
        public async Task<IActionResult> GetTopology()
        {
            try
            {
                var result = await _sdn.GetTopologyAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener topología SDN: {ex.Message}" });
            }
        }

        /// <summary>Estadísticas de puertos del bridge SDN.</summary>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var result = await _sdn.GetStatisticsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener estadísticas SDN: {ex.Message}" });
            }
        }

        /// <summary>Flujos OpenFlow activos instalados en el bridge SDN.</summary>
        [HttpGet("flows")]
        public async Task<IActionResult> GetFlows()
        {
            try
            {
                var result = await _sdn.GetFlowsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener flujos SDN: {ex.Message}" });
            }
        }

        /// <summary>Información del dispositivo MikroTik administrado por el SCNO.</summary>
        [HttpGet("mikrotik/info")]
        public async Task<IActionResult> GetMikroTikInfo()
        {
            try
            {
                var result = await _sdn.GetMikroTikInfoAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener info MikroTik: {ex.Message}" });
            }
        }

        /// <summary>Interfaces del dispositivo MikroTik administrado por el SCNO.</summary>
        [HttpGet("mikrotik/interfaces")]
        public async Task<IActionResult> GetMikroTikInterfaces()
        {
            try
            {
                var result = await _sdn.GetMikroTikInterfacesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener interfaces MikroTik: {ex.Message}" });
            }
        }

        /// <summary>Bloquea una IP en el bridge SDN instalando una regla OpenFlow de descarte.</summary>
        [HttpPost("security/block-ip")]
        //[Authorize(Roles = "5101,5102,5103")]
        public async Task<IActionResult> BlockIp([FromBody] SdnBlockIpDto request)
        {
            try
            {
                var result = await _sdn.BlockIpAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al bloquear IP: {ex.Message}" });
            }
        }

        /// <summary>Elimina el bloqueo de una IP en el bridge SDN.</summary>
        [HttpDelete("security/block-ip")]
        //[Authorize(Roles = "5101,5102,5103")]
        public async Task<IActionResult> UnblockIp([FromBody] SdnBlockIpDto request)
        {
            try
            {
                var result = await _sdn.UnblockIpAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al desbloquear IP: {ex.Message}" });
            }
        }

        /// <summary>Ejecuta una acción de automatización SDN en el SCNO.</summary>
        [HttpPost("automation/execute")]
        //[Authorize(Roles = "5101,5103")]
        public async Task<IActionResult> ExecuteAutomation([FromBody] SdnAutomationRequestDto request)
        {
            try
            {
                var result = await _sdn.ExecuteAutomationAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al ejecutar automatización SDN: {ex.Message}" });
            }
        }

        /// <summary>Recibe y reenvía eventos Graylog al SCNO para correlación SDN.</summary>
        [HttpPost("webhooks/graylog")]
        [AllowAnonymous]
        public async Task<IActionResult> GraylogWebhook([FromBody] JsonElement payload)
        {
            try
            {
                var result = await _sdn.ForwardGraylogWebhookAsync(payload);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al procesar webhook Graylog: {ex.Message}" });
            }
        }
    }
}
