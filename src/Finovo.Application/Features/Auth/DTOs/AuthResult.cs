namespace Finovo.Application.Features.Auth.DTOs;

public record AuthResult(
    string AccessToken,
    string RefreshToken,
    string UserId,
    string Email,
    string Role,
    Guid? TenantId,
    string? FirstName,
    string? LastName);
