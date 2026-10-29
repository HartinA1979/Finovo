namespace Finovo.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    Guid? TenantId { get; }
    string? Role { get; }
    bool IsAuthenticated { get; }
}
