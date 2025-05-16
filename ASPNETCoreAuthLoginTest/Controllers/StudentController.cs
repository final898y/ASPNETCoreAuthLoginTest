using ASPNETCoreAuthLoginTest.Models;
using ASPNETCoreAuthLoginTest.Services;
using ASPNETCoreAuthLoginTest.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASPNETCoreAuthLoginTest.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly IUserService<StudentProfileViewModel> _studentService;

        public StudentController(IUserService<StudentProfileViewModel> teacherService)
        {
            _studentService = teacherService;
        }


        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var accountName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(accountName))
            {
                return View("AppError", new AppErrorViewModel
                {
                    Title = "使用者錯誤",
                    Message = "無法取得使用者名稱，請重新登入。",
                    ReturnUrl = Url.Action("Index", "Home") ?? "/"
                });
            }

            if (!User.TryGetUserRole(out var role))
            {
                return View("AppError", new AppErrorViewModel
                {
                    Title = "角色錯誤",
                    Message = "無法確認您的角色權限。",
                    ReturnUrl = Url.Action("Index", "Home") ?? "/"
                });
            }

            var user = _studentService.GetUserProfile(accountName);
            if (user == null)
            {
                return View("AppError", new AppErrorViewModel
                {
                    Title = "找不到使用者",
                    Message = "找不到您的帳戶資料。",
                    ReturnUrl = Url.Action("Index", "Home") ?? "/"
                });
            }

            var viewModel = new StudentProfileViewModel
            {
                UserName = user.UserName,
                Role = user.Role,
                AccountName = user.AccountName,
                Grade = user.Grade,
                Class = user.Class,
            };

            return View(viewModel);
        }
    }
}
