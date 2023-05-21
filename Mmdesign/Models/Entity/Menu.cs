using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mmdesign.Models
{
    [Table("Menus")]
    public class Menu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public int ParentId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Slug { get; set; }
        public string Params { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public bool IsHorizontal { get; set; }
        public System.DateTime? DateCreated { get; set; }
        public System.DateTime? DateUpdated { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? OrderNo { get; set; }
    }
}