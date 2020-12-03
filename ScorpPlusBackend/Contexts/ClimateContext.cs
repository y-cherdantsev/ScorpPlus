using ScorpPlusBackend.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlusBackend.Contexts
{
    /// <summary>
    /// Climate context for proceeding activities with climate information
    /// </summary>
    public class ClimateContext : DbContext
    {
        /// <summary>
        /// DbSet for Climate model
        /// </summary>
        public DbSet<Climate> ClimateList { get; set; }

        /// <inheritdoc />
        public ClimateContext(DbContextOptions<ClimateContext> options)
            : base(options)
        {
        }
    }
}