using Finovo.Application.Common.Interfaces;
using Finovo.Domain.Entities.Invoices;
using Finovo.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Finovo.Infrastructure.Persistence.Repositories;

internal sealed class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _db;

    public InvoiceRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<Invoice>> GetAllAsync(Guid? customerId, InvoiceStatus? status, CancellationToken ct)
    {
        var query = _db.Invoices.AsQueryable();

        if (customerId.HasValue)
            query = query.Where(i => i.CustomerId == customerId.Value);

        if (status.HasValue)
            query = query.Where(i => i.Status == status.Value);

        return await query
            .OrderByDescending(i => i.IssueDate)
            .ThenByDescending(i => i.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<Invoice?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _db.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id, ct);

    public async Task<int> GetNextSequenceNumberAsync(Guid tenantId, int year, CancellationToken ct)
    {
        var max = await _db.Invoices
            .IgnoreQueryFilters()
            .Where(i => i.TenantId == tenantId && i.InvoiceNumber.Year == year)
            .Select(i => (int?)i.InvoiceNumber.SequenceNumber)
            .MaxAsync(ct);

        return (max ?? 0) + 1;
    }

    public async Task AddAsync(Invoice invoice, CancellationToken ct)
        => await _db.Invoices.AddAsync(invoice, ct);

    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
