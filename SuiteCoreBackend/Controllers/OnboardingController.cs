using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/onboarding")]
    //[Authorize]
    public class OnboardingController : ControllerBase
    {
        private readonly IOnboardingService _onboarding;

        public OnboardingController(IOnboardingService onboarding)
        {
            _onboarding = onboarding;
        }

        /// <summary>Estado general del proceso de onboarding de dispositivos en el SCNO.</summary>
        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            try
            {
                var result = await _onboarding.GetStatusAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener estado de onboarding: {ex.Message}" });
            }
        }

        /// <summary>Escanea dispositivos MikroTik locales candidatos vía RouterOS API.</summary>
        [HttpGet("discovery/local")]
        public async Task<IActionResult> GetLocalDiscovery()
        {
            try
            {
                var result = await _onboarding.GetLocalDiscoveryAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al descubrir dispositivos locales: {ex.Message}" });
            }
        }

        /// <summary>Escanea dispositivos candidatos en la malla Tailscale.</summary>
        [HttpGet("discovery/tailscale")]
        public async Task<IActionResult> GetTailscaleDiscovery()
        {
            try
            {
                var result = await _onboarding.GetTailscaleDiscoveryAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al descubrir dispositivos en Tailscale: {ex.Message}" });
            }
        }

        /// <summary>Listado de candidatos de onboarding persistidos en el SCNO.</summary>
        [HttpGet("candidates")]
        public async Task<IActionResult> GetCandidates()
        {
            try
            {
                var result = await _onboarding.GetCandidatesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener candidatos de onboarding: {ex.Message}" });
            }
        }

        /// <summary>Listado de planes de onboarding generados para candidatos.</summary>
        [HttpGet("plans")]
        public async Task<IActionResult> GetPlans()
        {
            try
            {
                var result = await _onboarding.GetPlansAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener planes de onboarding: {ex.Message}" });
            }
        }

        /// <summary>Detalle de un plan de onboarding por su Id.</summary>
        [HttpGet("plans/{planId}")]
        public async Task<IActionResult> GetPlanById(string planId)
        {
            try
            {
                var result = await _onboarding.GetPlanByIdAsync(planId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener el plan de onboarding: {ex.Message}" });
            }
        }

        /// <summary>Estado de disponibilidad de ejecución automatizada del onboarding.</summary>
        [HttpGet("execution/readiness")]
        public async Task<IActionResult> GetExecutionReadiness()
        {
            try
            {
                var result = await _onboarding.GetExecutionReadinessAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener disponibilidad de ejecución: {ex.Message}" });
            }
        }

        /// <summary>
        /// Listado de ejecuciones de planes de onboarding. Actúa como proxy directo: reenvía el código de
        /// estado y el cuerpo exactos devueltos por el SCNO, sin deserializar ni transformar la respuesta.
        /// </summary>
        [HttpGet("executions")]
        public async Task<IActionResult> GetExecutions()
        {
            using var response = await _onboarding.GetExecutionsRawAsync();
            return await ProxyResponseAsync(response);
        }

        /// <summary>
        /// Detalle de una ejecución de onboarding por su Id. Actúa como proxy directo: reenvía el código
        /// de estado y el cuerpo exactos devueltos por el SCNO, sin deserializar ni transformar la respuesta.
        /// </summary>
        [HttpGet("executions/{executionId}")]
        public async Task<IActionResult> GetExecutionById(string executionId)
        {
            using var response = await _onboarding.GetExecutionByIdRawAsync(executionId);
            return await ProxyResponseAsync(response);
        }

        /// <summary>
        /// Pasos de una ejecución de onboarding. Actúa como proxy directo: reenvía el código de estado
        /// y el cuerpo exactos devueltos por el SCNO, sin deserializar ni transformar la respuesta.
        /// </summary>
        [HttpGet("executions/{executionId}/steps")]
        public async Task<IActionResult> GetExecutionSteps(string executionId)
        {
            using var response = await _onboarding.GetExecutionStepsRawAsync(executionId);
            return await ProxyResponseAsync(response);
        }

        private async Task<IActionResult> ProxyResponseAsync(HttpResponseMessage response)
        {
            Response.StatusCode = (int)response.StatusCode;
            Response.ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";

            await response.Content.CopyToAsync(Response.Body);
            return new EmptyResult();
        }
    }
}
