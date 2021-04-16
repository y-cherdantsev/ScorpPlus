using ScorpPlus.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlus.Contexts
{
    /// <summary>
    /// Room context for proceeding activities with rooms
    /// </summary>
    public class RoomContext : DbContext
    {
        /// <summary>
        /// DbSet for Room model
        /// </summary>
        public DbSet<Room> Rooms { get; set; }

        /// <inheritdoc />
        public RoomContext(DbContextOptions<RoomContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().HasIndex(x => x.Code).IsUnique();
        }
    }
}