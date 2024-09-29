using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using Demo_1_Ecommerce.Data;
using Demo_1_Ecommerce.Implementation;
using Demo_1_Ecommerce.Reposatories;
using Demo_1_Ecommerce;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Demo_1_Ecommerce
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var xmlPath = Path.Combine(AppContext.BaseDirectory, "Demo_1_Ecommerce.xml"); // Update this to match your project's XML file path

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddEndpointsApiExplorer(); // Required for minimal APIs
														// builder.Services.AddSwaggerGen(); // Register Swagger services
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "EasyShopper API",
					Version = "v1",
					Description = "This is the API documentation for the EasyShopper project. The API enables users to efficiently manage products, orders, and user accounts for a seamless shopping experience.",

					Contact = new OpenApiContact
					{
						Name = "Ahmed Mohamed Gaber",
						Email = "elhmzawy91@gmail.com",
						Url = new Uri("https://www.linkedin.com/in/ahmed-mohamed-gaber-65bb39238/")
					}
				});

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
					Type = SecuritySchemeType.ApiKey,
					Name = "Authorization",
					In = ParameterLocation.Header,
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new string[] {}
					}
				});

			});


			builder.Services.AddControllersWithViews();
			builder.Services.AddControllers()
			  .AddJsonOptions(options =>
			  {
				  options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
			  });

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			  .AddJwtBearer(options =>
			  {
				  options.TokenValidationParameters = new TokenValidationParameters
				  {
					  ValidateIssuer = true,
					  ValidateAudience = true,
					  ValidateLifetime = true,
					  ValidateIssuerSigningKey = true,
					  // Add your issuer and audience details here
				  };
			  });
			//builder.Services.AddMiniProfiler(options =>
			//{
			//	options.RouteBasePath = "/profiler"; // Ensure this matches your access path
			//	options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter(); // For database profiling
			//}).AddEntityFramework(); // For EF profiling





			builder.Services.AddDbContext<ApplicationDbContext>(option =>
			{
				option.UseSqlServer(builder.Configuration.GetConnectionString("cs"),
					b => b.MigrationsAssembly("Demo_1_Ecommerce"));

			});
			builder.Services.Configure<StripeData>(builder.Configuration.GetSection("stripe"));
			// builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddDefaultIdentity<IdentityUser>
				 (options => options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5)).AddDefaultUI()
				 .AddRoles<IdentityRole>()
				 .AddRoleManager<RoleManager<IdentityRole>>()
				 .AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddScoped<ApplicationDbContext>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWok>();
			builder.Services.AddSingleton<IEmailSender, EmailSender>();
			//builder.Services.AddScoped<IGeneircRepository<Category>,CategoryRepository>();
			builder.Services.AddDistributedMemoryCache();
			builder.Services.AddSession();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();
			StripeConfiguration.ApiKey = builder.Configuration.GetSection("stripe:Secretkey").Get<string>();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseSession();
			app.MapRazorPages();

			app.UseAuthentication();
			app.UseAuthorization();
			//app.UseMiniProfiler(); // Ensure this is in the correct order

			app.UseSwagger();


			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-commerce Apis V1");
				c.RoutePrefix = "swagger"; // This is the default

			});



			app.MapControllerRoute(
			name: "default",
			pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");

			app.MapControllerRoute(
				name: "Customer",
				pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");




			app.Run();
		}
	}
}
