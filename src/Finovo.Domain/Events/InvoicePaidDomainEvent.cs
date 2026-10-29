using Finovo.Domain.Common;
using Finovo.Domain.ValueObjects;

namespace Finovo.Domain.Events;

public record InvoicePaidDomainEvent(
    Guid InvoiceId,
    Guid TenantId,
    Guid ProjectId,
    Guid CustomerId,
    Money Amount) : IDomainEvent;
