using Instagram.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Instagram.Data;

public static class DataSeeder
{
    private static class DevAdmin
    {
        public const string RoleName = "Admin";
        public const string Email = "admin@Instagram.com";
        public const string UserName = "admin";
        public const string Password = "Admin123!";
    }

    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;
        var env = provider.GetRequiredService<IHostEnvironment>();
        if (!env.IsDevelopment())
            return;

        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = provider.GetRequiredService<UserManager<InstagramUser>>();
        var context = provider.GetRequiredService<InstagramContext>();

        await SeedAdminRoleAndUserAsync(roleManager, userManager, cancellationToken);

        if (await context.Posts.AnyAsync(cancellationToken))
            return;

        await Task.CompletedTask;
    }

    private static async Task SeedAdminRoleAndUserAsync(
        RoleManager<IdentityRole> roleManager,
        UserManager<InstagramUser> userManager,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var roleName = DevAdmin.RoleName;

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var created = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (!created.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to create role '{roleName}': {string.Join("; ", created.Errors.Select(e => e.Description))}");
            }
        }

        var email = DevAdmin.Email;
        var userName = DevAdmin.UserName;
        var password = DevAdmin.Password;

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new InstagramUser
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true,
                Name = "Administrator",
                Bio = string.Empty,
                Photo = string.Empty
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to create admin user: {string.Join("; ", createResult.Errors.Select(e => e.Description))}");
            }
        }
        else
        {
            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);
            }
        }

        if (!await userManager.IsInRoleAsync(user, roleName))
        {
            var addRole = await userManager.AddToRoleAsync(user, roleName);
            if (!addRole.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to assign user to role '{roleName}': {string.Join("; ", addRole.Errors.Select(e => e.Description))}");
            }
        }
    }
}
