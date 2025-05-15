namespace ASPNETCoreAuthLoginTest.Models
{
    public class AppErrorViewModel
    {
        public string Title { get; set; } = "發生錯誤";
        public string Message { get; set; } = "請稍後再試，或聯絡系統管理員。";
        public string ReturnUrl { get; set; } = "/";
    }
}
