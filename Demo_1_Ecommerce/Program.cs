//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Demo_1_Ecommerce.Data;
//using Microsoft.AspNetCore.Authentication.Cookies;

//namespace Demo_1_Ecommerce
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Configure the DbContext with SQL Server
//            builder.Services.AddDbContext<ApplicationDbContext>(options =>
//                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging());

//            // Configure Identity services
//            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
//                .AddEntityFrameworkStores<ApplicationDbContext>()
//                .AddDefaultTokenProviders();

//            // Add services to the container.
//            builder.Services.AddControllersWithViews();
//            builder.Services.AddRazorPages(); // Add this line

//            // Configure authentication and authorization
//            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//                   .AddCookie(options =>
//                   {
//                       options.LoginPath = "/Account/Login";
//                       options.AccessDeniedPath = "/Account/AccessDenied";
//                   });


//            builder.Services.AddAuthorization(options =>
//            {
//                options.AddPolicy("AdminOnly", policy =>
//                    policy.RequireRole("Admin"));
//            });

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (!app.Environment.IsDevelopment())
//            {
//                app.UseExceptionHandler("/Home/Error");
//                app.UseHsts();
//            }

//            app.UseHttpsRedirection();
//            app.UseStaticFiles();

//            app.UseRouting();


//            app.UseAuthentication();  
//            app.UseAuthorization();

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllerRoute(
//                    name: "default",
//                    pattern: "{controller=Home}/{action=Index}/{id?}");

//                //endpoints.MapControllerRoute(
//                //    name: "admin",
//                //    pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}");

//                //// Map Razor Pages (if used)
//                //endpoints.MapRazorPages();
//            });

//            app.Run();
//        }
//    }
//}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Demo_1_Ecommerce.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Demo_1_Ecommerce.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure the DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging());

// Configure Identity services
builder.Services.AddIdentity<User, Role>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configure authentication and authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
            name: "admin_login",
            pattern: "admin/login/index",
            defaults: new { controller = "Login", action = "Index" });
});

app.Run();

