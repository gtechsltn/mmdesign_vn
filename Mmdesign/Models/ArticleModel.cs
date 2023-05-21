using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Mmdesign.Models
{
    public class ArticleModel
    {
        [Display(Name = "Tiêu đề bài viết:")]
        [Required(ErrorMessage = "Tiêu đề bài viết là trường bắt buộc phải nhập.")]
        public string Title { get; set; }

        [Display(Name = "Đường dẫn bài viết:")]
        [Required(ErrorMessage = "Đường dẫn bài viết là trường bắt buộc phải nhập.")]
        public string Slug { get; set; }

        [Display(Name = "Nội dung bài viết:")]
        [Required(ErrorMessage = "Nội dung bài viết là trường bắt buộc phải nhập.")]
        [AllowHtml]
        public string Content { get; set; }
    }
}