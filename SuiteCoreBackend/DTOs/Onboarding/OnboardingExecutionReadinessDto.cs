namespace SuiteCoreBackend.DTOs.Onboarding
{
    /// <summary>Estado de disponibilidad de ejecución automatizada del onboarding (orquestador y conectores).</summary>
    public class OnboardingExecutionReadinessDto
    {
        public bool Success { get; set; }
        public string Service { get; set; } = string.Empty;
        public string ExecutionScope { get; set; } = string.Empty;
        public List<string> ExecutionScopes { get; set; } = new();
        public List<string> ExecutorRoles { get; set; } = new();
        public bool ExecutorInstalled { get; set; }
        public bool ExecutionEndpointPublished { get; set; }
        public bool ExternalWriteAuthorized { get; set; }
        public bool OverallExecutionReady { get; set; }

        /// <summary>Estado del orquestador de ejecución por etapas (netbox/librenms/oxidized).</summary>
        public Dictionary<string, object?> Orchestrator { get; set; } = new();

        /// <summary>Estado agregado del conector de ejecución.</summary>
        public Dictionary<string, object?> Connector { get; set; } = new();

        /// <summary>Estado por sistema conectado (netbox, oxidized, librenms), campos variables por sistema.</summary>
        public Dictionary<string, object?> Connectors { get; set; } = new();

        /// <summary>Estado por ejecutor (netbox, oxidized, librenms), incluye los pasos soportados por cada uno.</summary>
        public Dictionary<string, object?> Executors { get; set; } = new();

        public List<int> SupportedSteps { get; set; } = new();
        public List<object> BlockedSteps { get; set; } = new();
        public List<object> ExcludedSystems { get; set; } = new();

        /// <summary>Estado del orquestador legado (compatibilidad hacia atrás).</summary>
        public Dictionary<string, object?> LegacyOrchestrator { get; set; } = new();

        public bool ExecutionAvailable { get; set; }
    }
}
