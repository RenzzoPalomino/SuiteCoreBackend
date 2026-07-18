namespace SuiteCoreBackend.DTOs.SdnSupervision
{
    /// <summary>Estado del proceso de onboarding de dispositivos en el SCNO.</summary>
    public class OnboardingStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public string Integrity { get; set; } = string.Empty;
        public int CandidateCount { get; set; }
        public int ScanCount { get; set; }
        public OnboardingLatestScanDto? LatestScan { get; set; }
    }

    /// <summary>Detalle del último escaneo de onboarding ejecutado.</summary>
    public class OnboardingLatestScanDto
    {
        public string ScanId { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string RequestedBy { get; set; } = string.Empty;
        public string RequestedRole { get; set; } = string.Empty;
        public int TotalSeen { get; set; }
        public int KnownCount { get; set; }
        public int IgnoredCount { get; set; }
        public int ManualReviewCount { get; set; }
        public int EligibleCount { get; set; }
        public int PersistedCount { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
    }
}
