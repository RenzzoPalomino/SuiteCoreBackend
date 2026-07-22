namespace SuiteCoreBackend.Settings
{
    public class ScnoSettings
    {
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>Usuario enviado en el header X-SCNO-User para operaciones que lo requieren.</summary>
        public string User { get; set; } = string.Empty;

        /// <summary>Rol enviado en el header X-SCNO-Role para operaciones que lo requieren.</summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>Usuario enviado en el header X-SCNO-User al aprobar planes de onboarding.</summary>
        public string ApproverUser { get; set; } = string.Empty;
    }
}
