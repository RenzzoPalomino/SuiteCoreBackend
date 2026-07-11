using SuiteCoreBackend.DTOs.Monitoring;

namespace SuiteCoreBackend.Services.Interfaces;

public interface IGrafanaEmbedService
{
    Task<IEnumerable<GrafanaEmbedLinkDto>> GetEmbedLinksAsync();
}
