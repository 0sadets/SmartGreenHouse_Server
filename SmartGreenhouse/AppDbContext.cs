﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartGreenhouse.Models.Entities;
using SmartGreenhouse.Models;

namespace SmartGreenhouse
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public AppDbContext() : base() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Greenhouse> Greenhouses { get; set; }
        public DbSet<SensorReading> SensorReadings { get; set; }
        public DbSet<DeviceState> DeviceStates { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<GreenhouseStatusRecord> GreenhouseStatuses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.SeedPlants();
        }
    }
}
