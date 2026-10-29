using Finovo.Domain.Entities.Customers;

namespace Finovo.Application.Common.Interfaces;

public interface ICustomerRepository
{
    Task<IReadOnlyList<Customer>> GetAllAsync(string? nameFilter, bool? isActive, CancellationToken ct);
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Dictionary<Guid, string>> GetNamesAsync(IEnumerable<Guid> customerIds, CancellationToken ct);
    Task AddAsync(Customer customer, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
