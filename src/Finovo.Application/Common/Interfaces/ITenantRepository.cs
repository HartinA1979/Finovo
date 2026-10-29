using Finovo.Domain.Entities.Tenants;

namespace Finovo.Application.Common.Interfaces;

public interface ITenantRepository
{
    Task<Tenant?> GetByIdAsync(Guid tenantId, CancellationToken ct);
}
