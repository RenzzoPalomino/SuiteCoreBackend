using SuiteCoreBackend.DTOs.Backup;
using SuiteCoreBackend.DTOs.Oxidized;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IOxidizedService
    {
        Task<List<OxidizedDeviceDto>> GetDevicesAsync();
        Task<List<OxidizedBackupDto>> GetAllDeviceBackupsAsync();
        Task<OxidizedBackupDto> GetDeviceBackupAsync(string deviceName);
    }
}
