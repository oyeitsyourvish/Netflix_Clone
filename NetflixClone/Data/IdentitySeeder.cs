using Microsoft.AspNetCore.Identity;
using NetflixClone.Models;

namespace NetflixClone.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider
                    .GetRequiredService<UserManager<ApplicationUser>>();

            // Create Admin role
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(
                    new IdentityRole("Admin"));
            }

            // Create User role
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(
                    new IdentityRole("User"));
            }

            // Admin account
            var adminEmail = "admin@netflixclone.com";

            var adminUser =
                await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    FullName = "Netflix Admin",
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(
                    adminUser,
                    "Admin@123");

                if (!result.Succeeded)
                {
                    throw new Exception(
                        "Failed to create admin user.");
                }
            }

            // Assign Admin role
            if (!await userManager.IsInRoleAsync(
                    adminUser,
                    "Admin"))
            {
                await userManager.AddToRoleAsync(
                    adminUser,
                    "Admin");
            }
        }
    }
}