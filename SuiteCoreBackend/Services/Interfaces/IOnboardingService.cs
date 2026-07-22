using SuiteCoreBackend.DTOs.Onboarding;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IOnboardingService
    {
        /// <summary>Obtiene el estado general del proceso de onboarding de dispositivos.</summary>
        Task<OnboardingStatusDto> GetStatusAsync();

        /// <summary>Ejecuta un escaneo de descubrimiento de dispositivos MikroTik locales (RouterOS API).</summary>
        Task<OnboardingDiscoveryResultDto> GetLocalDiscoveryAsync();

        /// <summary>Ejecuta un escaneo de descubrimiento de dispositivos candidatos en la malla Tailscale.</summary>
        Task<OnboardingDiscoveryResultDto> GetTailscaleDiscoveryAsync();

        /// <summary>
        /// Obtiene la respuesta cruda (sin deserializar) del listado de candidatos de onboarding persistidos
        /// en el SCNO, para reenviarla tal cual al cliente (proxy directo).
        /// </summary>
        Task<HttpResponseMessage> GetCandidatesRawAsync();

        /// <summary>Obtiene el listado de planes de onboarding generados para candidatos.</summary>
        Task<OnboardingPlansListDto> GetPlansAsync();

        /// <summary>Obtiene el detalle de un plan de onboarding por su Id.</summary>
        /// <param name="planId">Id del plan de onboarding.</param>
        Task<OnboardingPlanDto> GetPlanByIdAsync(string planId);

        /// <summary>Obtiene el estado de disponibilidad de ejecución automatizada del onboarding.</summary>
        Task<OnboardingExecutionReadinessDto> GetExecutionReadinessAsync();

        /// <summary>
        /// Obtiene la respuesta cruda (sin deserializar) del listado de ejecuciones de planes de onboarding,
        /// para reenviarla tal cual al cliente (proxy directo).
        /// </summary>
        Task<HttpResponseMessage> GetExecutionsRawAsync();

        /// <summary>
        /// Obtiene la respuesta cruda (sin deserializar) del detalle de una ejecución de onboarding,
        /// para reenviarla tal cual al cliente (proxy directo).
        /// </summary>
        /// <param name="executionId">Id de la ejecución.</param>
        Task<HttpResponseMessage> GetExecutionByIdRawAsync(string executionId);

        /// <summary>
        /// Obtiene la respuesta cruda (sin deserializar) de los pasos de una ejecución de onboarding,
        /// para reenviarla tal cual al cliente (proxy directo).
        /// </summary>
        /// <param name="executionId">Id de la ejecución.</param>
        Task<HttpResponseMessage> GetExecutionStepsRawAsync(string executionId);

        /// <summary>
        /// Dispara un escaneo de descubrimiento de dispositivos MikroTik locales y devuelve la respuesta
        /// cruda (sin deserializar) tal cual la entrega el SCNO (proxy directo).
        /// </summary>
        Task<HttpResponseMessage> TriggerLocalDiscoveryScanRawAsync();

        /// <summary>
        /// Dispara un escaneo de descubrimiento de dispositivos candidatos en la malla Tailscale y devuelve
        /// la respuesta cruda (sin deserializar) tal cual la entrega el SCNO (proxy directo).
        /// </summary>
        Task<HttpResponseMessage> TriggerTailscaleDiscoveryScanRawAsync();

        /// <summary>
        /// Genera el plan de onboarding para un candidato y devuelve la respuesta cruda (sin deserializar)
        /// tal cual la entrega el SCNO (proxy directo).
        /// </summary>
        /// <param name="candidateId">Id del candidato.</param>
        Task<HttpResponseMessage> CreatePlanForCandidateRawAsync(string candidateId);

        /// <summary>
        /// Ejecuta un plan de onboarding y devuelve la respuesta cruda (sin deserializar) tal cual la
        /// entrega el SCNO (proxy directo).
        /// </summary>
        /// <param name="planId">Id del plan de onboarding.</param>
        Task<HttpResponseMessage> ExecutePlanRawAsync(string planId);

        /// <summary>
        /// Aprueba un plan de onboarding y devuelve la respuesta cruda (sin deserializar) tal cual la
        /// entrega el SCNO (proxy directo).
        /// </summary>
        /// <param name="planId">Id del plan de onboarding.</param>
        Task<HttpResponseMessage> ApprovePlanRawAsync(string planId);
    }
}
