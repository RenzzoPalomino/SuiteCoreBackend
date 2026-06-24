using SuiteCoreBackend.DTOs.Backup;
using SuiteCoreBackend.DTOs.Oxidized;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface IOxidizedService
    {
        Task<List<OxidizedDeviceDto>> GetDevicesAsync();
        Task<OxidizedBackupDto> GetDeviceBackupAsync(
            string deviceName,
            string? oid = null,
            long? epoch = null,
            int? num = null,
            string? group = ""
        );

        Task<List<OxidizedVersionDto>> GetDeviceVersionsAsync(string deviceName);
    }
}
