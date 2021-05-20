using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScorpPlus.Models
{
    /// <summary>
    /// Climate table representation
    /// </summary>
    [Table("accurate_temperature_history")]
    public class AccurateTemperatureHistory
    {
        /// <summary>
        /// id field
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// temperature field
        /// </summary>
        [Column("temperature")]
        public double Temperature { get; set; }

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
        [ForeignKey(nameof(Room))]
        [Column("room_id")]
        public int RoomId { get; set; }

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
    }
}