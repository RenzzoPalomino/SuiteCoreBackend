using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuiteCoreBackend.Models.Entities
{
    [Table("UserActivity")]
    public class UserActivity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("sessionid")]
        public string? SessionId { get; set; }
        [Column("username")]
        public string? Username { get; set; }
        [Column("ipaddress")]
        public string? IpAddress { get; set; }
        [Column("startedat")]
        public DateTime StartedAt { get; set; }
        [Column("endedat")]
        public DateTime EndedAt { get; set; }
        [Column("lastactivityat")]
        public DateTime LastActivityAt { get; set; }
    }
}
