﻿using Microsoft.EntityFrameworkCore;
using ScorpPlus.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlus.Contexts
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

        /// <summary>
        /// DbSet for Access model
        /// </summary>
        public DbSet<AccessHistory> AccessHistories { get; set; }

        /// <inheritdoc />
        public AccessContext(DbContextOptions<AccessContext> options)
            : base(options)
        {
        }
    }
}