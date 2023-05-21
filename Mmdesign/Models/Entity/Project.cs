using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mmdesign.Models.Entity
{
    [Table("Projects")]
    public class Project
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Seo { get; set; }
        public string Keyword { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDesc { get; set; }
        public DateTime Created { get; set; }
        public bool? IsActive { get; set; }
        public string Investor { get; set; }
        public string Address { get; set; }
        public decimal? LandArea { get; set; }
        public decimal? ConstructionArea { get; set; }
        public short? YearOfCompletion { get; set; }
        public string Architect { get; set; }
        public string Intro { get; set; }
        public string IntroContent { get; set; }
        public string Intro1 { get; set; }
        public string Intro1Content { get; set; }
        public string Intro2 { get; set; }
        public string Intro2Content { get; set; }
        public string Picture { get; set; }
        public string Picture1 { get; set; }
        public string Picture2 { get; set; }
        public string Picture3 { get; set; }
        public string Picture4 { get; set; }
        public string CategoryClasses { get; set; }
        public int? OrderNo { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }        
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}