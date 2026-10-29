using MediatR;

namespace Finovo.Application.Features.Invoices.Queries.GetInvoicePdf;

public record GetInvoicePdfQuery(Guid InvoiceId) : IRequest<byte[]>;
