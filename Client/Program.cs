
using Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDistributedMemoryCache(); // Use a distributed cache for session data

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie configuration for authentication cookies
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax; 
});

builder.Services.AddSession(options =>
{
    // Session-specific configuration
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Set the session timeout
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // Set the MinimumSameSitePolicy property to SameSiteMode.Lax
    options.MinimumSameSitePolicy = SameSiteMode.Lax;

    options.Secure = CookieSecurePolicy.Always;
    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "SessionAuthenticationScheme"; // Set the default authentication scheme
    options.DefaultSignInScheme = "SessionAuthenticationScheme"; // Set the default sign-in scheme
    options.DefaultChallengeScheme = "SessionAuthenticationScheme"; // Set the default challenge scheme
})
    .AddCookie("SessionAuthenticationScheme", options =>
    {
        options.LoginPath = "/SignIn";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Error/Error403";
    });


builder.Services.AddHttpContextAccessor();

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Services
builder.Services.AddScoped<ClientService>();

// Transient

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseExceptionHandler("/Error");
app.UseStatusCodePagesWithRedirects("/Error/Error404");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
       name: "Admin",
       pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
   );

    endpoints.MapControllerRoute(
       name: "Staff",
       pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
   );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
        );
});

app.Run();
