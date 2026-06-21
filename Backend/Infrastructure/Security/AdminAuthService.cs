using System.Security.Cryptography;
using System.Text;

namespace Ecommerce.Infrastructure.Security;

public sealed class AdminAuthService
{
    private readonly IConfiguration _configuration;

    public AdminAuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string AdminUsername => GetSetting("Admin:Username", "ADMIN_USERNAME", "admin");

    public bool ValidateCredentials(string username, string password)
    {
        var expectedPassword = GetSetting("Admin:Password", "ADMIN_PASSWORD", "Admin123!");

        return FixedEquals(username, AdminUsername) && FixedEquals(password, expectedPassword);
    }

    public string CreateToken(string username)
    {
        var expiresAt = DateTimeOffset.UtcNow.AddHours(8).ToUnixTimeSeconds();
        var payload = $"{username}:{expiresAt}";
        var signature = Sign(payload);

        return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{payload}:{signature}"));
    }

    public bool ValidateToken(string? authorizationHeader)
    {
        if (string.IsNullOrWhiteSpace(authorizationHeader) ||
            !authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        try
        {
            var token = authorizationHeader["Bearer ".Length..].Trim();
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var parts = decoded.Split(':');

            if (parts.Length != 3 || !long.TryParse(parts[1], out var expiresAt))
            {
                return false;
            }

            if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > expiresAt)
            {
                return false;
            }

            return FixedEquals(parts[2], Sign($"{parts[0]}:{parts[1]}"));
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private string Sign(string value)
    {
        var secret = GetSetting("Admin:TokenSecret", "ADMIN_TOKEN_SECRET", "dev-secret-change-me");
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(value)));
    }

    private string GetSetting(string key, string environmentName, string fallback)
    {
        return _configuration[key] ?? Environment.GetEnvironmentVariable(environmentName) ?? fallback;
    }

    private static bool FixedEquals(string left, string right)
    {
        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);

        return CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }
}
