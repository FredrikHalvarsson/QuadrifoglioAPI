//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.DependencyInjection;
//using QuadrifoglioAPI.Data;
//using QuadrifoglioAPI.Models;

//namespace QuadrifoglioAPI
//{
//    public class Startup
//    {
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddCors(options =>
//            {
//                options.AddPolicy("AllowOrigin",
//                    builder => builder.WithOrigins("https://localhost:7281")
//                                      .AllowAnyMethod()
//                                      .AllowAnyHeader());
//            });
//            //services.AddControllersWithViews();
//            // Add Identity services
//            services.AddIdentity<ApplicationUser, IdentityRole>()
//                .AddEntityFrameworkStores<ApplicationDbContext>()
//                .AddDefaultTokenProviders();

//            // Configure cookie settings
//            services.ConfigureApplicationCookie(options =>
//            {
//                options.LoginPath = "/Account/Login";
//                options.LogoutPath = "/Account/Logout";
//                options.AccessDeniedPath = "/Account/AccessDenied";
//            });
//        }
//        public void Configure(IApplicationBuilder app)
//        {
//            app.UseCors("AllowOrigin");

//            app.UseRouting();

//            app.UseAuthentication();
//            app.UseAuthorization();

//        }
//    }
//}
