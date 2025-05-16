using System;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Data
{
    public class SchedulerDbContext : DbContext
    {
        public SchedulerDbContext(DbContextOptions<SchedulerDbContext> options) : base(options) { }

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<ScheduleDevice> ScheduleDevices { get; set; }
        public DbSet<ScheduleDeviceAttribute> ScheduleDeviceAttributes { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<ScheduleExecutionLog> ScheduleExecutionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Building 1--* Schedules
            modelBuilder.Entity<Building>()
                .HasMany(b => b.Schedules)
                .WithOne(s => s.Building)
                .HasForeignKey(s => s.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Building 1--* Devices
            modelBuilder.Entity<Building>()
                .HasMany(b => b.Devices)
                .WithOne(d => d.Building)
                .HasForeignKey(d => d.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Device 1--* ScheduleDevices
            modelBuilder.Entity<Device>()
                .HasMany(d => d.ScheduleDevices)
                .WithOne(sd => sd.Device)
                .HasForeignKey(sd => sd.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Schedule 1--* ScheduleDevices
            modelBuilder.Entity<Schedule>()
                .HasMany(s => s.ScheduleDevices)
                .WithOne(sd => sd.Schedule)
                .HasForeignKey(sd => sd.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);

            // ScheduleDevice 1--* ScheduleDeviceAttributes
            modelBuilder.Entity<ScheduleDevice>()
                .HasMany(sd => sd.Attributes)
                .WithOne(a => a.ScheduleDevice)
                .HasForeignKey(a => a.ScheduleDeviceId)
                .OnDelete(DeleteBehavior.Restrict);

            // ScheduleExecutionLog *--1 Schedule
            modelBuilder.Entity<ScheduleExecutionLog>()
                .HasOne(log => log.Schedule)
                .WithMany()
                .HasForeignKey(log => log.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Buildings
            modelBuilder.Entity<Building>().HasData(
                new Building { Id = 1, Name = "Building A" },
                new Building { Id = 2, Name = "Building B" },
                new Building { Id = 3, Name = "Building D" }

            );

            // Seed Devices
            modelBuilder.Entity<Device>().HasData(
                new Device { Id = 1, UnitName = "AC101", UnitType = "AirConditioner", BuildingId = 1, AccessToken = "RUa54OFG7cl5kATAi54Q" },
                new Device { Id = 2, UnitName = "AC102", UnitType = "AirConditioner", BuildingId = 2, AccessToken = "DEVICE_TOKEN_2" },
                new Device { Id = 3, UnitName = "AC103", UnitType = "AirConditioner", BuildingId = 3, AccessToken = "DEVICE_TOKEN_3" }


            );

            // Seed Schedule
            modelBuilder.Entity<Schedule>().HasData(
                new Schedule
                {
                    Id = 1,
                    ScheduleName = "Morning Cooling",
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(12, 0, 0),
                    SelectedDays = "Monday,Wednesday,Friday",
                    IsActive = true,
                    BuildingId = 1 // <-- assign BuildingId to Schedule
                }
            );

            // Seed ScheduleDevice
            modelBuilder.Entity<ScheduleDevice>().HasData(
                new ScheduleDevice { Id = 1, ScheduleId = 1, DeviceId = 1 },
                new ScheduleDevice { Id = 2, ScheduleId = 1, DeviceId = 2 }
            );

            // Seed ScheduleDeviceAttributes
            modelBuilder.Entity<ScheduleDeviceAttribute>().HasData(
                new ScheduleDeviceAttribute { Id = 1, AttributeKey = "temperature", AttributeValue = "22", ScheduleDeviceId = 1 },
                new ScheduleDeviceAttribute { Id = 2, AttributeKey = "fanSpeed", AttributeValue = "Medium", ScheduleDeviceId = 1 },
                new ScheduleDeviceAttribute { Id = 3, AttributeKey = "temperature", AttributeValue = "24", ScheduleDeviceId = 2 }
            );
        }
    }
}
