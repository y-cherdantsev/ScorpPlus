using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlusBackend.Models
{
    /// <summary>
    /// Device table representation
    /// </summary>
    [Table("devices")]
    public sealed class Device
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
        /// type_id field
        /// </summary>
        [Required]
        [ForeignKey(nameof(Type))]
        [Column("type_id")]
        public int TypeId { get; set; }

        /// <summary>
        /// description field
        /// </summary>
        [Column("description")]
        public string Description { get; set; }


        /// <summary>
        /// type_id field
        /// </summary>
        [Required]
        [ForeignKey(nameof(Room))]
        [Column("room_id")]
        public int RoomId { get; set; }

        /// <summary>
        /// DeviceType reference
        /// </summary>
        public DeviceType Type { get; set; }

        /// <summary>
        /// Room reference
        /// </summary>
        public Room Room { get; set; }

        /// <summary>
        /// climate list
        /// </summary>
        public List<Climate> ClimateList { get; set; }
    }

    /// <summary>
    /// Device Type table representation
    /// </summary>
    [Table("device_types")]
    public sealed class DeviceType
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
        /// devices list
        /// </summary>
        public List<Device> Devices { get; set; }
    }
}