using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApplication5.DTOs;
using WebApplication5.Options;

namespace WebApplication5.Services;

public interface ITokenService
{
    AuthResponseDTO CreateToken(CurrentUserDTO user);
}

public class TokenService(IOptions<JwtOptions> options, SymmetricSecurityKey signingKey) : ITokenService
{
    private readonly JwtOptions _options = options.Value;

    public AuthResponseDTO CreateToken(CurrentUserDTO user)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim("role", user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

        return new AuthResponseDTO
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt,
            User = user
        };
    }
}
