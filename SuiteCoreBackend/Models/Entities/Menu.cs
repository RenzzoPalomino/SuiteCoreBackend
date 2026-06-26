using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuiteCoreBackend.Models.Entities
{
    [Table("menus", Schema = "session")]
    public class Menu
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("block_id")]
        public int BlockId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("slug")]
        public string Slug { get; set; } = string.Empty;

        [Column("order")]
        public short Order { get; set; } = 0;

        [Column("is_public")]
        public bool IsPublic { get; set; } = false;

        [Column("active")]
        public bool Active { get; set; } = true;

        [ForeignKey(nameof(BlockId))]
        public MenuBlock Block { get; set; } = null!;

        public ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
    }
}
