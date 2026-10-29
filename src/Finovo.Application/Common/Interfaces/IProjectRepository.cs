using Finovo.Domain.Entities.Projects;
using Finovo.Domain.Enums;

namespace Finovo.Application.Common.Interfaces;

public interface IProjectRepository
{
    Task<IReadOnlyList<Project>> GetAllAsync(Guid? customerId, ProjectStatus? status, CancellationToken ct);
    Task<Project?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Dictionary<Guid, string>> GetNamesAsync(IEnumerable<Guid> projectIds, CancellationToken ct);
    Task AddAsync(Project project, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
