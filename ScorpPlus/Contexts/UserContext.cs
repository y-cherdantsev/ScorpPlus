using ScorpPlus.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlus.Contexts
{
    /// <summary>
    /// User context for proceeding activities with users
    /// </summary>
    public sealed class UserContext : DbContext
    {
        /// <summary>
        /// DbSet for User model
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// DbSet for Role model
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <inheritdoc />
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasIndex(x => x.Code).IsUnique();

            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        }
    }
}