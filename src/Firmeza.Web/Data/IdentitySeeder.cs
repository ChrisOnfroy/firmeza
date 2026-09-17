using Firmeza.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace Firmeza.Web.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { "Administrator", "Customer" })
        {
            if (await roleManager.RoleExistsAsync(role))
                continue;

            var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
            EnsureSucceeded(roleResult, $"Could not create role '{role}'.");
        }

        var email = configuration["Admin:Email"];
        var password = configuration["Admin:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Admin credentials are not configured. Set Admin:Email and Admin:Password.");
        }

        var admin = await userManager.FindByEmailAsync(email);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var userResult = await userManager.CreateAsync(admin, password);
            EnsureSucceeded(userResult, "Could not create the administrator account.");
        }

        if (!await userManager.IsInRoleAsync(admin, "Administrator"))
        {
            var roleResult = await userManager.AddToRoleAsync(admin, "Administrator");
            EnsureSucceeded(roleResult, "Could not assign the Administrator role.");
        }
    }

    private static void EnsureSucceeded(IdentityResult result, string message)
    {
        if (result.Succeeded)
            return;

        var errors = string.Join("; ", result.Errors.Select(error => error.Description));
        throw new InvalidOperationException($"{message} {errors}");
    }
}
