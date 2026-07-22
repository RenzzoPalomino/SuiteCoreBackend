using System.Text.Json.Serialization;

namespace SuiteCoreBackend.DTOs.Scno
{
    /// <summary>Solicitud para disparar la decomisión del ciclo de vida de un candidato en el SCNO.</summary>
    public class DecommissionLifecycleRequestDto
    {
        [JsonPropertyName("plan_id")]
        public string PlanId { get; set; } = string.Empty;

        [JsonPropertyName("candidate_id")]
        public string CandidateId { get; set; } = string.Empty;

        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;
    }
}
