using System.ComponentModel.DataAnnotations;

namespace loginTest.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "請輸入用戶名")]
        [Display(Name = "用戶名")]
        public string Username { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Display(Name = "記住我")]
        public bool RememberMe { get; set; }
    }
}
