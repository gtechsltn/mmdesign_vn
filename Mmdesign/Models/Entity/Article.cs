using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mmdesign.Models
{
    [Table("Articles")]
    public class Article
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Slug { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Published { get; set; }
        public Guid? AuthorId { get; set; }
        public System.DateTime? DateCreated { get; set; }
        public System.DateTime? DateUpdated { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? OrderNo { get; set; }
    }
}