using loginTest.Models;
using loginTest.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

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
            // 驗證用戶登入信息 (這裡是示例，實際中您需要查詢數據庫)
            var ValidateduserDto = _accountService.ValidateUser(model.Username, model.Password);
            if (ValidateduserDto != null)
            {
                // 創建身份驗證 Cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, ValidateduserDto.Username),
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
            // 驗證用戶登入信息 (這裡是示例，實際中您需要查詢數據庫)
            var user = _accountService.ValidateUser(model.Username, model.Password);
            if (user != null)
            {
                // 創建身份驗證 Cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
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
                var user = _accountService.GetUserByUsername(model.Username);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
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
        // 獲取當前登入用戶信息
        var username = User.Identity.Name;
        var user = _accountService.GetUserByUsername(username);
        return View(user);
    }
}