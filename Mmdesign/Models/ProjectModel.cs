using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace Mmdesign.Models
{
    public class ProjectModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mã dự án.")]
        [Display(Name = "Mã dự án")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục.")]
        [Display(Name = "Thuộc nhóm")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên dự án.")]
        [Display(Name = "Tên dự án")]
        public string Name { get; set; }

        [Display(Name = "Từ khóa SEO")]
        public string Seo { get; set; }

        [Display(Name = "Từ khóa Keyword")]
        public string Keyword { get; set; }

        [Display(Name = "Phân loại")]
        public string Category { get; set; }

        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Display(Name = "Miêu tả")]
        public string Description { get; set; }

        [Display(Name = "Miêu tả ngắn gọn")]
        public string ShortDesc { get; set; }

        public DateTime Created { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày tạo dự án.")]
        [Display(Name = "Ngày tạo dự án")]
        public string CreatedDate
        {
            get
            {
                return Created.ToString("dd/MM/yyyy");
            }
            set
            {
                string dt = value.Substring(0, 10);
                Created = DateTime.ParseExact(dt, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
        }

        public bool? IsActive { get; set; }

        [Display(Name = "Nhà đầu tư")]
        public string Investor { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Diện tích đất")]
        public decimal? LandArea { get; set; }

        [Display(Name = "Diện tích xây dựng")]
        public decimal? ConstructionArea { get; set; }

        [Display(Name = "Năm hoàn thành")]
        public short? YearOfCompletion { get; set; }

        [Display(Name = "Kiến trúc sư")]
        public string Architect { get; set; }

        public string LandArea2 { get { return LandArea?.ToString("#,##"); } }
        public string ConstructionArea2 { get { return ConstructionArea?.ToString("#,##"); } }

        [Display(Name = "Lời giới thiệu")]
        public string Intro { get; set; }

        [AllowHtml]
        [Display(Name = "Nội dung giới thiệu")]
        public string IntroContent { get; set; }

        [Display(Name = "Lời giới thiệu 1")]
        public string Intro1 { get; set; }

        [AllowHtml]
        [Display(Name = "Nội dung giới thiệu 1")]
        public string Intro1Content { get; set; }

        [Display(Name = "Lời giới thiệu 2")]
        public string Intro2 { get; set; }

        [AllowHtml]
        [Display(Name = "Nội dung giới thiệu 2")]
        public string Intro2Content { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string Picture { get; set; }

        public HttpPostedFileBase ImagePicture { get; set; }

        [Display(Name = "Ảnh 1")]
        public string Picture1 { get; set; }

        public HttpPostedFileBase ImagePicture1 { get; set; }

        [Display(Name = "Ảnh 2")]
        public string Picture2 { get; set; }

        public HttpPostedFileBase ImagePicture2 { get; set; }

        [Display(Name = "Ảnh 3")]
        public string Picture3 { get; set; }

        public HttpPostedFileBase ImagePicture3 { get; set; }

        [Display(Name = "Ảnh 4")]
        public string Picture4 { get; set; }

        public HttpPostedFileBase ImagePicture4 { get; set; }

        [Display(Name = "Phân loại dự án theo class")]
        public string CategoryClasses { get; set; } // => Filter by class: {branch, coffee, design, photo ....}

        public int? OrderNo { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}