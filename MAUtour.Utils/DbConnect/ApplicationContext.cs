﻿using MAUtour.Utils.DbConnect.TestData;
using MAUtour.Utils.Models;

using Microsoft.EntityFrameworkCore;

using System.Reflection.Emit;

namespace MAUtour.Utils.DbConnect
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Roles> Roles { get; set; }

        public DbSet<UserPins> UserPins { get; set; }
        public DbSet<UserRoutes> UserRoutes { get; set; }
        public DbSet<PinsRoutes> PinRoutes { get; set; }
        public DbSet<TypeOfPins> PinTypes { get; set; }
        public DbSet<TypeOfRoutes> RouteTypes { get; set; }
        
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data source=test.db");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRoles>()
                .HasOne(p => p.Roles)
                .WithMany(c => c.UserRoles)
                .HasForeignKey(p=>p.RoleId);

            modelBuilder.Entity<UserRoles>()
                .HasOne(p => p.Users)
                .WithMany(c => c.Roles)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<UserPins>()
                .HasOne(p => p.Users)
                .WithMany(p => p.Pins)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<UserPins>()
                .HasOne(p => p.Type)
                .WithMany(p => p.Pins)
                .HasForeignKey(p => p.TypeId);

            modelBuilder.Entity<UserRoutes>()
                .HasOne(p => p.Users)
                .WithMany(p => p.Routes)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<UserRoutes>()
                .HasOne(p => p.Type)
                .WithMany(p => p.Routes)
                .HasForeignKey(p => p.TypeId);

            modelBuilder.Entity<PinsRoutes>()
                .HasOne(p => p.UserRoutes)
                .WithMany(p => p.Routes)
                .HasForeignKey(p => p.RouteId);

            modelBuilder.Entity<PinsRoutes>()
                .HasOne(p => p.UserPins)
                .WithMany(p => p.Pins)
                .HasForeignKey(p => p.PinId);

            modelBuilder.Entity<Users>().HasData(TestUserData.CreateUserData());
            modelBuilder.Entity<TypeOfPins>().HasData(TestPinsData.CreatePinTypes());
            modelBuilder.Entity<TypeOfRoutes>().HasData(TestRoutesData.CreateRouteTypes());
            modelBuilder.Entity<UserPins>().HasData(TestPinsData.CreatePinsData());
            modelBuilder.Entity<UserRoutes>().HasData(TestRoutesData.CreateRoutesData());
            modelBuilder.Entity<PinsRoutes>().HasData(TestRoutesData.CreatePinRoutesData());

            base.OnModelCreating(modelBuilder);
        }
    }
}
