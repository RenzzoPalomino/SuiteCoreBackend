namespace SuiteCoreBackend.DTOs.Sdn
{
    /// <summary>Payload para ejecutar una acción de automatización SDN.</summary>
    public class SdnAutomationRequestDto
    {
        /// <summary>Nombre de la acción a ejecutar en el SCNO.</summary>
        public string Action { get; set; } = string.Empty;
    }
}
