using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Scno
{
    /// <summary>Solicitud para disparar el onboarding del ciclo de vida de un candidato en el SCNO.</summary>
    public class OnboardLifecycleRequestDto
    {
        [JsonPropertyName("candidate_id")]
        public string CandidateId { get; set; } = string.Empty;
    }
}
