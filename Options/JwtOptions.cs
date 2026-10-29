namespace WebApplication5.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string? Key { get; set; }

    public int AccessTokenMinutes { get; set; } = 60;
}
