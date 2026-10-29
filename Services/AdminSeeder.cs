using Microsoft.AspNetCore.Identity;
using WebApplication5.Models;
using WebApplication5.Repositories;

namespace WebApplication5.Services;

public static class AdminSeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration, ILogger logger)
    {
        var username = configuration["Seed:AdminUsername"];
        var password = configuration["Seed:AdminPassword"];
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        using var scope = services.CreateScope();
        var users = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        if (await users.GetByUsernameAsync(username) is not null)
        {
            return;
        }

        var role = await users.GetRoleByNameAsync("Admin");
        if (role is null)
        {
            logger.LogWarning("Роль Admin не найдена, администратор не создан.");
            return;
        }

        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
        var admin = new User { Username = username, RoleId = role.RoleId, IsActive = true };
        admin.PasswordHash = hasher.HashPassword(admin, password);

        await users.AddAsync(admin);
        await users.SaveChangesAsync();
        logger.LogInformation("Создан администратор {Username}.", username);
    }
}
