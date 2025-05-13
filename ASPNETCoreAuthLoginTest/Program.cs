using loginTest.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using loginTest.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCustomCookieAuthentication();



// �`�U�Τ�A�ȡA������Υi��|�ϥ� Scoped �ͩR�g��
builder.Services.AddSingleton<IAccountService, AccountService>();


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

app.UseAuthentication(); // �K�[�������Ҥ�����
app.UseAuthorization(); // �K�[���v������

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
