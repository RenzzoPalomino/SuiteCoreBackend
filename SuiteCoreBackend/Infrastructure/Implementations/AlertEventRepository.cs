using System.Threading.Tasks;
using SuiteCoreBackend.Infrastructure.Context;
using SuiteCoreBackend.Infrastructure.Interfaces;
using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Infrastructure.Implementations
{
    public class AlertEventRepository : IAlertEventRepository
    {
        private readonly SCDbContext _context;

        public AlertEventRepository(SCDbContext context)
        {
            _context = context;
        }

        public async Task<AlertEvent> CreateAsync(AlertEvent alertEvent)
        {
            _context.AlertEvents.Add(alertEvent);
            await _context.SaveChangesAsync();
            return alertEvent;
        }
    }
}
