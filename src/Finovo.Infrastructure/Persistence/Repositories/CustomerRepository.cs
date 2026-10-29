using Finovo.Application.Common.Interfaces;
using Finovo.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;

namespace Finovo.Infrastructure.Persistence.Repositories;

internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;

    public CustomerRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<Customer>> GetAllAsync(string? nameFilter, bool? isActive, CancellationToken ct)
    {
        var query = _db.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(nameFilter))
            query = query.Where(c =>
                c.Name.Contains(nameFilter) ||
                (c.CompanyName != null && c.CompanyName.Contains(nameFilter)));

        if (isActive.HasValue)
            query = query.Where(c => c.IsActive == isActive.Value);

        return await query.OrderBy(c => c.Name).ToListAsync(ct);
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _db.Customers.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<Dictionary<Guid, string>> GetNamesAsync(IEnumerable<Guid> customerIds, CancellationToken ct)
    {
        var ids = customerIds.ToList();
        return await _db.Customers
            .Where(c => ids.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, c => c.Name, ct);
    }

    public async Task AddAsync(Customer customer, CancellationToken ct)
        => await _db.Customers.AddAsync(customer, ct);

    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
