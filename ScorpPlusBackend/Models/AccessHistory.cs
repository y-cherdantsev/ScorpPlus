using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlusBackend.Models
{
    /// <summary>
    /// Access history table representation
    /// </summary>
    [Table("access_history")]
    public class AccessHistory
    {
        /// <summary>
        /// id field
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// status field
        /// </summary>
        /// <code>
        /// true - IN;
        /// false - OUT;
        /// null - DENIED;
        /// </code>
        [Column("status")]
        public bool? Status { get; set; }

        /// <summary>
        /// device_id field
        /// </summary>
        [Required]
        [ForeignKey(nameof(Device))]
        [Column("device_id")]
        public int DeviceId { get; set; }

        /// <summary>
        /// room_id field
        /// </summary>
        [Required]
        [ForeignKey(nameof(Room))]
        [Column("room_id")]
        public int RoomId { get; set; }

        /// <summary>
        /// employee_id field
        /// </summary>
        [ForeignKey(nameof(Employee))]
        [Column("employee_id")]
        public int EmployeeId { get; set; }

        /// <summary>
        /// relevance field
        /// </summary>
        [Column("relevance")]
        public DateTime Relevance { get; set; }

        /// <summary>
        /// Device reference
        /// </summary>
        public Device Device { get; set; }

        /// <summary>
        /// Room reference
        /// </summary>
        public Room Room { get; set; }

        /// <summary>
        /// Employee reference
        /// </summary>
        public Employee Employee { get; set; }
    }
}