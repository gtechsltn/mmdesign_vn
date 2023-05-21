using System;

namespace Mmdesign.Models
{
    public class MenuViewModel
    {
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

        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}