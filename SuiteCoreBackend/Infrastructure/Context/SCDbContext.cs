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
        public DbSet<UserActivity> UserActivities { get; set; } = null!;
        public DbSet<Menu> Menus { get; set; } = null!;
        public DbSet<MenuBlock> MenuBlocks { get; set; } = null!;
        public DbSet<RoleMenu> RoleMenus { get; set; } = null!;
        

        /// <summary>
        /// M�todo para configurar el modelo de datos. Aqu� puedes definir las relaciones, restricciones y otras configuraciones para tus entidades.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GrafanaPanel>();
            modelBuilder.Entity<UserActivity>();

            modelBuilder.Entity<Menu>();
            modelBuilder.Entity<MenuBlock>();
            modelBuilder.Entity<RoleMenu>(entity =>
            {
                entity.Property(e => e.ModifiedAt)
                    .HasDefaultValueSql("(NOW() - INTERVAL '5 hours')");

                entity.Property(e => e.Active)
                    .HasDefaultValue(true);
            });


             
        }
    }
}
