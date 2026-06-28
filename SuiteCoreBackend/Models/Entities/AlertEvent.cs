using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuiteCoreBackend.Models.Entities
{
    [Table("alert_event")]
    public class AlertEvent
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("source")]
        public string Source { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("status")]
        public string Status { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [MaxLength(100)]
        [Column("instance")]
        public string? Instance { get; set; }

        [MaxLength(50)]
        [Column("severity")]
        public string? Severity { get; set; }

        [Required]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
