using LifeOS.Application.Abstractions.Security;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace LifeOS.Infra.Data.Security;

public sealed class GoogleAuthService(IConfiguration configuration) : IGoogleAuthService
{
    private readonly IConfiguration _configuration = configuration;

    public async Task<GoogleAuthResult> ValidateIdTokenAsync(string idToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(idToken))
        {
            throw new InvalidOperationException("Google ID token is missing.");
        }

        var google = _configuration.GetSection("Google");
        var clientId = google["ClientId"] ?? throw new InvalidOperationException("Google:ClientId is not configured.");

        var payload = await GoogleJsonWebSignature.ValidateAsync(
            idToken,
            new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [clientId]
            });

        if (!payload.EmailVerified)
        {
            throw new InvalidOperationException("Google email is not verified.");
        }

        var googleId = payload.Subject ?? string.Empty;
        var email = payload.Email ?? string.Empty;
        var name = payload.Name ?? string.Empty;

        return new GoogleAuthResult(googleId, email, name, payload.EmailVerified);
    }
}
