namespace Finovo.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(bool Success, string? UserId, string? Error)> CreateUserAsync(
        string email, string password, string firstName, string lastName,
        Guid? tenantId, string role);

    /// <summary>Returns userId on success, null on invalid credentials.</summary>
    Task<string?> ValidateCredentialsAsync(string email, string password);

    Task<UserInfo?> GetUserInfoAsync(string userId);

    /// <summary>True when the tenant has no users yet (first registrant becomes Owner).</summary>
    Task<bool> IsFirstUserInTenantAsync(Guid tenantId);

    Task StoreRefreshTokenAsync(string userId, string token);
    Task<StoredRefreshToken?> GetRefreshTokenAsync(string token);
    Task RevokeRefreshTokenAsync(string token);
    Task RevokeAllUserRefreshTokensAsync(string userId);
}

public record UserInfo(string Id, string Email, string Role, Guid? TenantId, string? FirstName, string? LastName);
public record StoredRefreshToken(string UserId, DateTime ExpiresAt, bool IsRevoked);
