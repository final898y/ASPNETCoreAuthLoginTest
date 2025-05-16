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
            var ValidateduserDto = _accountService.ValidateUser(model.AccountName, model.Password,UserRole.Teacher);
            if (ValidateduserDto != null)
            {
                // 創建身份驗證 Cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, ValidateduserDto.AccountName),
                    new Claim(ClaimTypes.Role, ValidateduserDto.Role),
                    new Claim(CustomClaimTypes.FullName,ValidateduserDto.UserName)
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
            var ValidateduserDto = _accountService.ValidateUser(model.AccountName, model.Password, UserRole.Student);
            if (ValidateduserDto != null)
            {
                // 創建身份驗證 Cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, ValidateduserDto.AccountName),
                    new Claim(ClaimTypes.Role, ValidateduserDto.Role),
                    new Claim(CustomClaimTypes.FullName,ValidateduserDto.UserName)
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
            if (_accountService.IsUserExists(model.AccountName))
            {
                ModelState.AddModelError("AccountName", "用戶名已存在");
                return View(model);
            }

            var result = _accountService.CreateUser(model.AccountName, model.Email, model.Password);
            if (result)
            {
                // 自動登入新用戶
                var user = _accountService.ValidateUser(model.AccountName, model.Password, UserRole.Student);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.AccountName),
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
}