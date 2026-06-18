using Microsoft.EntityFrameworkCore;
using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Infrastructure.Context
{
    public class SCDbContext : DbContext
    {
        public SCDbContext(DbContextOptions<SCDbContext> options) : base(options)
        {
        }

        public DbSet<GrafanaPanel> GrafanaPanels { get; set; } = null!;
    }
}
