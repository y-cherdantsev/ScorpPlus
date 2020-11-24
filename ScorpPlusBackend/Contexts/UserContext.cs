using ScorpPlusBackend.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlusBackend.Contexts
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
            Database.EnsureCreated();
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();

            modelBuilder.Entity<Role>().HasIndex(x => x.Code).IsUnique();
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Code = "admin"
                },
                new Role
                {
                    Id = 2,
                    Code = "manager"
                },
                new Role
                {
                    Id = 3,
                    Code = "guest"
                });
        }
    }
}