using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlusBackend.Models
{
    /// <summary>
    /// Employee table representation
    /// </summary>
    [Table("employees")]
    public sealed class Employee
    {
        /// <summary>
        /// id field
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// first_name field
        /// </summary>
        [Required]
        [Column("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// last_name field
        /// </summary>
        [Column("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// middle_name field
        /// </summary>
        [Column("middle_name")]
        public string MiddleName { get; set; }

        /// <summary>
        /// birth_date field
        /// </summary>
        [Column("birth_date")]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// iin field
        /// </summary>
        [Column("iin")]
        public long? Iin { get; set; }

        /// <summary>
        /// access rights list
        /// </summary>
        public List<Access> Accesses { get; set; }

        /// <summary>
        /// access histories list
        /// </summary>
        public List<AccessHistory> AccessHistories { get; set; }
    }
}