using  ASPNETCoreAuthLoginTest.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using  ASPNETCoreAuthLoginTest.Auth;
using ASPNETCoreAuthLoginTest.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCustomCookieAuthentication();



// 注冊用戶服務，實際應用可能會使用 Scoped 生命週期
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // 添加身份驗證中間件
app.UseAuthorization(); // 添加授權中間件

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
