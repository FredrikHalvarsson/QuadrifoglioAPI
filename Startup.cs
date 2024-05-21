using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace QuadrifoglioAPI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins("https://localhost:7281")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });
            //services.AddControllersWithViews();

        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("AllowOrigin");

        }
    }
}
