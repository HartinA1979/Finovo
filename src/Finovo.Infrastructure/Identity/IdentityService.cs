using Finovo.Application.Common.Interfaces;
using Finovo.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Finovo.Infrastructure.Identity;

internal sealed class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _db;
    private readonly JwtSettings _jwtSettings;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        AppDbContext db,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _db = db;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<(bool Success, string? UserId, string? Error)> CreateUserAsync(
        string email, string password, string firstName, string lastName,
        Guid? tenantId, string role)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            TenantId = tenantId,
            Role = role,
            IsActive = true,
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var error = string.Join("; ", result.Errors.Select(e => e.Description));
            return (false, null, error);
        }

        return (true, user.Id, null);
    }

    public async Task<string?> ValidateCredentialsAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null || !user.IsActive)
            return null;

        return await _userManager.CheckPasswordAsync(user, password) ? user.Id : null;
    }

    public async Task<UserInfo?> GetUserInfoAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user is null
            ? null
            : new UserInfo(user.Id, user.Email!, user.Role, user.TenantId, user.FirstName, user.LastName);
    }

    public async Task<bool> IsFirstUserInTenantAsync(Guid tenantId)
        => !await _db.Users.AnyAsync(u => u.TenantId == tenantId);

    public async Task StoreRefreshTokenAsync(string userId, string token)
    {
        _db.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            CreatedAt = DateTime.UtcNow,
        });
        await _db.SaveChangesAsync();
    }

    public async Task<StoredRefreshToken?> GetRefreshTokenAsync(string token)
    {
        var rt = await _db.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Token == token);

        return rt is null ? null : new StoredRefreshToken(rt.UserId, rt.ExpiresAt, rt.IsRevoked);
    }

    public async Task RevokeRefreshTokenAsync(string token)
    {
        var rt = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        if (rt is not null)
        {
            rt.IsRevoked = true;
            await _db.SaveChangesAsync();
        }
    }

    public async Task RevokeAllUserRefreshTokensAsync(string userId)
    {
        await _db.RefreshTokens
            .Where(x => x.UserId == userId && !x.IsRevoked)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsRevoked, true));
    }
}
