using Microsoft.EntityFrameworkCore;
using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Infraestucture.Context
{
    public class SCDbContext : DbContext
    {
        public SCDbContext(DbContextOptions<SCDbContext> options) : base(options)
        {
        }

        public DbSet<GrafanaPanel> GrafanaPanels { get; set; } = null!;
    }
}
