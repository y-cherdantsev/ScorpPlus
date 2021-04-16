using Microsoft.EntityFrameworkCore;
using ScorpPlus.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlus.Contexts
{
    /// <summary>
    /// Device context for proceeding activities with devices
    /// </summary>
    public sealed class DeviceContext : DbContext
    {
        /// <summary>
        /// DbSet for Device model
        /// </summary>
        public DbSet<Device> Devices { get; set; }

        /// <summary>
        /// DbSet for DeviceType model
        /// </summary>
        public DbSet<DeviceType> DeviceTypes { get; set; }

        /// <inheritdoc />
        public DeviceContext(DbContextOptions<DeviceContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeviceType>().HasIndex(x => x.Code).IsUnique();

            modelBuilder.Entity<Device>().HasIndex(x => x.Code).IsUnique();
        }
    }
}