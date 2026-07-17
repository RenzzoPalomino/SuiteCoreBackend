using SuiteCoreBackend.DTOs.SdnSupervision;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface ISdnSupervisionService
    {
        /// <summary>Obtiene la topología del bridge SDN: controlador, versión OVS y puertos activos.</summary>
        Task<SdnSupervisionTopologyDto> GetTopologyAsync();

        /// <summary>Obtiene los flujos OpenFlow activos instalados en el bridge SDN.</summary>
        Task<SdnSupervisionFlowsDto> GetFlowsAsync();

        /// <summary>Obtiene el estado del proceso de onboarding de dispositivos en el SCNO.</summary>
        Task<OnboardingStatusDto> GetOnboardingStatusAsync();

        /// <summary>Obtiene el manifiesto canónico del último proceso de decomisión SDN.</summary>
        Task<DecommissionManifestDto> GetDecommissionManifestAsync();
    }
}
