using Finovo.Domain.Entities.Invoices;
using Finovo.Domain.Enums;

namespace Finovo.Application.Common.Interfaces;

public interface IInvoiceRepository
{
    Task<IReadOnlyList<Invoice>> GetAllAsync(Guid? customerId, InvoiceStatus? status, CancellationToken ct);
    Task<Invoice?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<int> GetNextSequenceNumberAsync(Guid tenantId, int year, CancellationToken ct);
    Task AddAsync(Invoice invoice, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
