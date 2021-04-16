using Microsoft.EntityFrameworkCore;
using ScorpPlus.Models;

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

        /// <inheritdoc />
        public ClimateContext(DbContextOptions<ClimateContext> options)
            : base(options)
        {
        }
    }
}