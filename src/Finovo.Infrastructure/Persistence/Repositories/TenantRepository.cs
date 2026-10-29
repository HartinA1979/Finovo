using Finovo.Application.Common.Interfaces;
using Finovo.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;

namespace Finovo.Infrastructure.Persistence.Repositories;

internal sealed class TenantRepository : ITenantRepository
{
    private readonly AppDbContext _db;

    public TenantRepository(AppDbContext db) => _db = db;

    public async Task<Tenant?> GetByIdAsync(Guid tenantId, CancellationToken ct)
        => await _db.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId, ct);
}
