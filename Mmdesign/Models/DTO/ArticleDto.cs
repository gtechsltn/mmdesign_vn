using System;

namespace Mmdesign.Models
{
    public class ArticleDto
    {
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