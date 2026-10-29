using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApplication5.Options;

namespace WebApplication5.Auth;

public static class JwtSetup
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment, out bool usesTemporaryKey)
    {
        var section = configuration.GetSection(JwtOptions.SectionName);
        services.Configure<JwtOptions>(section);
        var options = section.Get<JwtOptions>() ?? new JwtOptions();

        var signingKey = CreateSigningKey(options.Key, environment, out usesTemporaryKey);
        services.AddSingleton(signingKey);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwt =>
            {
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = options.Issuer,
                    ValidateAudience = true,
                    ValidAudience = options.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ClockSkew = TimeSpan.FromSeconds(30),
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };

                jwt.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        var error = context.AuthenticateFailure switch
                        {
                            SecurityTokenExpiredException => "Срок действия токена истёк, войдите заново.",
                            not null => "Недействительный токен.",
                            _ => "Требуется вход в систему."
                        };
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new { error });
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsJsonAsync(new { error = "Недостаточно прав для этого действия." });
                    }
                };
            });

        return services;
    }

    private static SymmetricSecurityKey CreateSigningKey(string? key, IHostEnvironment environment, out bool temporary)
    {
        temporary = false;

        if (string.IsNullOrWhiteSpace(key))
        {
            if (!environment.IsDevelopment())
            {
                throw new InvalidOperationException(
                    "Не задан ключ подписи JWT (Jwt:Key). Укажите его в переменных окружения или секретах.");
            }

            temporary = true;
            return new SymmetricSecurityKey(RandomNumberGenerator.GetBytes(64));
        }

        var bytes = Encoding.UTF8.GetBytes(key);
        if (bytes.Length < 32)
        {
            throw new InvalidOperationException("Ключ подписи JWT (Jwt:Key) должен быть не короче 32 байт.");
        }

        return new SymmetricSecurityKey(bytes);
    }
}
