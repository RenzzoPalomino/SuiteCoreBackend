using SuiteCoreBackend.DTOs.Monitoring;

namespace SuiteCoreBackend.Services.Interfaces;

public interface INetboxService
{
    Task<IEnumerable<NetboxRegionDto>> GetRegionsAsync();
}
