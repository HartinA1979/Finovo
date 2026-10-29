namespace Finovo.Application.Common.Interfaces;

public interface ITenantContext
{
    Guid CurrentTenantId { get; }

    /// <summary>
    /// True when the request comes from the admin subdomain (admin.finovo.at).
    /// Admin context has no tenant — use IgnoreQueryFilters() for admin queries.
    /// </summary>
    bool IsAdminContext { get; }
}
