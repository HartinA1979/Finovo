using MediatR;

namespace Finovo.Application.Features.Invoices.Commands.MarkInvoicePaid;

public record MarkInvoicePaidCommand(Guid InvoiceId) : IRequest;
