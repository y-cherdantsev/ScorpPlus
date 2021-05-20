using ScorpPlus.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlus.Contexts
{
    /// <summary>
    /// Climate context for proceeding activities with climate information
    /// </summary>
    public class ClimateContext : DbContext
    {
        /// <summary>
        /// DbSet for Climate model
        /// </summary>
        public DbSet<ClimateHistory> ClimateHistories { get; set; }

        /// <summary>
        /// DbSet for AccurateTemperature model
        /// </summary>
        public DbSet<AccurateTemperatureHistory> AccurateTemperatureHistories { get; set; }

        /// <summary>
        /// DbSet for WaterLevel model
        /// </summary>
        public DbSet<WaterLevelHistory> WaterLevelHistories { get; set; }

        /// <inheritdoc />
        public ClimateContext(DbContextOptions<ClimateContext> options)
            : base(options)
        {
        }
    }
}