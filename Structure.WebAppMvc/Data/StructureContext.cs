using Microsoft.EntityFrameworkCore;
using Structure.WebAppMvc.Models;

namespace Structure.WebAppMvc.Data
{
    public class StructureContext : DbContext
    {
        public StructureContext(DbContextOptions<StructureContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("structure");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StructureContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Location> Location { get; set; } = default!;
    }
}