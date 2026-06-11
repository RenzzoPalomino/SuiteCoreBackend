using SuiteCoreBackend.DTOs.Monitoring;

namespace SuiteCoreBackend.Services.Interfaces;

public interface ILibreNmsService
{
    Task<IEnumerable<DeviceTypeDto>> GetDeviceTypesAsync();
}
