using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/vpn")]
    [Authorize]
    public class VpnController : ControllerBase
    {
        private readonly IWireGuardService _wireGuard;
        private readonly ITailscaleService _tailscale;

        public VpnController(IWireGuardService wireGuard, ITailscaleService tailscale)
        {
            _wireGuard = wireGuard;
            _tailscale = tailscale;
        }

        /// <summary>Estado general del gateway VPN: servidor, IP de gestión y uptime.</summary>
        [HttpGet("status")]
        public async Task<IActionResult> GetGatewayStatus()
        {
            try
            {
                var result = await _wireGuard.GetGatewayStatusAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener estado del gateway: {ex.Message}" });
            }
        }

        /// <summary>Estado de la interfaz WireGuard y tabla de peers con handshake, RX y TX.</summary>
        [HttpGet("wireguard")]
        public async Task<IActionResult> GetWireGuardStatus()
        {
            try
            {
                var result = await _wireGuard.GetStatusAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener estado WireGuard: {ex.Message}" });
            }
        }

        /// <summary>Estado de Tailscale y lista de peers del tailnet ordenados online primero.</summary>
        [HttpGet("tailscale")]
        public async Task<IActionResult> GetTailscaleStatus()
        {
            try
            {
                var result = await _tailscale.GetStatusAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener estado Tailscale: {ex.Message}" });
            }
        }

        /// <summary>Matriz estática de acceso VPN: qué redes son alcanzables desde 172.16.40.0/24.</summary>
        [HttpGet("access-policy")]
        public IActionResult GetAccessPolicy()
        {
            var result = _tailscale.GetAccessPolicy();
            return Ok(result);
        }

        /// <summary>Métricas de tráfico de la interfaz WireGuard: RX/TX en bytes, paquetes y errores.
        /// Consultar periódicamente para calcular throughput en Mbps comparando snapshots consecutivos.</summary>
        [HttpGet("wireguard/stats")]
        public async Task<IActionResult> GetWireGuardStats()
        {
            try
            {
                var result = await _wireGuard.GetStatsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener métricas WireGuard: {ex.Message}" });
            }
        }
    }
}
