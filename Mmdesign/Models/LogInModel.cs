using System.ComponentModel.DataAnnotations;

namespace Mmdesign.Models
{
    public class LogInModel
    {
        [Required]
        [Display(Name = "Tên người dùng")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Tự động đăng nhập")]
        public bool RememberMe { get; set; }
    }
}