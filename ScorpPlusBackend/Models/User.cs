using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlusBackend.Models
{
    /// <summary>
    /// User table representation
    /// </summary>
    [Table("users")]
    public sealed class User
    {
        /// <summary>
        /// id field
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// username field
        /// </summary>
        [Required]
        [Column("username")]
        public string Username { get; set; }


        /// <summary>
        /// password field
        /// </summary>
        [Required]
        [Column("password")]
        public string Password { get; set; }

        /// <summary>
        /// email field
        /// </summary>
        [Column("email")]
        public string Email { get; set; }

        /// <summary>
        /// role_id field
        /// </summary>
        [ForeignKey(nameof(Role))]
        [Column("role_id")]
        public int RoleId { get; set; }

        /// <summary>
        /// Role reference
        /// </summary>
        public Role Role { get; set; }
    }

    /// <summary>
    /// Role table representation
    /// </summary>
    [Table("roles")]
    public sealed class Role
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
        /// users list
        /// </summary>
        public List<User> Users { get; set; }
    }
}