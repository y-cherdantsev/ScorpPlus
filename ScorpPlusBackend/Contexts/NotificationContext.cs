using ScorpPlusBackend.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlusBackend.Contexts
{
    /// <summary>
    /// Notification context for proceeding activities with notifications
    /// </summary>
    public class NotificationContext : DbContext
    {
        /// <summary>
        /// DbSet for User model
        /// </summary>
        public DbSet<User> Users { get; set; }
        
        /// <summary>
        /// DbSet for TelegramChat model
        /// </summary>
        public DbSet<TelegramChat> TelegramChats { get; set; }

        /// <inheritdoc />
        public NotificationContext(DbContextOptions<NotificationContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TelegramChat>().HasIndex(x => x.UserId).IsUnique();
            modelBuilder.Entity<TelegramChat>().HasIndex(x => x.ChatId).IsUnique();

            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        }
    }
}