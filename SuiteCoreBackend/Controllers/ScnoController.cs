using Microsoft.AspNetCore.Mvc;
using SuiteCoreBackend.DTOs.Scno;
using SuiteCoreBackend.Services.Interfaces;

namespace SuiteCoreBackend.Controllers
{
    [ApiController]
    [Route("api/scno")]
    //[Authorize]
    public class ScnoController : ControllerBase
    {
        private readonly IScnoService _scno;

        public ScnoController(IScnoService scno)
        {
            _scno = scno;
        }

        /// <summary>
        /// Dispara el proceso de decomisión del ciclo de vida del SCNO. Actúa como proxy directo: reenvía
        /// el código de estado y el cuerpo exactos devueltos por el SCNO, sin deserializar la respuesta.
        /// </summary>
        [HttpPost("lifecycle/decommission")]
        public async Task<IActionResult> TriggerLifecycleDecommission([FromBody] DecommissionLifecycleRequestDto request)
        {
            using var response = await _scno.TriggerLifecycleDecommissionRawAsync(request.PlanId, request.CandidateId, request.Reason);
            return await ProxyResponseAsync(response);
        }

        /// <summary>
        /// Dispara el proceso de onboarding del ciclo de vida del SCNO. Actúa como proxy directo: reenvía
        /// el código de estado y el cuerpo exactos devueltos por el SCNO, sin deserializar la respuesta.
        /// </summary>
        [HttpPost("lifecycle/onboard")]
        public async Task<IActionResult> TriggerLifecycleOnboard([FromBody] OnboardLifecycleRequestDto request)
        {
            using var response = await _scno.TriggerLifecycleOnboardRawAsync(request.CandidateId);
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
