namespace SuiteCoreBackend.DTOs.Onboarding
{
    /// <summary>Listado de planes de onboarding generados para candidatos.</summary>
    public class OnboardingPlansListDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public int Total { get; set; }
        public string? FilterStatus { get; set; }
        public List<OnboardingPlanDto> Items { get; set; } = new();
    }

    /// <summary>Plan de onboarding de un candidato, con su aprobación y snapshot.</summary>
    public class OnboardingPlanDto
    {
        public string PlanId { get; set; } = string.Empty;
        public string CandidateId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string RiskLevel { get; set; } = string.Empty;
        public string RequestedBy { get; set; } = string.Empty;
        public string RequestedRole { get; set; } = string.Empty;
        public string? ApprovedBy { get; set; }
        public string? ApprovedRole { get; set; }
        public string? ApprovalNote { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public string? ApprovedAt { get; set; }
        public string UpdatedAt { get; set; } = string.Empty;
        public OnboardingPlanDetailDto Plan { get; set; } = new();

        /// <summary>Copia del candidato al momento de generar el plan.</summary>
        public Dictionary<string, object?> CandidateSnapshot { get; set; } = new();
    }

    /// <summary>Detalle técnico del plan: pasos de ejecución y configuración destino.</summary>
    public class OnboardingPlanDetailDto
    {
        public string AssetKind { get; set; } = string.Empty;

        /// <summary>Datos del candidato origen del plan.</summary>
        public Dictionary<string, object?> Candidate { get; set; } = new();

        /// <summary>Configuración destino a aplicar (NetBox, Proxmox, etc.), variable según el tipo de activo.</summary>
        public Dictionary<string, object?> Configuration { get; set; } = new();

        public string? ExecutionBlockReason { get; set; }
        public bool ExecutionReady { get; set; }
        public bool RequiresApproval { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public List<OnboardingPlanStepDto> Steps { get; set; } = new();
        public string Type { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
    }

    /// <summary>Paso individual de ejecución de un plan de onboarding.</summary>
    public class OnboardingPlanStepDto
    {
        public string Action { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }
        public string? Rollback { get; set; }
        public string System { get; set; } = string.Empty;
    }
}
