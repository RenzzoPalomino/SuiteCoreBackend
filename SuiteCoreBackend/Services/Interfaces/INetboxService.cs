using SuiteCoreBackend.DTOs.Monitoring;

namespace SuiteCoreBackend.Services.Interfaces;

public interface INetboxService
{
    Task<IEnumerable<NetboxRegionDto>> GetRegionsAsync();
    Task<NetboxRegionDto> CreateRegionAsync(CreateNetboxRegionDto dto);
    Task<NetboxRegionDetailDto> GetRegionByIdAsync(int id);
    Task<NetboxRegionDetailDto> UpdateRegionAsync(int id, UpdateNetboxRegionDto dto);
    Task<bool> DeleteRegionAsync(int id);
}
