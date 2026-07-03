using SuiteCoreBackend.DTOs.Sdn;
using System.Text.Json;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface ISdnService
    {
        /// <summary>Verifica el estado del servicio SCNO y el bridge OVS.</summary>
        Task<SdnHealthDto> GetHealthAsync();

        /// <summary>Obtiene la topología SDN: bridge, controlador y puertos activos.</summary>
        Task<SdnTopologyDto> GetTopologyAsync();

        /// <summary>Obtiene estadísticas de puertos del bridge SDN.</summary>
        Task<SdnStatisticsDto> GetStatisticsAsync();

        /// <summary>Obtiene los flujos OpenFlow activos en el bridge SDN.</summary>
        Task<SdnFlowsDto> GetFlowsAsync();

        /// <summary>Obtiene información del dispositivo MikroTik administrado por el SCNO.</summary>
        Task<MikroTikInfoDto> GetMikroTikInfoAsync();

        /// <summary>Obtiene las interfaces del MikroTik administrado por el SCNO.</summary>
        Task<MikroTikInterfacesDto> GetMikroTikInterfacesAsync();

        /// <summary>Bloquea una dirección IP en el bridge SDN via regla OpenFlow.</summary>
        Task<object> BlockIpAsync(SdnBlockIpDto request);

        /// <summary>Elimina el bloqueo de una dirección IP en el bridge SDN.</summary>
        Task<object> UnblockIpAsync(SdnBlockIpDto request);

        /// <summary>Ejecuta una acción de automatización SDN en el SCNO.</summary>
        Task<object> ExecuteAutomationAsync(SdnAutomationRequestDto request);

        /// <summary>Reenvía un evento de Graylog al SCNO para procesamiento SDN.</summary>
        Task<object> ForwardGraylogWebhookAsync(JsonElement payload);
    }
}
