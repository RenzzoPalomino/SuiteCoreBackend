namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IScnoService
    {
        /// <summary>
        /// Dispara el proceso de decomisión del ciclo de vida del SCNO para un candidato/plan y devuelve la
        /// respuesta cruda (sin deserializar) tal cual la entrega el SCNO (proxy directo).
        /// </summary>
        /// <param name="planId">Id del plan de onboarding asociado.</param>
        /// <param name="candidateId">Id del candidato a decomisionar.</param>
        /// <param name="reason">Motivo de la decomisión.</param>
        Task<HttpResponseMessage> TriggerLifecycleDecommissionRawAsync(string planId, string candidateId, string reason);

        /// <summary>
        /// Dispara el proceso de onboarding del ciclo de vida del SCNO para un candidato y devuelve la
        /// respuesta cruda (sin deserializar) tal cual la entrega el SCNO (proxy directo).
        /// </summary>
        /// <param name="candidateId">Id del candidato a onboardear.</param>
        Task<HttpResponseMessage> TriggerLifecycleOnboardRawAsync(string candidateId);
    }
}
