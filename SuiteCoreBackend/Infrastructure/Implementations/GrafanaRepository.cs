using Microsoft.EntityFrameworkCore;
using SuiteCoreBackend.Infrastructure.Context;
using SuiteCoreBackend.Infrastructure.Interfaces;
using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Infrastructure.Implementations
{
    public class GrafanaRepository : IGrafanaRepository
    {
        private readonly SCDbContext _context;

        public GrafanaRepository(SCDbContext context)
        {
            _context = context;
        }

        public async Task<List<GrafanaPanel>?> GetAll()
        {
            return await _context.GrafanaPanels.ToListAsync();
        }
    }
}
