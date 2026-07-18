namespace SuiteCoreBackend.DTOs.Onboarding
{
    /// <summary>Listado de candidatos de onboarding persistidos en el SCNO.</summary>
    public class OnboardingCandidatesListDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public int Total { get; set; }
        public string? State { get; set; }
        public List<OnboardingCandidateDto> Items { get; set; } = new();
    }

    /// <summary>Candidato de onboarding persistido, con su ciclo de vida completo.</summary>
    public class OnboardingCandidateDto
    {
        public string CandidateId { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string SourceKey { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Hostname { get; set; } = string.Empty;
        public string ManagementIp { get; set; } = string.Empty;
        public string OperatingSystem { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Eligibility { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string FirstSeenAt { get; set; } = string.Empty;
        public string LastSeenAt { get; set; } = string.Empty;
        public string LastOnlineAt { get; set; } = string.Empty;
        public int DiscoveryCount { get; set; }
        public string DiscoveredBy { get; set; } = string.Empty;
        public string DiscoveredRole { get; set; } = string.Empty;
        public string LastScanId { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
        public string UpdatedAt { get; set; } = string.Empty;
        public Dictionary<string, object?> Metadata { get; set; } = new();
    }
}
