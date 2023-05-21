namespace Mmdesign.Models
{
    public class ProjectImageModel
    {
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsActive { get; set; }
        public int? OrderNo { get; set; }
        public int RowNumber { get; set; }
        public string Description { get; set; }
        public System.DateTime? CreatedOn { get; set; }
        public System.DateTime? UpdatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}