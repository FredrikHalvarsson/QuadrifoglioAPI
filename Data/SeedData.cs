using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using QuadrifoglioAPI.Models;
using System;
using System.Threading.Tasks;

namespace QuadrifoglioAPI.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "admin", "customer" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create an admin user if it doesn't exist
            ApplicationUser user = await userManager.FindByEmailAsync("admin@admin.com");

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com"
                };

                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}
