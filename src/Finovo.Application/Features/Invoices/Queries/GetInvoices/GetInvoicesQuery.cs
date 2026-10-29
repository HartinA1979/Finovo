using Finovo.Application.Features.Invoices.DTOs;
using Finovo.Domain.Enums;
using MediatR;

namespace Finovo.Application.Features.Invoices.Queries.GetInvoices;

public record GetInvoicesQuery(
    Guid? CustomerId,
    InvoiceStatus? Status) : IRequest<IReadOnlyList<InvoiceSummaryDto>>;
