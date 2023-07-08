using MAUtour.Local.Models;
using Microsoft.EntityFrameworkCore;

namespace MAUtour.Local.DBConnect
{
    internal class LocalContext : DbContext
    {
        public DbSet<Pins> LocalPins { get; set; }
        public DbSet<Routes> LocalRoutes { get; set; }
        public DbSet<PinTypes> LocalPinsTypes { get; set; }
        public DbSet<RouteTypes> LocalRouteTypes { get; set; }

        public LocalContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Database=localdb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Pins>()
                .HasOne(p => p.PinType)
                .WithMany(p => p.Pins)
                .HasForeignKey(p => p.PinTypeId);

            modelBuilder.Entity<Routes>()
                .HasOne(p => p.RouteType)
                .WithMany(p => p.Routes)
                .HasForeignKey(p => p.RouteTypeId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
