using Microsoft.EntityFrameworkCore;
using SuiteCoreBackend.Infrastructure.Context;
using SuiteCoreBackend.Infrastructure.Interfaces;
using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Infrastructure.Implementations
{
    public class NotificationChannelRepository : INotificationChannelRepository
    {
        private readonly SCDbContext _context;

        public NotificationChannelRepository(SCDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NotificationChannel>> GetAllAsync()
        {
            return await _context.NotificationChannels.ToListAsync();
        }

        public async Task<NotificationChannel?> GetByIdAsync(int id)
        {
            return await _context.NotificationChannels.FindAsync(id);
        }

        public async Task<NotificationChannel> CreateAsync(NotificationChannel entity)
        {
            _context.NotificationChannels.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(NotificationChannel entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(NotificationChannel entity)
        {
            _context.NotificationChannels.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
