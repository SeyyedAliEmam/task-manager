using Hangfire;
using MemberShip.Infrastructure.Extensions;
using MemberShip.Web.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddMembershipInfrastructure(builder.Configuration);

#region CORS (برای React + Cookie Auth)
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // دقیقاً همین origin
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // برای کوکی لازم است
    });
});
#endregion

#region Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireAssertion(context =>
        {
            var isAdminClaim = context.User
                .FindFirst("IsAdmin")?
                .Value;

            return bool.TryParse(
                       isAdminClaim,
                       out var isAdmin)
                   && isAdmin;
        });
    });
});
#endregion

#region Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Account/Login";
        option.LogoutPath = "/Account/Logout";
        option.ExpireTimeSpan = TimeSpan.FromDays(30);
        // پیشنهاد: option.SlidingExpiration = true;
    });
#endregion

var app = builder.Build();

#region چک کردن روزانه تاریخ تولد کاربر
app.UseHangfireDashboard();
app.UseHangfireServer();
RecurringJob.AddOrUpdate<SendEmailController>
(
    "CheckBirthdays",
    x => x.SendBirthdayEmail(),
    Cron.Daily
);
#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// خیلی مهم: CORS باید اینجا باشد
app.UseCors("ReactPolicy");

app.UseAuthentication();
app.UseAuthorization();

#region مدیریت دسترسی های کاربر
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/Admin/UserManagement"))
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            context.Response.Redirect("/Account/Login");
            return;
        }

        var isAdminClaim = context.User.FindFirst("IsAdmin");
        bool isAdmin = isAdminClaim != null && bool.Parse(isAdminClaim.Value);

        if (!isAdmin)
        {
            context.Response.Redirect("/Home/Index");
            return;
        }
    }

    await next(); // اینجا await بهتر و استانداردتره
});
#endregion

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

