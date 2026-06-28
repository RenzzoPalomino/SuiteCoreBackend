using System.Collections.Generic;
using System.Threading.Tasks;
using SuiteCoreBackend.DTOs.Notification;

namespace SuiteCoreBackend.Services.Interfaces
{
    public interface INotificationChannelService
    {
        Task<IEnumerable<NotificationChannelDto>> GetAllAsync();
        Task<NotificationChannelDto?> GetByIdAsync(int id);
        Task<NotificationChannelDto> CreateAsync(CreateNotificationChannelDto dto);
        Task<NotificationChannelDto?> UpdateAsync(int id, UpdateNotificationChannelDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> SendTestNotificationAsync(int id);
        Task<bool> SendTestNotificationDirectAsync(TestNotificationDirectDto dto);
    }
}
