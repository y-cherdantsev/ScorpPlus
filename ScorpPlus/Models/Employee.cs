using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlus.Models
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

        public string GetBirthDate()
        {
            if (BirthDate != null)
                return
                    $"{BirthDate.Value.Day.ToString().PadLeft(2, '0')}.{BirthDate.Value.Month.ToString().PadLeft(2, '0')}.{BirthDate.Value.Year}";
            if (Iin == null) return null;
            var iin = Iin.Value.ToString().PadLeft(12, '0');
            var yearNum = int.Parse(iin.Substring(0, 2));
            var year = yearNum < 21 ? $"20{yearNum.ToString().PadLeft(2, '0')}" : $"19{yearNum}";
            return $"{iin.Substring(4, 2)}.{iin.Substring(2, 2)}.{year}";
        }

        public string GetIin() => Iin == null ? null : Iin.ToString().PadLeft(12, '0');

        public override string ToString() => $"{LastName} {FirstName} {MiddleName}".Trim();

        public bool IsInOffice()
        {
            try
            {
                if (!AccessHistories.Any()) return false;
                var lastEntry = AccessHistories.OrderByDescending(x => x.Relevance)
                    .FirstOrDefault(x => x.Room.Code == "entry");
                if (lastEntry == null) return false;
                return lastEntry.Status != null && (bool) lastEntry.Status;

            }
            catch (Exception)
            {
                throw new Exception("Include rooms into AccessHistory entities");
            }
        }
    }
}