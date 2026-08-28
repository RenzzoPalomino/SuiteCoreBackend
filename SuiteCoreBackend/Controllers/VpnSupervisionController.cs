using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/vpn-supervision")]
    [Authorize]
    public class VpnSupervisionController : ControllerBase
    {
        private readonly IVpnSupervisionService _vpnSupervision;

        public VpnSupervisionController(IVpnSupervisionService vpnSupervision)
        {
            _vpnSupervision = vpnSupervision;
        }

        /// <summary>Estado de salud agregado de los componentes VPN (Tailscale, WireGuard).</summary>
        [HttpGet("health")]
        public async Task<IActionResult> GetHealth()
        {
            try
            {
                var result = await _vpnSupervision.GetHealthAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener salud de VPN: {ex.Message}" });
            }
        }

        /// <summary>Estado detallado de Tailscale y los nodos de la malla.</summary>
        [HttpGet("tailscale/status")]
        public async Task<IActionResult> GetTailscaleStatus()
        {
            try
            {
                var result = await _vpnSupervision.GetTailscaleStatusAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener estado de Tailscale: {ex.Message}" });
            }
        }

        /// <summary>Estado detallado de WireGuard, sus peers y la prueba de conectividad.</summary>
        [HttpGet("wireguard/status")]
        public async Task<IActionResult> GetWireGuardStatus()
        {
            try
            {
                var result = await _vpnSupervision.GetWireGuardStatusAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener estado de WireGuard: {ex.Message}" });
            }
        }
    }
}
