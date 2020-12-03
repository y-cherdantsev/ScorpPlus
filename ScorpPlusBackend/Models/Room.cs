using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlusBackend.Models
{
    /// <summary>
    /// Room table representation
    /// </summary>
    [Table("rooms")]
    public class Room
    {
        /// <summary>
        /// id field
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// code field
        /// </summary>
        [Required]
        [Column("code")]
        public string Code { get; set; }

        /// <summary>
        /// description field
        /// </summary>
        [Column("description")]
        public string Description { get; set; }

        /// <summary>
        /// access rights list
        /// </summary>
        public List<Access> Accesses { get; set; }

        /// <summary>
        /// devices list
        /// </summary>
        public List<Device> Devices { get; set; }

        /// <summary>
        /// climate list
        /// </summary>
        public List<Climate> ClimateList { get; set; }
    }
}