using System.ComponentModel.DataAnnotations.Schema;

namespace SuiteCoreBackend.Models.Entities
{
    public class HeaderMenu//
    {
        public int id { get; set; }
        public string? name { get; set; }
        public int order { get; set; }
        public bool active { get; set; }
        

    }
    public class Menus
    {
        public int id { get; set; }
        public int parentId { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public string? route { get; set; }
        public string? icon { get; set; }
    }

    [Table("Role")]
    public class Role//
    {
        public int id { get; set; }
        public string? gidNumber { get; set; }
        public string? name { get; set; }
        public bool active { get; set; }
    }

    //public class MenuRole
    //{
    //    public 
    //}
}
