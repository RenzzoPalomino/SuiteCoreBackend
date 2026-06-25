using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SuiteCoreBackend.Models.Entities
{
    [Table("role_menus", Schema = "session")]
    [PrimaryKey(nameof(GidNumber), nameof(MenuId))]
    public class RoleMenu
    {
        [Required]
        [MaxLength(20)]
        [Column("gid_number")]
        public string GidNumber { get; set; } = string.Empty;

        [Required]
        [Column("menu_id")]
        public int MenuId { get; set; }

        [ForeignKey(nameof(MenuId))]
        public Menu Menu { get; set; } = null!;
    }
}
