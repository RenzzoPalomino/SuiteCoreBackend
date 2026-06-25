using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuiteCoreBackend.Models.Entities
{
    [Table("menu_blocks", Schema = "session")]
    public class MenuBlock
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("order")]
        public short Order { get; set; } = 0;

        [Column("active")]
        public bool Active { get; set; } = true;

        public ICollection<Menu> Menus { get; set; } = new List<Menu>();
    }
}
