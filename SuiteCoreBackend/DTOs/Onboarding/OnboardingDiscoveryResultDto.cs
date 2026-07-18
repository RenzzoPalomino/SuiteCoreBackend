namespace SuiteCoreBackend.DTOs.Onboarding
{
    /// <summary>Resultado de un escaneo de descubrimiento de dispositivos candidatos (local o Tailscale).</summary>
    public class OnboardingDiscoveryResultDto
    {
        public string Status { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public string CheckedAt { get; set; } = string.Empty;

        /// <summary>Reglas del escaneo. Varían según la fuente (local vs. Tailscale).</summary>
        public Dictionary<string, object?> Rules { get; set; } = new();

        public OnboardingDiscoverySummaryDto Summary { get; set; } = new();

        /// <summary>Conteo de elementos excluidos por motivo (clave = motivo, valor = cantidad).</summary>
        public Dictionary<string, int> ExcludedSummary { get; set; } = new();

        public List<object> Observations { get; set; } = new();
        public List<OnboardingDiscoveryCandidateDto> Candidates { get; set; } = new();
    }

    /// <summary>Resumen numérico de un escaneo de descubrimiento.</summary>
    public class OnboardingDiscoverySummaryDto
    {
        public int Total { get; set; }
        public int Known { get; set; }
        public int Ignored { get; set; }
        public int ManualReview { get; set; }
        public int Eligible { get; set; }
    }

    /// <summary>Dispositivo candidato detectado en un escaneo de descubrimiento (aún no persistido).</summary>
    public class OnboardingDiscoveryCandidateDto
    {
        public string CandidateId { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string SourceKey { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Hostname { get; set; } = string.Empty;
        public string ManagementIp { get; set; } = string.Empty;
        public string OperatingSystem { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public Dictionary<string, object?> Metadata { get; set; } = new();
    }
}
