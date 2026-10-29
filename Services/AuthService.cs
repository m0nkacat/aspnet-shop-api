using Microsoft.AspNetCore.Identity;
using WebApplication5.DTOs;
using WebApplication5.Models;
using WebApplication5.Repositories;

namespace WebApplication5.Services;

public interface IAuthService
{
    Task<ServiceResult<AuthResponseDTO>> RegisterAsync(RegisterDTO dto);

    Task<ServiceResult<AuthResponseDTO>> LoginAsync(LoginDTO dto);

    Task<CurrentUserDTO?> GetCurrentUserAsync(int userId);
}

public class AuthService(IUserRepository users, IPasswordHasher<User> passwordHasher, ITokenService tokens) : IAuthService
{
    public const string DefaultRole = "User";

    public async Task<ServiceResult<AuthResponseDTO>> RegisterAsync(RegisterDTO dto)
    {
        if (await users.GetByUsernameAsync(dto.Username) is not null)
        {
            return ServiceResult<AuthResponseDTO>.Fail(ServiceStatus.Conflict, $"Логин «{dto.Username}» уже занят.");
        }

        var role = await users.GetRoleByNameAsync(DefaultRole);
        if (role is null)
        {
            return ServiceResult<AuthResponseDTO>.Fail(ServiceStatus.Invalid, $"В базе нет роли «{DefaultRole}».");
        }

        var user = new User { Username = dto.Username, RoleId = role.RoleId, IsActive = true };
        user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);

        await users.AddAsync(user);
        await users.SaveChangesAsync();

        return ServiceResult<AuthResponseDTO>.Ok(tokens.CreateToken(ToCurrentUser(user, role.RoleName)));
    }

    public async Task<ServiceResult<AuthResponseDTO>> LoginAsync(LoginDTO dto)
    {
        var user = await users.GetByUsernameAsync(dto.Username);
        if (user is null || !VerifyPassword(user, dto.Password))
        {
            return ServiceResult<AuthResponseDTO>.Fail(ServiceStatus.Unauthorized, "Неверный логин или пароль.");
        }

        if (!user.IsActive)
        {
            return ServiceResult<AuthResponseDTO>.Fail(ServiceStatus.Forbidden, "Учётная запись заблокирована.");
        }

        return ServiceResult<AuthResponseDTO>.Ok(tokens.CreateToken(ToCurrentUser(user, user.Role?.RoleName)));
    }

    public async Task<CurrentUserDTO?> GetCurrentUserAsync(int userId)
    {
        var user = await users.GetByIdAsync(userId);
        return user is null ? null : ToCurrentUser(user, user.Role?.RoleName);
    }

    private bool VerifyPassword(User user, string password)
    {
        if (string.IsNullOrEmpty(user.PasswordHash))
        {
            return false;
        }

        try
        {
            return passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) != PasswordVerificationResult.Failed;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static CurrentUserDTO ToCurrentUser(User user, string? roleName) => new()
    {
        UserId = user.UserId,
        Username = user.Username ?? string.Empty,
        Role = roleName ?? DefaultRole
    };
}
