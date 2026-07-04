using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuiteCoreBackend.Models.Entities
{
    [Table("grafanapanels")]
    public class GrafanaPanel
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("dashboarduid")]
        public string DashboardUid { get; set; } = string.Empty;

        [Required]
        [Column("panelid")]
        public int PanelId { get; set; }

        [Required]
        [Column("category")]
        public string Category { get; set; } = string.Empty;
    }
}
