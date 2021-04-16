using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ScorpPlus.Models
{
    /// <summary>
    /// Telegram chats table representation
    /// </summary>
    [Table("telegram_chats")]
    public class TelegramChat
    {
        /// <summary>
        /// id field
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// user_id field
        /// </summary>
        [Required]
        [ForeignKey(nameof(User))]
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// chat_id field
        /// </summary>
        [Required]
        [Column("chat_id")]
        public long ChatId { get; set; }

        /// <summary>
        /// User reference
        /// </summary>
        public User User { get; set; }
    }
}