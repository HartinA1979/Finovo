using System.Security.Claims;
using Finovo.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Finovo.Infrastructure.Identity;

internal sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    private ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;

    public string? UserId => Principal?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? Principal?.FindFirstValue("sub");

    public Guid? TenantId
    {
        get
        {
            var raw = Principal?.FindFirstValue("tenant_id");
            return raw is not null && Guid.TryParse(raw, out var id) ? id : null;
        }
    }

    public string? Role => Principal?.FindFirstValue(ClaimTypes.Role);

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;
}
