using System;
using System.ComponentModel.DataAnnotations;

namespace SuiteCoreBackend.DTOs.Notification
{
    public class NotificationChannelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string BotToken { get; set; } = string.Empty;
        public string ChatId { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateNotificationChannelDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string BotToken { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ChatId { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }

    public class UpdateNotificationChannelDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string BotToken { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ChatId { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }

    public class TestNotificationDirectDto
    {
        [Required]
        [MaxLength(500)]
        public string BotToken { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ChatId { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Name { get; set; }
    }
}
