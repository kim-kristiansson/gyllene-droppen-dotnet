using GylleneDroppen.Core.Entities;
using GylleneDroppen.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting; // ← Use this instead

namespace GylleneDroppen.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var adminSettings = scope.ServiceProvider.GetRequiredService<IOptions<AdminSettings>>().Value;
        var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>(); // ← Changed this

        // Create roles if they don't exist
        await EnsureRoleExists(roleManager, "Admin");
        await EnsureRoleExists(roleManager, "User");

        Console.WriteLine($"Environment: {environment.EnvironmentName}");
        Console.WriteLine($"CreateAdminUser setting: {adminSettings.CreateAdminUser}");

        // Only create admin user in Development or if explicitly requested
        if (environment.IsDevelopment() || adminSettings.CreateAdminUser)
        {
            await CreateAdminUser(userManager, adminSettings, environment);
        }
        else
        {
            Console.WriteLine("ℹ️ Skipping admin user creation. Set AdminSettings:CreateAdminUser=true to enable.");
        }
    }

    private static async Task EnsureRoleExists(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
            Console.WriteLine($"✅ {roleName} role created");
        }
    }

    private static async Task CreateAdminUser(UserManager<ApplicationUser> userManager, AdminSettings adminSettings,
        IHostEnvironment environment)
    {
        var adminUser = adminSettings.AdminUser;

        // Validate required settings
        if (string.IsNullOrEmpty(adminUser.Email))
        {
            Console.WriteLine("❌ AdminSettings:AdminUser:Email is required. Skipping admin user creation.");
            return;
        }

        if (string.IsNullOrEmpty(adminUser.Password))
        {
            if (environment.IsDevelopment())
            {
                Console.WriteLine("⚠️ No password configured. Using default password for development.");
                // Create a new instance with default password
                adminUser = new AdminUserSettings
                {
                    Email = adminUser.Email,
                    Password = "Admin123!",
                    FirstName = adminUser.FirstName ?? "Admin",
                    LastName = adminUser.LastName ?? "User"
                };
            }
            else
            {
                Console.WriteLine(
                    "❌ AdminSettings:AdminUser:Password is required for production. Skipping admin user creation.");
                return;
            }
        }

        var existingUser = await userManager.FindByEmailAsync(adminUser.Email);

        if (existingUser == null)
        {
            var newUser = new ApplicationUser
            {
                UserName = adminUser.Email,
                Email = adminUser.Email,
                EmailConfirmed = true,
                FirstName = adminUser.FirstName,
                LastName = adminUser.LastName
            };

            var result = await userManager.CreateAsync(newUser, adminUser.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, "Admin");
                Console.WriteLine($"✅ Admin user created: {adminUser.Email}");

                // Only show password in development
                if (environment.IsDevelopment())
                {
                    Console.WriteLine($"🔑 Password: {adminUser.Password}");
                }
            }
            else
            {
                Console.WriteLine("❌ Failed to create admin user:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"   - {error.Description}");
                }
            }
        }
        else if (!await userManager.IsInRoleAsync(existingUser, "Admin"))
        {
            await userManager.AddToRoleAsync(existingUser, "Admin");
            Console.WriteLine($"✅ Admin role assigned to existing user: {adminUser.Email}");
        }
        else
        {
            Console.WriteLine($"ℹ️ Admin user already exists: {adminUser.Email}");
        }
    }
}