using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/sdn-supervision")]
    //[Authorize]
    public class SdnSupervisionController : ControllerBase
    {
        private readonly ISdnSupervisionService _sdnSupervision;

        public SdnSupervisionController(ISdnSupervisionService sdnSupervision)
        {
            _sdnSupervision = sdnSupervision;
        }

        /// <summary>Topología del bridge SDN: controlador OpenFlow, versión OVS y puertos activos.</summary>
        [HttpGet("topology")]
        public async Task<IActionResult> GetTopology()
        {
            try
            {
                var result = await _sdnSupervision.GetTopologyAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener topología SDN: {ex.Message}" });
            }
        }

        /// <summary>Flujos OpenFlow activos instalados en el bridge SDN.</summary>
        [HttpGet("flows")]
        public async Task<IActionResult> GetFlows()
        {
            try
            {
                var result = await _sdnSupervision.GetFlowsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener flujos SDN: {ex.Message}" });
            }
        }

        /// <summary>Estado del proceso de onboarding de dispositivos en el SCNO.</summary>
        [HttpGet("onboarding/status")]
        public async Task<IActionResult> GetOnboardingStatus()
        {
            try
            {
                var result = await _sdnSupervision.GetOnboardingStatusAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener estado de onboarding: {ex.Message}" });
            }
        }

        /// <summary>Manifiesto canónico del último proceso de decomisión SDN.</summary>
        [HttpGet("decommission/manifest")]
        public async Task<IActionResult> GetDecommissionManifest()
        {
            try
            {
                var result = await _sdnSupervision.GetDecommissionManifestAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener manifiesto de decomisión: {ex.Message}" });
            }
        }
    }
}
