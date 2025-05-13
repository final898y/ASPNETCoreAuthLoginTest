// 📄 Auth/CookieAuthConfigurator.cs
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace loginTest.Auth
{
    public static class CookieAuthConfigurator
    {
        public static void AddCustomCookieAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    // 預設登入頁
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    // 自訂未授權導向邏輯
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                        {
                            var requestPath = context.Request.Path;

                            string loginUrl = requestPath.StartsWithSegments("/Teacher", StringComparison.OrdinalIgnoreCase)
                                ? "/Account/TeacherLogin"
                                : "/Account/StudentLogin";

                            var returnUrl = context.Request.Path + context.Request.QueryString;
                            var redirectUrl = loginUrl + "?returnUrl=" + Uri.EscapeDataString(returnUrl);

                            context.Response.Redirect(redirectUrl);
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
