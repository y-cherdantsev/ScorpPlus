using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlusBackend.Models
{
    /// <summary>
    /// Access table representation
    /// </summary>
    [Table("accesses")]
    public sealed class Access
    {
        /// <summary>
        /// id field
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// employee_id field
        /// </summary>
        [Required]
        [ForeignKey(nameof(Employee))]
        [Column("employee_id")]
        public int EmployeeId { get; set; }

        /// <summary>
        /// room_id field
        /// </summary>
        [Required]
        [ForeignKey(nameof(Room))]
        [Column("room_id")]
        public int RoomId { get; set; }

        /// <summary>
        /// Employee reference
        /// </summary>
        public Employee Employee { get; set; }

        /// <summary>
        /// Room reference
        /// </summary>
        public Room Room { get; set; }
    }
}