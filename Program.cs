using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuadrifoglioAPI.Data;
using QuadrifoglioAPI.Models;
using QuadrifoglioAPI.Services;
using System;
using System.Text;
using System.Threading.Tasks;

namespace QuadrifoglioAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins("https://localhost:7281", "https://ilquadrifoglio.windows.net") //add new address form azure
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            // Add User Secrets
            builder.Configuration.AddUserSecrets<Program>();

            // Add Identity services to the container
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure cookie settings
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            // Register the no-op email sender
            builder.Services.AddTransient<IEmailSender, Services.NoOpEmailSender>();

            // Add DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add authorization services
            builder.Services.AddAuthorization();

            // Add controller services
            builder.Services.AddControllersWithViews();

            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add restaurant service
            builder.Services.AddScoped<RestaurantService>();

            // Get the Google Maps API key
            var googleMapsApiKey = builder.Configuration["GoogleMaps:ApiKey"];

            // Register the GoogleMapsService
            builder.Services.AddHttpClient<GoogleMapsService>(client =>
            {
                client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/");
            });

            builder.Services.AddTransient(provider => new GoogleMapsService(
                provider.GetRequiredService<HttpClient>(),
                googleMapsApiKey
            ));

            builder.Services.AddMvc(options =>
            {
                options.OutputFormatters.OfType<StringOutputFormatter>().FirstOrDefault()?.SupportedEncodings.Add(Encoding.UTF8);
            });

            var app = builder.Build();


            // Seed the database with initial data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    await SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                    throw;
                }
            }

            // Use CORS middleware
            app.UseCors("AllowOrigin");

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // Ensure authentication is used
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}