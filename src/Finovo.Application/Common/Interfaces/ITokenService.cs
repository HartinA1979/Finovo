namespace Finovo.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(string userId, string email, Guid? tenantId, string role);
    string GenerateRefreshToken();
}
