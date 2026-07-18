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

        /// <summary>Obtiene el listado de candidatos de onboarding persistidos en el SCNO.</summary>
        Task<OnboardingCandidatesListDto> GetCandidatesAsync();

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
    }
}
