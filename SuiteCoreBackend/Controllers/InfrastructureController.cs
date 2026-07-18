using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/infrastructure")]
    //[Authorize]
    public class InfrastructureController : ControllerBase
    {
        private readonly IInfrastructureService _infrastructure;

        public InfrastructureController(IInfrastructureService infrastructure)
        {
            _infrastructure = infrastructure;
        }

        /// <summary>Resumen general del estado de la infraestructura: nodos, VMs, memoria y almacenamiento.</summary>
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                var result = await _infrastructure.GetSummaryAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener el resumen de infraestructura: {ex.Message}" });
            }
        }

        /// <summary>Gráfico de dona con el uso de recursos: CPU, memoria y almacenamiento.</summary>
        [HttpGet("charts/resources")]
        public async Task<IActionResult> GetResourcesUsageChart()
        {
            try
            {
                var result = await _infrastructure.GetResourcesUsageChartAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener el uso de recursos: {ex.Message}" });
            }
        }
    }
}
