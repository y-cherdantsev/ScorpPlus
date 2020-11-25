using ScorpPlusBackend.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlusBackend.Contexts
{
    /// <summary>
    /// Employee context for proceeding activities with employees
    /// </summary>
    public sealed class AccessContext : DbContext
    {
        /// <summary>
        /// DbSet for Access model
        /// </summary>
        public DbSet<Access> Accesses { get; set; }

        /// <inheritdoc />
        public AccessContext(DbContextOptions<AccessContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}