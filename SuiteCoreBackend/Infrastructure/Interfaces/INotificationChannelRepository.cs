using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Infrastructure.Interfaces
{
    public interface INotificationChannelRepository
    {
        Task<IEnumerable<NotificationChannel>> GetAllAsync();
        Task<NotificationChannel?> GetByIdAsync(int id);
        Task<NotificationChannel> CreateAsync(NotificationChannel entity);
        Task UpdateAsync(NotificationChannel entity);
        Task DeleteAsync(NotificationChannel entity);
    }
}
