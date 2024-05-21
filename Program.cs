using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using QuadrifoglioAPI.Data;
using QuadrifoglioAPI.Models;
using QuadrifoglioAPI.Services;
using System.Text;

namespace QuadrifoglioAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins("https://localhost:7281")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });


            // Add User Secrets
            builder.Configuration.AddUserSecrets<Program>();

            //builder.Services.AddHttpClient<GoogleMapsService>();

            builder.Services.AddScoped<UserService>();


            // Add Identity services to the container
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            // Register the no-op email sender
            builder.Services.AddTransient<IEmailSender, Services.NoOpEmailSender>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add JWT authentication services
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            // Add authorization services
            builder.Services.AddAuthorization();

            // Add controller services
            //builder.Services.AddControllers();
            builder.Services.AddControllersWithViews();

            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


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


            var app = builder.Build();

            // Use CORS middleware
            app.UseCors("AllowOrigin");

            //// Map Identity API endpoints
            //app.MapIdentityApi<ApplicationUser>();

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
