using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuiteCoreBackend.Models.Entities
{
    [Table("notification_channel")]
    public class NotificationChannel
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column("type")]
        public string Type { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        [Column("bot_token")]
        public string BotToken { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("chat_id")]
        public string ChatId { get; set; } = string.Empty;

        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Required]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("updated_date")]
        public DateTime? UpdatedDate { get; set; }
    }
}
