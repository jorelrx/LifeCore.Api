namespace LifeOS.Application.Abstractions.Security;

public interface IGoogleAuthService
{
    /// <summary>
    /// Validates a Google ID token issued to the mobile client.
    /// </summary>
    Task<GoogleAuthResult> ValidateIdTokenAsync(string idToken, CancellationToken cancellationToken = default);

}

public sealed record GoogleAuthResult(
    string GoogleId,
    string Email,
    string FullName,
    bool EmailVerified);
