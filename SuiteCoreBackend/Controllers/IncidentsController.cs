using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/incidents")]
    [Authorize]
    public class IncidentsController : ControllerBase
    {
        private readonly IIncidentsService _incidents;

        public IncidentsController(IIncidentsService incidents)
        {
            _incidents = incidents;
        }

        /// <summary>Resumen general de alertas e incidentes: activas, históricas y canales de notificación.</summary>
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                var result = await _incidents.GetSummaryAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener el resumen de incidentes: {ex.Message}" });
            }
        }

        /// <summary>Gráfico de dona con las alertas agrupadas por severidad.</summary>
        [HttpGet("charts/severity")]
        public async Task<IActionResult> GetSeverityChart()
        {
            try
            {
                var result = await _incidents.GetSeverityChartAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener alertas por severidad: {ex.Message}" });
            }
        }

        /// <summary>Gráfico de barras con las alertas agrupadas por módulo del sistema.</summary>
        [HttpGet("charts/modules")]
        public async Task<IActionResult> GetModulesChart()
        {
            try
            {
                var result = await _incidents.GetModulesChartAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener alertas por módulo: {ex.Message}" });
            }
        }

        /// <summary>Eventos recientes registrados en Graylog.</summary>
        [HttpGet("events")]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                var result = await _incidents.GetEventsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener eventos: {ex.Message}" });
            }
        }

        /// <summary>Eventos de seguridad recientes registrados en Graylog.</summary>
        [HttpGet("security-events")]
        public async Task<IActionResult> GetSecurityEvents()
        {
            try
            {
                var result = await _incidents.GetSecurityEventsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener eventos de seguridad: {ex.Message}" });
            }
        }
    }
}
