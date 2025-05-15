using ASPNETCoreAuthLoginTest.Models;
using ASPNETCoreAuthLoginTest.Utils;
using ASPNETCoreAuthLoginTest.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


public class AccountController : Controller
{
    private readonly IAccountService _accountService; 

    public AccountController(IAccountService AccountService)
    {
        _accountService = AccountService;
    }

    [HttpGet]
    public IActionResult TeacherLogin()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TeacherLogin(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            var ValidateduserDto = _accountService.ValidateUser(model.UserID, model.Password,UserRole.Teacher);
            if (ValidateduserDto != null)
            {
                // 創建身份驗證 Cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, ValidateduserDto.UserName),
                    new Claim(ClaimTypes.Role, ValidateduserDto.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Teacher");
                }
            }
            ModelState.AddModelError(string.Empty, "用戶名或密碼不正確");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult StudentLogin()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StudentLogin(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            var user = _accountService.ValidateUser(model.UserID, model.Password, UserRole.Student);
            if (user != null)
            {
                // 創建身份驗證 Cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Dashboard", "Student");
                }
            }
            ModelState.AddModelError(string.Empty, "用戶名或密碼不正確");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // 檢查用戶名是否已存在
            if (_accountService.IsUserExists(model.Username))
            {
                ModelState.AddModelError("Username", "用戶名已存在");
                return View(model);
            }

            // 創建新用戶 (這裡是示例，實際中您需要儲存到數據庫)
            var result = _accountService.CreateUser(model.Username, model.Email, model.Password);
            if (result)
            {
                // 自動登入新用戶
                var user = _accountService.GetUserByUsername(model.Username,UserRole.Teacher);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.userID),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Authorize]
    public IActionResult Profile()
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrWhiteSpace(username))
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

        var user = _accountService.GetUserByUsername(username, role);
        if (user == null)
        {
            return View("AppError", new AppErrorViewModel
            {
                Title = "找不到使用者",
                Message = "找不到您的帳戶資料。",
                ReturnUrl = Url.Action("Index", "Home") ?? "/"
            });
        }

        var viewModel = new UserProfileViewModel
        {
            UserName = user.UserName,
            Role = user.Role
        };

        return View(viewModel);
    }


}