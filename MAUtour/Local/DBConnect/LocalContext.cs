using MAUtour.Local.DBConnect.DictData;
using MAUtour.Local.Models;
using Microsoft.EntityFrameworkCore;

namespace MAUtour.Local.DBConnect
{
    public class LocalContext : DbContext
    {
        public DbSet<Pins> LocalPins { get; set; }
        public DbSet<Routes> LocalRoutes { get; set; }
        public DbSet<PinTypes> LocalPinsTypes { get; set; }
        public DbSet<RouteTypes> LocalRouteTypes { get; set; }
        public DbSet<RoutePins> LocalRoutesPins { get; set; }

        public LocalContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData)}\\local.db");
            base.OnConfiguring(optionsBuilder);
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

            modelBuilder.Entity<RoutePins>()
                .HasOne(p => p.Routes)
                .WithMany(p => p.Pins)
                .HasForeignKey(p => p.RoutesId);
            
            modelBuilder.Entity<PinTypes>().HasData(DictionaryData.GetPinTypes());
            modelBuilder.Entity<RouteTypes>().HasData(DictionaryData.GetRouteTypes());

            base.OnModelCreating(modelBuilder);
        }
    }
}
