using Finovo.Application.Common.Interfaces;

namespace Finovo.Infrastructure.Persistence;

/// <summary>
/// Scoped per-request. Middleware sets the tenant; services read it via ITenantContext.
/// </summary>
public sealed class TenantContext : ITenantContext, ITenantContextSetter
{
    public Guid CurrentTenantId { get; private set; }
    public bool IsAdminContext { get; private set; }

    public void SetTenantId(Guid tenantId)
    {
        CurrentTenantId = tenantId;
        IsAdminContext = false;
    }

    public void SetAdminContext()
    {
        CurrentTenantId = Guid.Empty;
        IsAdminContext = true;
    }
}
