using Finovo.Application.Common.Interfaces;
using Finovo.Domain.Entities.Projects;
using Finovo.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Finovo.Infrastructure.Persistence.Repositories;

internal sealed class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _db;

    public ProjectRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<Project>> GetAllAsync(Guid? customerId, ProjectStatus? status, CancellationToken ct)
    {
        var query = _db.Projects
            .Include(p => p.ServiceItems)
            .AsQueryable();

        if (customerId.HasValue)
            query = query.Where(p => p.CustomerId == customerId.Value);

        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _db.Projects
            .Include(p => p.ServiceItems)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Dictionary<Guid, string>> GetNamesAsync(IEnumerable<Guid> projectIds, CancellationToken ct)
    {
        var ids = projectIds.ToList();
        return await _db.Projects
            .Where(p => ids.Contains(p.Id))
            .Select(p => new { p.Id, p.Name })
            .ToDictionaryAsync(p => p.Id, p => p.Name, ct);
    }

    public async Task AddAsync(Project project, CancellationToken ct)
        => await _db.Projects.AddAsync(project, ct);

    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
