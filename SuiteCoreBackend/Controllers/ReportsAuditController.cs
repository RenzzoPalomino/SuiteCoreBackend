using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/reports-audit")]
    //[Authorize]
    public class ReportsAuditController : ControllerBase
    {
        private readonly IReportsAuditService _reportsAudit;

        public ReportsAuditController(IReportsAuditService reportsAudit)
        {
            _reportsAudit = reportsAudit;
        }

        /// <summary>Historial de eventos de alertas gestionados por el SCNO Alert Manager.</summary>
        [HttpGet("history")]
        public async Task<IActionResult> GetAlertsHistory()
        {
            try
            {
                var result = await _reportsAudit.GetAlertsHistoryAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener el historial de alertas: {ex.Message}" });
            }
        }

        /// <summary>Estado de la base de datos de auditoría de automatización del SCNO.</summary>
        [HttpGet("audit-status")]
        public async Task<IActionResult> GetAuditStatus()
        {
            try
            {
                var result = await _reportsAudit.GetAuditStatusAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener el estado de auditoría: {ex.Message}" });
            }
        }
    }
}
