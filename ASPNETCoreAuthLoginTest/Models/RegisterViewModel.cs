using System.ComponentModel.DataAnnotations;

namespace ASPNETCoreAuthLoginTest.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "請輸入用戶名")]
        [Display(Name = "用戶名")]
        public required string AccountName { get; set; }

        [Required(ErrorMessage = "請輸入郵箱")]
        [EmailAddress(ErrorMessage = "郵箱格式不正確")]
        [Display(Name = "郵箱")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(100, ErrorMessage = "{0} 必須至少包含 {2} 個字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "確認密碼")]
        [Compare("Password", ErrorMessage = "密碼和確認密碼不匹配。")]
        public required string ConfirmPassword { get; set; }
    }
}
