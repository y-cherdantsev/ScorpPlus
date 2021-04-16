using Microsoft.EntityFrameworkCore;
using ScorpPlus.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlus.Contexts
{
    /// <summary>
    /// Employee context for proceeding activities with employees
    /// </summary>
    public sealed class EmployeeContext : DbContext
    {
        /// <summary>
        /// DbSet for Employee model
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <inheritdoc />
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasIndex(x => x.Iin).IsUnique();
        }
    }
}