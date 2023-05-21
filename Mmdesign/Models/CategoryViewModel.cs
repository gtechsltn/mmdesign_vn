using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mmdesign.Models
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mã danh mục.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục cha.")]
        [DisplayName("Danh mục cha")]
        public int ParentId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục.")]
        [DisplayName("Tên danh mục")]
        public string Name { get; set; }

        [DisplayName("Miêu tả danh mục")]
        public string Description { get; set; }

        public bool IsActive { get; set; }
        public int? OrderNo { get; set; }
    }
}