using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    //[Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboard;

        public DashboardController(IDashboardService dashboard)
        {
            _dashboard = dashboard;
        }

        /// <summary>Estado general del sistema: alertas activas, checks y recursos monitoreados.</summary>
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                var result = await _dashboard.GetSummaryAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener el resumen del dashboard: {ex.Message}" });
            }
        }

        /// <summary>Gráfico de barras con el estado de los módulos del sistema.</summary>
        [HttpGet("charts/modules-status")]
        public async Task<IActionResult> GetModulesStatusChart()
        {
            try
            {
                var result = await _dashboard.GetModulesStatusChartAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener el estado de módulos: {ex.Message}" });
            }
        }

        /// <summary>Gráfico de dona con el estado de los servicios del sistema.</summary>
        [HttpGet("charts/services-status")]
        public async Task<IActionResult> GetServicesStatusChart()
        {
            try
            {
                var result = await _dashboard.GetServicesStatusChartAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener el estado de servicios: {ex.Message}" });
            }
        }
    }
}
